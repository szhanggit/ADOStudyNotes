using Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.BusinessLogic;
using Service.Utility;
using Service.Utility.GraphQLClient;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Test
{
    public class ClientFetchingGraphQLServiceTest
    {
        private IClientGraphQLClient _clientGraphQLClient = null;
        private IClientHelperService _clientHelperService = null;
        private IClientFetchingGraphQLService _clientFetchingGraphQLService = null;
        private Mock<IObjectConvertingService> _objectConvertingServiceMock = null;

        public ClientFetchingGraphQLServiceTest()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            _clientGraphQLClient = new ClientGraphQLClient(config);
            _clientHelperService = new ClientHelperService();
            _objectConvertingServiceMock = new Mock<IObjectConvertingService>();
            _clientFetchingGraphQLService = new ClientFetchingGraphQLService(_clientGraphQLClient, _clientHelperService, _objectConvertingServiceMock.Object);
        }

        [Fact]
        public async Task TestGetClientsAsync()
        {
            GetClientListModel request = new GetClientListModel { 
                RowCount = 10,
                PageNumber = 1,
                TenantId = 9,
            };
            await _clientFetchingGraphQLService.GetClientsAsync(request);
        }

        [Fact]
        public async Task TestGetClientsBySearchKeyAsync()
        {
            GetClientListModel request = new GetClientListModel
            {
                RowCount = 10,
                PageNumber = 1,
                TenantId = 9,
                SearchKeyWord = "asdf"
            };
            await _clientFetchingGraphQLService.GetClientsBySearchKeyAsync(request);
        }

        [Fact]
        public async Task TestGetClientsByIdAsync()
        {
            GetClientListModel request = new GetClientListModel
            {
                RowCount = 10,
                PageNumber = 1,
                TenantId = 9,
                ClientId = 1,
            };
            await _clientFetchingGraphQLService.GetClientsByIdAsync(request);
        }

        [Fact]
        public async Task TestGetClientsByNameAsync()
        {
            GetClientListModel request = new GetClientListModel
            {
                RowCount = 10,
                PageNumber = 1,
                TenantId = 9,
                SearchKeyWord = "ClientName"
            };
            await _clientFetchingGraphQLService.GetClientsByNameAsync(request);
        }

        [Fact]
        public async Task TestGetClientsByCodeAsync()
        {
            GetClientListModel request = new GetClientListModel
            {
                RowCount = 10,
                PageNumber = 1,
                TenantId = 9,
                SearchKeyWord = "ClientCode"
            };
            await _clientFetchingGraphQLService.GetClientsByCodeAsync(request);
        }
    }
}
