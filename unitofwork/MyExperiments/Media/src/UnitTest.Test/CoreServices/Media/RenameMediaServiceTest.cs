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
using System.Diagnostics.CodeAnalysis;
using static Repository.MediaUnit;

namespace UnitTest.Test.CoreServices.Media
{
    [ExcludeFromCodeCoverageAttribute]
    public class RenameMediaServiceTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var cdnConfig = new Mock<IOptions<CdnConfiguration>>();
            var dapperOperation = new Mock<IDapperOperation>();
            var txcServiceBusSender = new Mock<ITX2ServiceBusSender>();
            var tenantConfigHelper = new Mock<ITenantConfigHelper>();
            var logger = new Mock<ILogger<RenameMediaService>>();            
            var dbconn = new Mock<IDbConnection>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

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
            httpContextAccessor.Setup(f => f.HttpContext.Request.Headers[HeaderConstants.TenantId]).Returns("1");
            httpContextAccessor.Setup(f => f.HttpContext.Request.Headers[HeaderConstants.TenantName]).Returns(HeaderConstants.TenantName);

            tenantConfigHelper.Setup(
                f => f.GetTenantConfigValue("TX2ConnectorQueueName", It.IsAny<int>()))
                .ReturnsAsync(new TenantConfig());

            tenantConfigHelper.Setup(
                f => f.GetTenantConfigValue("ContainerName", It.IsAny<int>()))
                .ReturnsAsync(new TenantConfig() { ConfigName = "Test" });

            
            var request = new RenameMediaRequest
            {
                MediaId = 1,
                TenantId = 5,
                KeyWord ="New Name",
                TX2UserName = "GR"
            };


            txcServiceBusSender.Setup(s => s.SendMessageAsync(1, "", new RenameMediaMessageV1() { }, ESBMessageType.Media, 1, "", 1));


            mediaRepo.Setup(s => s.Update(It.IsAny<Entities.Media>()))
                .ReturnsAsync(true);

            var res = new ProtoBaseResponse() 
            { 
                Message = "success"
                , Success = true 
                , Data = Any.Pack(new GetMediaByIdResponse()
                {
                    MediaId =1,
                    FileName="test",
                    FileContentType ="png",
                    Url = "url",
                    Account ="test acc",
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

            var renameMediaSvc = new RenameMediaService(
                tenantDbConnection.Object
                , dapperOperation.Object
                , txcServiceBusSender.Object
                , tenantConfigHelper.Object
                , logger.Object
                , mediaUnit
                , getMediaByIdService.Object
                , cdnConfig.Object);


            var req = new RenameMediaRequest
            {
               MediaId = 1,
               KeyWord = "KeyWord",
               TenantId = 9
            };

            var result = await renameMediaSvc.RenameMedia(req);
            Assert.True(result.Success);
        }
    }
}
