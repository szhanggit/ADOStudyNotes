using Dapper;
using Domain.Dto;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using Services.Models;
using Services.Queries.ImageMedia;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.CacheManagement.Interface;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.MessageContract;
using TXC.Proto.Media;
using TXC.Common.CacheManagement;
using Domain.Models.Request;
using Microsoft.Data.SqlClient;
using static Repository.MediaUnit;
using Domain.Models.ConfigOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Services.Core
{
    public interface IRenameMediaService
    {
        Task<ProtoBaseResponse> RenameMedia(RenameMediaRequest request);
    }
    public class RenameMediaService : ServiceHandlerBase, IRenameMediaService
    {
        private readonly CdnConfiguration _cdnConfig;
        private IDbConnection _dbConnection;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly ITenantConfigHelper _tenantConfigHelper;
        private readonly ILogger<RenameMediaService> _logger;
        private readonly IMediaUnitOfWork _mediaUnit;
        private readonly IGetMediaByIdService _getMediaByIdService;
        public RenameMediaService(ITenantDbConnection tenantDbConnection,
            IDapperOperation dapperOperation,
            ITX2ServiceBusSender txcServiceBusSender,
            ITenantConfigHelper tenantConfigHelper,
            ILogger<RenameMediaService> logger,
            IMediaUnitOfWork mediaUnit,
            IGetMediaByIdService getMediaByIdService,
            IOptions<CdnConfiguration> cdnConfig) : base(tenantDbConnection, dapperOperation)
        {
            _tenantConfigHelper = tenantConfigHelper;
            _txcServiceBusSender = txcServiceBusSender;
            _logger = logger;
            _mediaUnit = mediaUnit;
            _getMediaByIdService = getMediaByIdService;
            _cdnConfig = cdnConfig.Value;
        }

        public async Task<ProtoBaseResponse> RenameMedia(RenameMediaRequest request)
        {
            ProtoBaseResponse failed = new ProtoBaseResponse
            {
                Success = false,
                Message = "failed to upload image",
                Data = null
            };
            try
            {

                //check tx2 connector config
                var queueNameConfig = await _tenantConfigHelper.GetTenantConfigValue("TX2ConnectorQueueName", request.TenantId);
                var containerNameConfig = await _tenantConfigHelper.GetTenantConfigValue("ContainerName", request.TenantId);

                var conn = await _tenantDbConnection.GetTenantDbConnection(request.TenantId.ToString(), false, CancellationToken.None);

                if (!conn.Success)
                {
                    failed.Message = "error in getting tenant connection";
                    return failed;
                }
                _dbConnection = conn.Data;
                _mediaUnit.SetConnection(conn.Data);

                var svcResponse = await _getMediaByIdService.GetMediaId(new GetMediaByIdRequest { MediaId = request.MediaId, TenantId = request.TenantId }); //await _mediaUnit.MediaRepository.GetMediaById(request.MediaId);

                if (svcResponse == null)
                {
                    failed.Message = "media not exists";
                    return failed;
                }
                var res = svcResponse.Data.Unpack<GetMediaByIdResponse>();
                res.KeyWord = request.KeyWord;
                await _mediaUnit.MediaRepository.Update(new Entities.Media
                {
                    Media_Id = res.MediaId,
                    File_Name = res.FileName,
                    File_Content_Type = res.FileContentType,
                    Node_Url = res.Url.Replace(_cdnConfig.ImageCdnUri, ""),
                    Account = res.Account,
                    Blob_Name = res.BlobName,
                    Type = res.MediaCategory,
                    Height = res.Height,
                    Width = res.Width,
                    Keyword = res.KeyWord

                });

                var message = new RenameMediaMessageV1
                {
                    Id = request.MediaId,
                    Keyword = request.KeyWord,
                    TX2UserName = request.TX2UserName
                };

                //send to service bus
                await _txcServiceBusSender.SendMessageAsync(request.TenantId, queueNameConfig.Value, message, ESBMessageType.Media, (int)EMediaMessageActionType.Rename, "Media", 1);

                RenameMediaResponse response = new RenameMediaResponse
                {
                    MediaId = request.MediaId
                };

                var success = new ProtoBaseResponse
                {
                    Success = true,
                    Message = "success",
                    Data = Any.Pack(response)
                };

                return success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RenameImageMedia Error");
                return failed;
            }
        }

    }
}
