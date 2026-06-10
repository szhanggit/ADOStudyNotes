using Moq;
using Repository;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Proto.Client;
using Xunit;
using Xunit.Abstractions;

namespace Services.Tests
{
    [Collection("Client Services collection")]
    public class GetClientListServiceTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ICoreService> _coreServiceMock;
        private IGetClientListService _getClientListService;
        private GetClientListRequest _request;
        private IEnumerable<ClientListItem> _clientList;
        public GetClientListServiceTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _clientRepositoryMock = _servicesFixture._clientRepository;
            _coreServiceMock = _servicesFixture._coreServiceMock;
            _clientList = new List<ClientListItem> { };
        }

        private void init()
        {
            _clientRepositoryMock.Setup(p => p.GetClientAsync(It.IsAny<GetClientListRequest>(), It.IsAny<IDbConnection>())).ReturnsAsync(Tuple.Create(1, _clientList));
            _coreServiceMock.Setup(p => p.GetConfig(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new TXC.Common.Domain.TenantConfig
            {
                Value = "asdf"
            });
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = true
            });
            _getClientListService = new GetClientListService(_clientRepositoryMock.Object, _coreServiceMock.Object);
            _request = new GetClientListRequest { TenantId = 7, TenantName = "TW" };
        }

        [Fact]
        public async Task GetClientList_HappyPath()
        {
            init();
            var _result = await _getClientListService.GetClientList(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);

            GetClientListResponse _getClientListResponse = _result.Data.Unpack<GetClientListResponse>();
            Assert.NotNull(_getClientListResponse);
            Assert.Equal(1, _getClientListResponse.TotalCount);
            Assert.NotNull(_getClientListResponse.ClientDtos);
            Assert.Equal(0, _getClientListResponse.ClientDtos.Count());
        }

        [Fact]
        public async Task GetClientList_InValidTenantId_ShallReturnTenantBasicInfoIdHeaderRequired()
        {
            init();
            _request.TenantId = -1;
            var result = await _getClientListService.GetClientList(_request);

            Assert.Equal("TenantBasicInfoId header required", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetClientList_EmptyTenantName_ShallReturnTenantNameHeaderRequired()
        {
            init();
            _request.TenantName = String.Empty;
            var result = await _getClientListService.GetClientList(_request);

            Assert.Equal("TenantName header required", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetClientList_NotExistTenantId_ShallReturnErrorInTenantDB()
        {
            init();
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = false
            });
            var result = await _getClientListService.GetClientList(_request);

            Assert.Equal("Error in Tenant DB", result.Message);
            Assert.False(result.Success);
        }
    }
}
