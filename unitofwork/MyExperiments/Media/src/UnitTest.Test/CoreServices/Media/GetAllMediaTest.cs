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
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Repository;
using static Repository.Repositories.MediaRepo;
using Services.Utility.GraphQLClient;
using System.Diagnostics.CodeAnalysis;
using Castle.Core.Configuration;
using Services.GraphQLResponse;
using GraphQL;
using Google.Protobuf.WellKnownTypes;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace UnitTest.Test.CoreServices.Media
{
    [ExcludeFromCodeCoverageAttribute]
    public class GetAllMediaTest
    {
        [Fact]
        public async Task Should_Success()
        {
            var tenantDbConnection = new Mock<ITenantDbConnection>();
            var cdnConfig = new Mock<IOptions<CdnConfiguration>>();
            var dapperOperation = new Mock<IDapperOperation>();
            var dbconn = new Mock<IDbConnection>();            
            var context = new Mock<Context>();
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



            var request = new GetAllMediaRequest
            {
                TenantId = 5,
                MediaCategory = 1,
                SearchKey = ""
            };


            graphQLClient.Setup(s => s.GetGraphQLClient(9))
                .Returns<GraphQLHttpClient>(r=>r);

            var getAllMediaService = new GetAllMediaService(
                tenantDbConnection.Object,
                cdnConfig.Object,
                dapperOperation.Object,
                graphQLClient.Object);

            var res = await getAllMediaService.GetAllMedia(request);
            Assert.NotNull(res);
        }
    }
}
