using Moq;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using Xunit;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TXC.Common.CacheManagement;
using TXC.Common.MessageContract;
using TXC.Common.Services.Storage;
using Services.Command.ImageMedia;
using Microsoft.AspNetCore.Http;

namespace UnitTest.Test.CoreServices.Media
{
    public class DeleteImageMediaCommandHandleTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var dbconn = new Mock<IDbConnection>();
            var IAzure = new Mock<IAzureBlobService>();
            var txcServiceBusSender = new Mock<ITX2ServiceBusSender>();
            var tenantConfigHelper = new Mock<ITenantConfigHelper>();
            var logger = new Mock<ILogger<DeleteImageMediaCommandHandler>>();
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var dapperOperation = new Mock<IDapperOperation>();
            var mediator = new Mock<IMediator>();

            var tenantDbResponse = Response.Success("success", dbconn.Object);
            
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

            var deleteImageMediaCommandHandler = new DeleteImageMediaCommandHandler(
                tenantDbConnection.Object,
                IAzure.Object,
                httpContextAccessor.Object,
                dapperOperation.Object,
                txcServiceBusSender.Object,
                tenantConfigHelper.Object,
                logger.Object);

            var request = new DeleteImageMediaCommand
            {
                MediaId = 1,
                BlobName = "Test"
            };

            var result = await deleteImageMediaCommandHandler.Handle(request, cancellationToken: CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
