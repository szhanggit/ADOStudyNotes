using System.Threading.Tasks;
using TXC.Proto.Media;
using System;
using TXC.Common.Services.Storage;
using Domain.Models.ConfigOptions;
using System.Data;
using TXC.Common.MessageContract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TXC.Common.Data;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Services.Storage.Model;
using System.Threading;
using Domain.Dto;
using MediatR;
using Services.Queries.ImageMedia;
using System.IO;
using Domain.EnumList;
using Dapper;
using Services.Models;
using Google.Protobuf.WellKnownTypes;
using TXC.Common.CacheManagement;
using Domain.Models.Request;
using System.Data.SqlClient;
using static Repository.MediaUnit;

namespace Services.Core
{
    public interface ICreateMediaService
    {
        Task<ProtoBaseResponse> CreateMedia(CreateMediaRequest request);
    }

    public class CreateMediaService : ServiceHandlerBase, ICreateMediaService
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly StoragePathConfiguration _directoryConfig;
        private IDbConnection _dbConnection;
        private readonly ITX2ServiceBusSender _txcServiceBusSender;
        private readonly CdnConfiguration _cdnConfig;
        private readonly ITenantConfigHelper _tenantConfigHelper;
        private readonly ILogger<CreateMediaService> _logger;
        private readonly IMediaUnitOfWork _mediaUnit;
        public CreateMediaService(ITenantDbConnection tenantDbConnection,
            IAzureBlobService azureBlobService,
            IOptions<StoragePathConfiguration> directoryConfig,
            IOptions<CdnConfiguration> cdnConfig,
            IDapperOperation dapperOperation,
            ITX2ServiceBusSender txcServiceBusSender,
            ITenantConfigHelper tenantConfigHelper,
            ILogger<CreateMediaService> logger,
            IMediaUnitOfWork mediaUnit) : base(tenantDbConnection, dapperOperation)
        {
            _cdnConfig = cdnConfig.Value;
            _azureBlobService = azureBlobService;
            _directoryConfig = directoryConfig.Value;
            _txcServiceBusSender = txcServiceBusSender;
            _tenantConfigHelper = tenantConfigHelper;
            _logger = logger;
            _mediaUnit = mediaUnit;
        }
        public async Task<ProtoBaseResponse> CreateMedia(CreateMediaRequest request)
        {
            MediaUploadDto mediaUploadDto = null;
            ProtoBaseResponse failed = new ProtoBaseResponse 
            { 
                Success = false,
                Message = "failed to upload image",
                Data = null
            };
            try
            {
                if (string.IsNullOrEmpty(request.TenantName))
                {
                    failed.Message = "missing tenant name";
                    return failed;
                }

                //check tx2 connector config
                var queueNameConfig = await _tenantConfigHelper.GetTenantConfigValue("TX2ConnectorQueueName", request.TenantId);
                var containerNameConfig = await _tenantConfigHelper.GetTenantConfigValue("ContainerName", request.TenantId);
                
                var conn = await _tenantDbConnection.GetTenantDbConnection(request.TenantId.ToString(), false, CancellationToken.None);

                if (!conn.Success)
                {
                    failed.Message = conn.Message;
                    return failed;
                }
                _dbConnection = conn.Data;
                _mediaUnit.SetConnection(conn.Data);


                Stream imageStream = new MemoryStream(request.Image.ToByteArray());

                mediaUploadDto = new MediaUploadDto()
                {
                    TenantName = containerNameConfig.Value, 
                    FileName = request.FileName,
                    MainPath = _directoryConfig.MainPath,
                    File = imageStream,
                    ContentType = request.ContentType
                };

                /*
                GetAnyMediaNameTypeQuery nameTypeQuery = new GetAnyMediaNameTypeQuery
                {
                    Keyword = Path.GetFileNameWithoutExtension(mediaUploadDto.FileName),
                    Type = (ImageCategory)request.Type
                };

                var responseNameTypeQueryHandler = await _mediator.Send(nameTypeQuery, CancellationToken.None);

                if (responseNameTypeQueryHandler.Success == false)
                {
                    failed.Message = "error in checking existing image";
                    return failed;
                }

                var existsInDb = responseNameTypeQueryHandler.Data;

                if (existsInDb)
                {
                    failed.Message = "image already exists";
                    return failed;
                }
                */

                var response = await _azureBlobService.UploadAsync(mediaUploadDto, CancellationToken.None);

                if (!response.Success)
                {
                    failed.Message = response.Message;
                    return failed;
                }

                if (response.Data == null)
                {
                    failed.Message = "invalid model";
                    return failed;
                }

                var fileName = response.Data.GetFileName();
                var dotIndex = fileName.IndexOf('.');
                if (dotIndex > 0)
                {
                    fileName = fileName.Substring(0, dotIndex);
                }


                var mediaId = await _mediaUnit.MediaRepository.Add(new Entities.Media
                {
                    File_Name = fileName,
                    File_Content_Type = request.ContentType,
                    Node_Url = response.Data.Url,
                    Account = response.Data.AccountName,
                    Blob_Name = response.Data.Name,
                    Type = request.Type,
                    Height = Convert.ToInt32(request.ImageHeight),
                    Width = Convert.ToInt32(request.ImageWidth),
                    Keyword = mediaUploadDto.GetKeyWord()
                });


                if (mediaId < 1)
                {
                    await _azureBlobService.DeleteAsync(mediaUploadDto.TenantName, mediaUploadDto.GetBlobName(), CancellationToken.None);
                    
                    return failed;
                }

                if (mediaId > 0)
                {
                    var message = new CreateMediaMessageV1
                    {
                        Id = mediaId,
                        FileName = fileName,
                        Keyword = mediaUploadDto.GetKeyWord(),
                        Height = Convert.ToInt32(request.ImageHeight),
                        Width = Convert.ToInt32(request.ImageWidth),
                        Url = $"{_cdnConfig.ImageCdnUri}{ response.Data.Url}",
                        MediaCategory = (int)request.Type,
                        TX2UserName = request.TX2UserName
                    };

                    //send to service bus
                    await _txcServiceBusSender.SendMessageAsync(request.TenantId, queueNameConfig.Value, message, ESBMessageType.Media, (int)EMediaMessageActionType.Create, "TXCMedia", 1);

                    CreateMediaResponse grpcresponse = new CreateMediaResponse
                    {
                        MediaId = mediaId
                    };

                    return new ProtoBaseResponse 
                    { 
                        Success = true,
                        Message = "Sucess",
                        Data = Any.Pack(grpcresponse)

                    };
                }

                return failed;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateImageMediaCommandHandler Error");
                if (mediaUploadDto != null)
                    await _azureBlobService.DeleteAsync(mediaUploadDto.TenantName, mediaUploadDto.GetBlobName(), CancellationToken.None);
                return failed;
            }
        }

    }
}
