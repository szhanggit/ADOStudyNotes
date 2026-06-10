using Dapper;
using Domain.Dto;
using Microsoft.AspNetCore.Http;
using Services.Message;
using TXC.Common.Data.TenantDbConnection;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Services;
using TXC.Common.Services.Storage;
using TXC.Common.Services.Wrappers;
using txc_common_lib.Filters.Models;
using TXC.Common.Domain;
using TXC.Common.MessageContract;
using Newtonsoft.Json;
using TXC.Common.Security.Cryptography;
using Services.Models;
using Microsoft.Extensions.Logging;
using TXC.Common.CacheManagement;
using System.Diagnostics.CodeAnalysis;

namespace Services.Command.ImageMedia
{
    [ExcludeFromCodeCoverageAttribute]
    public class DeleteImageMediaCommandHandler : ServiceHandlerBase, IRequestHandlerWrapper<DeleteImageMediaCommand, int>
    {
        private IDbConnection _dbConnection;
        private readonly IAzureBlobService _azureBlobService;
        private readonly string _tenantName;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly ITenantConfigHelper _tenantConfigHelper;
        private readonly int _tenantId;
        private readonly string _TX2UserName;
        private readonly ILogger<DeleteImageMediaCommandHandler> _logger;

        public DeleteImageMediaCommandHandler(ITenantDbConnection tenantDbConnection
                                             , IAzureBlobService azureBlobService
                                             , IHttpContextAccessor httpContextAccessor
                                             , IDapperOperation dapperOperation
                                            , ITX2ServiceBusSender txcServiceBusSender
                                            , ITenantConfigHelper tenantConfigHelper
                                            , ILogger<DeleteImageMediaCommandHandler> logger
                                             ) : base(tenantDbConnection, dapperOperation)
        {
            _azureBlobService = azureBlobService;
            _tenantName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantName];
            _tenantId = int.Parse(httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TenantId]);
            _TX2UserName = httpContextAccessor.HttpContext?.Request?.Headers[HeaderConstants.TX2UserName];
            _tenantConfigHelper = tenantConfigHelper;
            _txcServiceBusSender = txcServiceBusSender;
            _logger = logger;
        }
        public async Task<Response<int>> Handle(DeleteImageMediaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check tx2 connector config
                var queueNameConfig = await _tenantConfigHelper.GetTenantConfigValue("TX2ConnectorQueueName", _tenantId);
                var containerNameConfig = await _tenantConfigHelper.GetTenantConfigValue("ContainerName", _tenantId);

                // initialize db connection
                var conn = await _tenantDbConnection.GetTenantDbConnection(_tenantId.ToString(),false, cancellationToken);

                if (!conn.Success)
                {

                    return Response.Fail("Error in Tenant DB", 0);
                }
                _dbConnection = conn.Data;

                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@MediaId", request.MediaId, DbType.String, ParameterDirection.Input);

                CommandDefinition commandDefinition = new CommandDefinition(SqlDeleteMedia(), commandType: CommandType.Text,
                                                                        parameters: parameters, cancellationToken: cancellationToken);

                await _dapperOperation.ProcessSql<ExecuteCommand, int>(_dbConnection, commandDefinition);

                await _azureBlobService.DeleteAsync(containerNameConfig.Value, request.BlobName, cancellationToken);

                var message = new DeleteMediaMessageV1
                {
                    Id = request.MediaId,
                };

                //send to service bus
                await _txcServiceBusSender.SendMessageAsync(_tenantId, queueNameConfig.Value, message, ESBMessageType.Media, (int)EMediaMessageActionType.Delete, "Media", 1);

                return Response.Success("Success", request.MediaId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteImageMediaCommandHandler Error");
                return Response.Fail<int>("Exception", 0);
            }
        }

        private string SqlDeleteMedia()
        {
            return @"
               DELETE FROM media.tb_m_media WHERE media_id = @MediaId
            ";
        }
    }
}