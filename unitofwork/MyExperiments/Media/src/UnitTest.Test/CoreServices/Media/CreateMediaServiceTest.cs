using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Logging;
using Xunit;
using Domain.Constant;
using TXC.Proto.Media;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Google.Protobuf.WellKnownTypes;
using FluentValidation.TestHelper;
using Dapper;
using Microsoft.ApplicationInsights.Channel;
using Services.Core;
using Domain.Models.ConfigOptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TXC.Common.CacheManagement;
using TXC.Common.MessageContract;
using TXC.Common.Services.Storage;
using Google.Protobuf;
using TXC.Common.Services.Storage.Model;
using SixLabors.ImageSharp;
using System.IO;
using System.Data.SqlClient;
using Repository;
using static Repository.Repositories.MediaRepo;
using Domain.Models.Request;
using static Repository.MediaUnit;

namespace UnitTest.Test.CoreServices.Media
{
    public class CreateMediaServiceTest
    {
        Mock<ICreateMediaService> _svc = new Mock<ICreateMediaService>();
        [Fact]
        public async Task ShouldSuccess()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var IAzure = new Mock<IAzureBlobService>();
            var directoryConfig = new Mock<IOptions<StoragePathConfiguration>>();
            var cdnConfig = new Mock<IOptions<CdnConfiguration>>();
            var dapperOperation = new Mock<IDapperOperation>();
            var txcServiceBusSender = new Mock<ITX2ServiceBusSender>();
            var tenantConfigHelper = new Mock<ITenantConfigHelper>();
            var logger = new Mock<ILogger<CreateMediaService>>();
            var mediator = new Mock<IMediator>();
            var dbconn = new Mock<IDbConnection>();
            var mediaRepo = new Mock<IMediaRepository>();
            var context = new Mock<Context>();
            

            var tenantDbResponse = Response.Success("success", dbconn.Object);

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

            IAzure.Setup(f => f.UploadAsync(It.IsAny<MediaUploadDto>(), CancellationToken.None))
                .ReturnsAsync(new Response<BlobMediaInfo>() { Success = true, Data = new BlobMediaInfo() { Name = "TEST NAME" } });

            cdnConfig.Setup(f => f.Value).Returns(new CdnConfiguration() { ImageCdnUri = "TestUri" });

            mediaRepo.Setup(s => s.Add(It.IsAny<Entities.Media>()))
                .ReturnsAsync(1);

            var mediaUnit = new MediaUnitOfWork(context.Object, mediaRepo.Object);

            var createMediaSvc = new CreateMediaService(tenantDbConnection.Object
                , IAzure.Object
                , directoryConfig.Object
                , cdnConfig.Object
                , dapperOperation.Object
                , txcServiceBusSender.Object
                , tenantConfigHelper.Object
                , logger.Object
                , mediaUnit);

            var request = new CreateMediaRequest
            {
                FileName = "testFile",
                ContentType = "png",                
                Image = ByteString.CopyFrom("e#>&*m16", Encoding.Unicode),                
                Type = 1,
                ImageHeight = "100",
                ImageWidth= "100",
                TenantId = 9,
                TenantName = "GL"
            };

            var res = await createMediaSvc.CreateMedia(request);
            Assert.True(res.Success);


        }

    }
}
