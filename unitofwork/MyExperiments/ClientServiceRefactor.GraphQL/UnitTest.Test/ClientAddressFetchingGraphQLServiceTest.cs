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
    public class ClientAddressFetchingGraphQLServiceTest
    {
        private Mock<IGraphQLGatewayClient> _graphQLGatewayClientMock;
        private Mock<IClientHelperService> _clientHelperServiceMock;
        private Mock<IObjectConvertingService> _objectConvertingServiceMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IGraphQLClient> _graphQLClientMock;
        private IGraphQLClient _graphQLClient;
        private IClientAddressFetchingGraphQLService _clientAddressFetchingGraphQLService;
        private ClientListItem _clientItem = null;

        public ClientAddressFetchingGraphQLServiceTest()
        {
            _clientItem = new ClientListItem { 
                SalesEmail = "",
                SecurityAlgorithm = 1,
            };
            _graphQLClientMock = new Mock<IGraphQLClient>();
            _graphQLClientMock.Setup(p => p.SendQueryAsync<GetClientByIdGraphQLResponse>(It.IsAny<GraphQLRequest>(), default)).ReturnsAsync(It.IsAny<GraphQLResponse<GetClientByIdGraphQLResponse>>());
            _configMock = new Mock<IConfiguration>();
            _graphQLGatewayClientMock = new Mock<IGraphQLGatewayClient>();
            _graphQLGatewayClientMock.Setup(p => p.GetGraphQLClient(It.IsAny<int>())).Returns(_graphQLClientMock.Object);
            _clientHelperServiceMock = new Mock<IClientHelperService>();
            _clientHelperServiceMock.Setup(p => p.GetSkipNum(It.IsAny<int?>(), It.IsAny<int?>())).Returns(It.IsAny<int>());
            _objectConvertingServiceMock = new Mock<IObjectConvertingService>();
            _objectConvertingServiceMock.Setup(p => p.ConvertClientModelToClientListItem(It.IsAny<ClientModel>())).Returns(_clientItem);
            _clientAddressFetchingGraphQLService = new ClientAddressFetchingGraphQLService(_graphQLGatewayClientMock.Object, _clientHelperServiceMock.Object, _objectConvertingServiceMock.Object);
        }

        [Fact]
        public async Task TestGetClientsAsync()
        {
            GetClientListModel getClientListModel = new GetClientListModel { 
                PageNumber = 1,
                RowCount = 10,
                TenantId = 9
            };
            ProtoBaseResponse result = await _clientAddressFetchingGraphQLService.GetClientsAsync(getClientListModel);
            Assert.True(result.Success);
            Assert.Equal("success", result.Message);
        }

        [Fact]
        public async Task TestGetClientsBySearchKeyAsync()
        {
            GetClientListModel getClientListModel = new GetClientListModel
            {
                PageNumber = 1,
                RowCount = 10,
                TenantId = 9,
                SearchKeyWord = "key"
            };
            ProtoBaseResponse result = await _clientAddressFetchingGraphQLService.GetClientsBySearchKeyAsync(getClientListModel);
            Assert.True(result.Success);
            Assert.Equal("success", result.Message);
        }

        [Fact]
        public async Task TestGetClientsByIdAsync()
        {
            GetClientListModel getClientListModel = new GetClientListModel
            {
                PageNumber = 1,
                RowCount = 10,
                TenantId = 9,
                SearchKeyWord = "ClientName"
            };
            ProtoBaseResponse result = await _clientAddressFetchingGraphQLService.GetClientsByIdAsync(getClientListModel);
            Assert.True(result.Success);
            Assert.Equal("success", result.Message);
        }

        [Fact]
        public async Task TestGetClientsByCodeAsync()
        {
            GetClientListModel getClientListModel = new GetClientListModel
            {
                PageNumber = 1,
                RowCount = 10,
                TenantId = 9,
                SearchKeyWord = "ClientCode"
            };
            ProtoBaseResponse result = await _clientAddressFetchingGraphQLService.GetClientsByCodeAsync(getClientListModel);
            Assert.True(result.Success);
            Assert.Equal("success", result.Message);
        }
    }
}
