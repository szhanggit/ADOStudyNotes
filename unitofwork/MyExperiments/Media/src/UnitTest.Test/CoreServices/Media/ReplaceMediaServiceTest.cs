using Moq;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using Xunit;
using TXC.Proto.Media;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Dapper;
using Services.Core;
using Domain.Models.ConfigOptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TXC.Common.CacheManagement;
using TXC.Common.MessageContract;
using Microsoft.AspNetCore.Http;
using Services.Queries.ImageMedia;
using Domain.Dto;
using Repository;
using static Repository.Repositories.MediaRepo;
using Domain.Models.Request;
using Domain.Models.Response;
using Services.Models;
using Google.Protobuf.WellKnownTypes;
using TXC.Common.Services.Storage;
using TXC.Common.Services.Storage.Model;
using static Repository.MediaUnit;
using Services.CDN;
using Google.Protobuf;
using System.Text;

namespace UnitTest.Test.CoreServices.Media
{
    public class ReplaceMediaServiceTest
    {
        Mock<IReplaceMediaService> _svc = new Mock<IReplaceMediaService>();
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var IAzure = new Mock<IAzureBlobService>();
            var directoryConfig = new Mock<IOptions<StoragePathConfiguration>>();
            var cdnConfig = new Mock<IOptions<CdnConfiguration>>();
            var dapperOperation = new Mock<IDapperOperation>();
            var txcServiceBusSender = new Mock<ITX2ServiceBusSender>();
            var tenantConfigHelper = new Mock<ITenantConfigHelper>();
            var logger = new Mock<ILogger<ReplaceMediaService>>();
            var mediator = new Mock<IMediator>();
            var dbconn = new Mock<IDbConnection>();
            var cdnHelper = new Mock<ICdnHelper>();
            var context = new Mock<Context>();
            var mediaRepo = new Mock<IMediaRepository>();
            var getMediaByIdService = new Mock<IGetMediaByIdService>();

            var tenantDbResponse = Response.Success("success", dbconn.Object);

            var cdn = new CdnConfiguration { ImageCdnUri = "url" };
            cdnConfig.Setup(s => s.Value)
                .Returns(cdn);

            tenantDbConnection.Setup
                (
                    f => f.GetTenantDbConnection(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())

                ).ReturnsAsync(tenantDbResponse);

            dapperOperation.Setup(
                 f => f.ProcessSql<ExecuteCommand, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()))
                 .ReturnsAsync(1);

            tenantConfigHelper.Setup(
                f => f.GetTenantConfigValue("TX2ConnectorQueueName", It.IsAny<int>()))
                .ReturnsAsync(new TenantConfig());

            tenantConfigHelper.Setup(
                f => f.GetTenantConfigValue("ContainerName", It.IsAny<int>()))
                .ReturnsAsync(new TenantConfig() { ConfigName = "Test" });

            directoryConfig.Setup(f => f.Value).Returns(new StoragePathConfiguration());

            IAzure.Setup(f => f.ExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Response<bool> { Success = true, Data = true });


            IAzure.Setup(f => f.ReplaceAsync(It.IsAny<MediaReplaceDto>(), CancellationToken.None))
                .ReturnsAsync(new Response<BlobMediaInfo>() { Success = true, Data = new BlobMediaInfo() { Name = "TEST NAME" } });



            cdnConfig.Setup(f => f.Value).Returns(new CdnConfiguration() { ImageCdnUri = "TestUri" });

            mediaRepo.Setup(s => s.Update(It.IsAny<Entities.Media>()))
                .ReturnsAsync(true);

            var res = new ProtoBaseResponse()
            {
                Message = "success"
               ,
                Success = true
               ,
                Data = Any.Pack(new GetMediaByIdResponse()
                {
                    MediaId = 1,
                    FileName = "test",
                    FileContentType = "png",
                    Url = "url",
                    Account = "test acc",
                    BlobName = "test",
                    MediaCategory = 1,
                    Height = 100,
                    Width = 100,
                    KeyWord = "test"
                })
            };

            getMediaByIdService.Setup(s => s.GetMediaId(It.IsAny<GetMediaByIdRequest>()))
                .ReturnsAsync(res);

            var mediaUnit = new MediaUnitOfWork(context.Object, mediaRepo.Object);

            var replaceMediaSvc = new ReplaceMediaService(
                tenantDbConnection.Object
                , IAzure.Object
                , directoryConfig.Object                
                , dapperOperation.Object
                , cdnConfig.Object
                , txcServiceBusSender.Object
                , tenantConfigHelper.Object
                , logger.Object
                , cdnHelper.Object
                , mediaUnit
                , getMediaByIdService.Object
                );

            var req = new ReplaceMediaRequest
            {
                FileName = "testFile",
                ContentType = "png",
                Image = ByteString.CopyFrom("e#>&*m16", Encoding.Unicode),
                BlobName = "testFile",
                ImageHeight = "100",
                ImageWidth = "100",
                TenantId = 9,
                TenantName = "GL"
            };
            var result = await replaceMediaSvc.ReplaceMedia(req);
            Assert.True(result.Success);
        }

    }

    
}
