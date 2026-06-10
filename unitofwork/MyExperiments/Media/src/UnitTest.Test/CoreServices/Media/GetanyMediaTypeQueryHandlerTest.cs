using Moq;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using Xunit;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Dapper;
using TXC.Common.CacheManagement;
using Microsoft.AspNetCore.Http;
using Services.Queries.ImageMedia;

namespace UnitTest.Test.CoreServices.Media
{
    public class GetanyMediaTypeQueryHandlerTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var dapperOperation = new Mock<IDapperOperation>();
            var tenantConfigHelper = new Mock<ITenantConfigHelper>();
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

            var getAnyMediaNameTypeQueryHandler = new GetAnyMediaNameTypeQueryHandler(
                tenantDbConnection.Object,
                dapperOperation.Object,
                httpContextAccessor.Object
                );

            var request = new GetAnyMediaNameTypeQuery
            {
                Keyword = "NewName",
                Type = Domain.EnumList.ImageCategory.Client
            };

            var result = await getAnyMediaNameTypeQueryHandler.Handle(request, cancellationToken: CancellationToken.None);

            Assert.True(result.Success);
        }
    }
}
