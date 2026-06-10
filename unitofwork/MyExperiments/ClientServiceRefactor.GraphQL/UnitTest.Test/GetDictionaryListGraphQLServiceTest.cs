using Domain.Models;
using Domain.Models.Response;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.BusinessLogic;
using Service.Utility;
using Service.Utility.GraphQLClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class GetDictionaryListGraphQLServiceTest
    {
        private Mock<IGraphQLGatewayClient> _graphQLGatewayClientMock;
        private Mock<IClientHelperService> _clientHelperServiceMock;
        private Mock<IObjectConvertingService> _objectConvertingServiceMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IGraphQLClient> _graphQLClientMock;
        private Mock<IGeneralGraphQLClient> _generalGraphQLClient;
        private IGraphQLClient _graphQLClient;
        private IGetDictionaryListGraphQLService _getDictionaryListGraphQLService;
        private ClientListItem _clientItem = null;

        public GetDictionaryListGraphQLServiceTest()
        {
            _clientItem = new ClientListItem
            {
                SalesEmail = "",
                SecurityAlgorithm = 1,
            };
            _generalGraphQLClient = new Mock<IGeneralGraphQLClient>();
            _generalGraphQLClient.Setup(p => p.GetGraphQLClient(It.IsAny<int>())).Returns(It.IsAny<IGraphQLClient>());
            _graphQLClientMock = new Mock<IGraphQLClient>();
            _graphQLClientMock.Setup(p => p.SendQueryAsync<GetClientByIdGraphQLResponse>(It.IsAny<GraphQLRequest>(), default)).ReturnsAsync(It.IsAny<GraphQLResponse<GetClientByIdGraphQLResponse>>());
            _configMock = new Mock<IConfiguration>();
            _graphQLGatewayClientMock = new Mock<IGraphQLGatewayClient>();
            _graphQLGatewayClientMock.Setup(p => p.GetGraphQLClient(It.IsAny<int>())).Returns(_graphQLClientMock.Object);
            _clientHelperServiceMock = new Mock<IClientHelperService>();
            _clientHelperServiceMock.Setup(p => p.GetSkipNum(It.IsAny<int?>(), It.IsAny<int?>())).Returns(It.IsAny<int>());
            _objectConvertingServiceMock = new Mock<IObjectConvertingService>();
            _objectConvertingServiceMock.Setup(p => p.ConvertClientModelToClientListItem(It.IsAny<ClientModel>())).Returns(_clientItem);
            _getDictionaryListGraphQLService = new GetDictionaryListGraphQLService(_generalGraphQLClient.Object);
        }

        [Fact]
        public async Task TestGetProvinceCityPairListAsync()
        {
            List<ProvinceCityPairModel> _result = await _getDictionaryListGraphQLService.GetProvinceCityPairListAsync(9, 6);
            Assert.Empty(_result);
        }
    }
}
