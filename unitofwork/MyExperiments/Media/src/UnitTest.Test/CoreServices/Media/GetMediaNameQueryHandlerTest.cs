using Moq;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using Xunit;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Dapper;
using Microsoft.AspNetCore.Http;
using Services.Queries.ImageMedia;

namespace UnitTest.Test.CoreServices.Media
{
    public class GetMediaNameQueryHandlerTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var dapperOperation = new Mock<IDapperOperation>();
            var dbconn = new Mock<IDbConnection>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

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

            var getMediaNameQueryHandler = new GetMediaNameQueryHandler(
                tenantDbConnection.Object,
                dapperOperation.Object,
                httpContextAccessor.Object
                );

            var request = new GetMediaNameQuery
            {
                MediaId = 1,
                BlobName = "TestName"
            };

            var result = await getMediaNameQueryHandler.Handle(request, cancellationToken: CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
