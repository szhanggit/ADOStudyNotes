using Moq;
using System.Threading.Tasks;
using TXC.Common.Data.TenantDbConnection;
using Xunit;
using TXC.Common.Data;
using System.Threading;
using System.Data;
using TXC.Common.Domain;
using Dapper;
using Domain.Models.ConfigOptions;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Services.Queries.ImageMedia;
using Services.Utility.GraphQLClient;
using Services.Core;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using TXC.Proto.Media;

namespace UnitTest.Test.CoreServices.Media
{
    public class GetMediaByIdServiceHandlerTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var cdnConfig = new Mock<IOptions<CdnConfiguration>>();
            var dapperOperation = new Mock<IDapperOperation>();
            var dbconn = new Mock<IDbConnection>();
            var graphQLClient = new Mock<IMediaGraphQLClient>();


            var tenantDbResponse = Response.Success("success", dbconn.Object);

            tenantDbConnection.Setup
                (
                    f => f.GetTenantDbConnection(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())

                ).ReturnsAsync(tenantDbResponse);

            dapperOperation.Setup(
                 f => f.ProcessSql<ExecuteCommand, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()))
                 .ReturnsAsync(1);

            cdnConfig.Setup(f => f.Value).Returns(new CdnConfiguration() { ImageCdnUri = "TestUri" });


            var request = new GetMediaByIdRequest
            {
                MediaId = 1,
                TenantId = 9
            };

            graphQLClient.Setup(s => s.GetGraphQLClient(9));

            var getMediaByIdSvc = new GetMediaByIdService(
                tenantDbConnection.Object,
                cdnConfig.Object,
                dapperOperation.Object,
                graphQLClient.Object);

            var res = await getMediaByIdSvc.GetMediaId(request);
            Assert.NotNull(res);
        }
    }
}
