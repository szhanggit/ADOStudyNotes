using Domain.Entities;
using Domain.Models;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Moq;
using Repository;
using Service.BusinessLogic;
using Service.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Domain;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using TXC.Proto.Credit;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Test
{
    [Collection("Client Services collection")]
    public class GetClientListServiceTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ICoreService> _coreServiceMock;
        private readonly Mock<ICommonClientService> _commonClientServiceMock;
        private readonly Mock<IObjectConvertingService> _objectConvertingServiceMock;
        private readonly Mock<IGetDictionaryListGraphQLService> _getDictionaryListGraphQLServiceMock;
        private readonly Mock<IClientAddressFetchingGraphQLService> _clientAddressFetchingGraphQLServiceMock;
        private readonly Mock<IClientDBService> _clientDBServiceMock;
        private readonly Mock<ISecurityKeyService> _securityKeyServiceMock;
        private readonly Mock<IValidator<GetClientListRequest>> _validatorMock;
        private IGetClientListService _getClientListService;
        private GetClientListRequest _request;
        private IEnumerable<ClientListItem> _clientList;

        public GetClientListServiceTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _clientRepositoryMock = _servicesFixture._clientRepository;
            _coreServiceMock = _servicesFixture._coreServiceMock;
            _commonClientServiceMock = _servicesFixture._commonClientService;
            _objectConvertingServiceMock = _servicesFixture._objectConvertingService;
            _clientDBServiceMock = _servicesFixture._clientDBServiceMock;
            _getDictionaryListGraphQLServiceMock = _servicesFixture._getDictionaryListGraphQLServiceMock;
            _securityKeyServiceMock = _servicesFixture._securityKeyServiceMock;
            _validatorMock = _servicesFixture._getClientValidatorMock;
            _clientAddressFetchingGraphQLServiceMock = _servicesFixture._clientAddressFetchingGraphQLServiceMock;
            _clientList = new List<ClientListItem> { 
                new ClientListItem { ClientId = 1, ClientName = "C1" },
                new ClientListItem { ClientId = 2, ClientName = "C2" },
                new ClientListItem { ClientId = 3, ClientName = "C3" },
                new ClientListItem { ClientId = 4, ClientName = "C4" },
            };
        }

        private void init()
        {
            _clientDBServiceMock.Setup(p => p.GenerateClientIdentityAsync(It.IsAny<int>(), It.IsAny<IDbConnection>())).ReturnsAsync("ClientIdentity");
            _clientDBServiceMock.Setup(p => p.InsertClientAsync(It.IsAny<Domain.Entities.Client>(), It.IsAny<Address>(), It.IsAny<IDbConnection>())).ReturnsAsync(1);
            _clientDBServiceMock.Setup(p => p.UpdateClientAsync(It.IsAny<ClientModel>(), It.IsAny<Domain.Entities.Client>(), It.IsAny<Address>(), It.IsAny<IDbConnection>())).ReturnsAsync(true);
            _clientDBServiceMock.Setup(p => p.DeleteClientByIdAsync(It.IsAny<int>(), It.IsAny<IDbConnection>()));

            _coreServiceMock.Setup(p => p.GetConfig(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new TXC.Common.Domain.TenantConfig
            {
                Value = "asdf"
            });
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = true
            });

            _getDictionaryListGraphQLServiceMock.Setup(p => p.GetProvinceCityPairListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<ProvinceCityPairModel> {
                new ProvinceCityPairModel { city = 100, province = 11 },
                new ProvinceCityPairModel { city = 200, province = 11 },
            });

            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            response.ClientDtos.AddRange(_clientList);
            response.TotalCount = _clientList.Count();

            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });
            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsBySearchKeyAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });
            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsByIdAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });
            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsByCodeAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });

            _securityKeyServiceMock.Setup(p => p.GenerateSecurityKey(It.IsAny<int>())).Returns("SecurityKey");
            _request = new GetClientListRequest { TenantId = 7, TenantName = "TW" };
            _validatorMock.Setup(p => p.ValidateAsync(_request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult { });

            _getClientListService = new GetClientListService(
                _clientDBServiceMock.Object,
                _coreServiceMock.Object,
                _clientAddressFetchingGraphQLServiceMock.Object,
                _validatorMock.Object);
        }

        [Fact]
        public async Task GetClientList_NoClientIdNoSearchKey_HappyPath()
        {
            init();
            var _result = await _getClientListService.GetClientList(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);

            GetClientListResponse _getClientListResponse = _result.Data.Unpack<GetClientListResponse>();
            Assert.NotNull(_getClientListResponse);
            Assert.Equal(4, _getClientListResponse.TotalCount);
            Assert.NotNull(_getClientListResponse.ClientDtos);
            Assert.Equal(4, _getClientListResponse.ClientDtos.Count());
        }

        [Fact]
        public async Task GetClientList_WithClientId_HappyPath()
        {
            init();
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            response.ClientDtos.Clear();
            response.ClientDtos.Add(new ClientListItem { ClientId = 5, ClientName = "C5" });
            response.TotalCount = 1;
            _request.ClientId = 5;
            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsByIdAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });
            var _result = await _getClientListService.GetClientList(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);

            GetClientListResponse _getClientListResponse = _result.Data.Unpack<GetClientListResponse>();
            Assert.NotNull(_getClientListResponse);
            Assert.Equal(1, _getClientListResponse.TotalCount);
            Assert.NotNull(_getClientListResponse.ClientDtos);
            Assert.Equal(1, _getClientListResponse.ClientDtos.Count());
        }

        [Fact]
        public async Task GetClientList_WithSearchKey_HappyPath()
        {
            init();
            TXC.Proto.Client.GetClientListResponse response = new TXC.Proto.Client.GetClientListResponse();
            response.ClientDtos.Clear();
            response.ClientDtos.Add(new ClientListItem { ClientId = 5, ClientName = "C5" });
            response.ClientDtos.Add(new ClientListItem { ClientId = 6, ClientName = "C6" });
            response.TotalCount = 2;
            _request.SearchKeyword = "SearchKey";
            _clientAddressFetchingGraphQLServiceMock.Setup(p => p.GetClientsBySearchKeyAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse { Success = true, Message = "Success", Data = Any.Pack(response) });
            var _result = await _getClientListService.GetClientList(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);

            GetClientListResponse _getClientListResponse = _result.Data.Unpack<GetClientListResponse>();
            Assert.NotNull(_getClientListResponse);
            Assert.Equal(2, _getClientListResponse.TotalCount);
            Assert.NotNull(_getClientListResponse.ClientDtos);
            Assert.Equal(2, _getClientListResponse.ClientDtos.Count());
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
    }
}
