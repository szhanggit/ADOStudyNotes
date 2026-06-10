using Dapper;
using Domain.Dto;
using Domain.Models.ConfigOptions;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.CDN;
using Services.Models;
using Services.Queries.ImageMedia;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using TXC.Common.MessageContract;
using TXC.Common.Services.Storage;
using TXC.Common.Services.Storage.Model;
using TXC.Proto.Media;
using TXC.Common.CacheManagement;
using Domain.Models.Request;
using Microsoft.Data.SqlClient;
using static Repository.MediaUnit;

namespace Services.Core
{
    public interface IReplaceMediaService
    {
        Task<ProtoBaseResponse> ReplaceMedia(ReplaceMediaRequest request);
    }
    public class ReplaceMediaService : ServiceHandlerBase, IReplaceMediaService
    {
        private IDbConnection _dbConnection;
        private readonly IAzureBlobService _azureBlobService;
        private readonly StoragePathConfiguration _directoryConfig;
        private readonly CdnConfiguration _cdnConfig;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly ITenantConfigHelper _tenantConfigHelper;
        private readonly ILogger<ReplaceMediaService> _logger;
        private readonly ICdnHelper _cdnHelper;
        private readonly IMediaUnitOfWork _mediaUnit;
        private readonly IGetMediaByIdService _getMediaByIdService;
        public ReplaceMediaService(ITenantDbConnection tenantDbConnection
            ,IAzureBlobService azureBlobService
            ,IOptions<StoragePathConfiguration> directoryConfig
            ,IDapperOperation dapperOperation
            ,IOptions<CdnConfiguration> cdnConfig
            ,ITX2ServiceBusSender txcServiceBusSender
            ,ITenantConfigHelper tenantConfigHelper
            ,ILogger<ReplaceMediaService> logger
            ,ICdnHelper cdnHelper
            ,IMediaUnitOfWork mediaUnit
            , IGetMediaByIdService getMediaByIdService) : base(tenantDbConnection, dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
            _azureBlobService = azureBlobService;
            _directoryConfig = directoryConfig.Value;
            _tenantConfigHelper = tenantConfigHelper;
            _txcServiceBusSender = txcServiceBusSender;
            _logger = logger;
            _cdnHelper = cdnHelper;
            _mediaUnit = mediaUnit;
            _getMediaByIdService = getMediaByIdService;

        }
        public async Task<ProtoBaseResponse> ReplaceMedia(ReplaceMediaRequest request)
        {
            ProtoBaseResponse failed = new ProtoBaseResponse
            {
                Success = false,
                Message = "failed to replace image",
                Data = null
            };
            try
            {

                //check tx2 connector config
                var queueNameConfig = await _tenantConfigHelper.GetTenantConfigValue("TX2ConnectorQueueName", request.TenantId);
                var containerNameConfig = await _tenantConfigHelper.GetTenantConfigValue("ContainerName", request.TenantId);

                // initialize db connection
                var conn = await _tenantDbConnection.GetTenantDbConnection(request.TenantId.ToString(),false, CancellationToken.None);

                if (!conn.Success)
                {
                    failed.Message = "error in getting database connection";
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

                Stream imageStream = new MemoryStream(request.Image.ToByteArray());

                MediaReplaceDto mediaReplaceDto = new MediaReplaceDto()
                {
                    TenantName = containerNameConfig.Value,
                    BlobName = res.BlobName,
                    FileName = $"{Path.GetFileNameWithoutExtension(res.KeyWord)}{Path.GetExtension(request.FileName)}",
                    File = imageStream,
                    MainPath = _directoryConfig.MainPath,
                    ContentType = request.ContentType
                };

                var existsInAzure = await _azureBlobService.ExistsAsync(containerNameConfig.Value, mediaReplaceDto.BlobName);

                if (!existsInAzure.Data)
                {
                    failed.Message = "image not exists";
                    return failed;
                }

                var response = await _azureBlobService.ReplaceAsync(mediaReplaceDto, CancellationToken.None);
                if (!response.Success)
                {
                    failed.Message = response.Message;
                    return failed;
                }
                if (response.Data == null)
                {

                    return failed;
                }
                var fileName = response.Data.GetFileName();
                var dotIndex = fileName.IndexOf('.');
                if (dotIndex > 0)
                {
                    fileName = fileName.Substring(0, dotIndex);
                }


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

                //await _mediaUnit.MediaRepository.ReplaceMedia(new ReplaceMediaRequestModel
                //{
                //    MediaId = request.MediaId,
                //    FileName = fileName,
                //    FileContentType = request.ContentType,
                //    NodeUrl = response.Data.Url,
                //    BlobName = response.Data.Name,
                //    Height = request.ImageHeight,
                //    Width = request.ImageWidth,
                //    Keyword = mediaReplaceDto.GetKeyWord()
                //});

                var message = new ReplaceMediaMessageV1
                {
                    Id = request.MediaId,
                    FileName = fileName,
                    Keyword = mediaReplaceDto.GetKeyWord(),
                    Height = Convert.ToInt32(request.ImageHeight),
                    Width = Convert.ToInt32(request.ImageWidth),
                    Url = $"{_cdnConfig.ImageCdnUri}{ response.Data.Url}",
                    TX2UserName = request.TX2UserName
                };

                _cdnHelper.PurgeCdnEndpoints(new System.Collections.Generic.List<string>() { response.Data.Url });
                //send to service bus
                await _txcServiceBusSender.SendMessageAsync(request.TenantId, queueNameConfig.Value, message, ESBMessageType.Media, (int)EMediaMessageActionType.Replace, "Media", 1);
                ReplaceMediaResponse grpcResponse = new ReplaceMediaResponse
                { 
                    MediaId = request.MediaId
                };
                return new ProtoBaseResponse 
                { 
                    Success = true,
                    Message = "success",
                    Data = Any.Pack(grpcResponse)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReplaceImageMediaCommandHandler Error");
                return failed;
            }
        }

        
    }
}
