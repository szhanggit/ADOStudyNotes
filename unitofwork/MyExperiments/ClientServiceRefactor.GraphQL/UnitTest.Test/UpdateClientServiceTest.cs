using Domain.Entities;
using Domain.Models;
using FluentValidation;
using Moq;
using Repository;
using Service.BusinessLogic;
using Service.Utility;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class UpdateClientServiceTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ICoreService> _coreServiceMock;
        private readonly Mock<ICommonClientService> _commonClientServiceMock;
        private readonly Mock<IObjectConvertingService> _objectConvertingServiceMock;
        private readonly Mock<IGetDictionaryListGraphQLService> _getDictionaryListGraphQLServiceMock;
        private readonly Mock<IClientFetchingGraphQLService> _clientFetchingGraphQLServiceMock;
        private readonly Mock<IClientAddressFetchingGraphQLService> _clientAddressFetchingGraphQLServiceMock;
        private readonly Mock<IClientDBService> _clientDBServiceMock;
        private readonly Mock<ISecurityKeyService> _securityKeyServiceMock;
        private readonly Mock<IValidator<UpdateClientRequest>> _validatorMock;
        private IUpdateClientService _updateClientService;
        private UpdateClientRequest _request;
        private IEnumerable<ClientListItem> _clientList;

        public UpdateClientServiceTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _clientRepositoryMock = _servicesFixture._clientRepository;
            _coreServiceMock = _servicesFixture._coreServiceMock;
            _commonClientServiceMock = _servicesFixture._commonClientService;
            _objectConvertingServiceMock = _servicesFixture._objectConvertingService;
            _clientDBServiceMock = _servicesFixture._clientDBServiceMock;
            _getDictionaryListGraphQLServiceMock = _servicesFixture._getDictionaryListGraphQLServiceMock;
            _clientFetchingGraphQLServiceMock = _servicesFixture._clientFetchingGraphQLServiceMock;
            _securityKeyServiceMock = _servicesFixture._securityKeyServiceMock;
            _validatorMock = _servicesFixture._updateClientValidatorMock;
            _clientList = new List<ClientListItem> { };
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

            _commonClientServiceMock.Setup(p => p.SendUpdateMessageAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClientMessageV1>())).ReturnsAsync(true);
            _commonClientServiceMock.Setup(p => p.CreateClientWalletAsync(It.IsAny<CreateClientWalletRequest>())).ReturnsAsync(new TXC.Proto.Credit.ProtoBaseResponse { Success = true });

            _getDictionaryListGraphQLServiceMock.Setup(p => p.GetProvinceCityPairListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<ProvinceCityPairModel> {
                new ProvinceCityPairModel { city = 100, province = 11 },
                new ProvinceCityPairModel { city = 200, province = 11 },
            });

            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse());
            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsBySearchKeyAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse());
            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsByIdAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ProtoBaseResponse());
            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsByNameAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new TXC.Proto.Client.GetClientListResponse());
            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsByCodeAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(new ClientModel());

            _securityKeyServiceMock.Setup(p => p.GenerateSecurityKey(It.IsAny<int>())).Returns("SecurityKey");
            _request = new UpdateClientRequest { TenantId = 7, TenantName = "TW", ClientId = 1, SecurityAlgorithm = 1, SecurityKey = "asdfasdfsdfkdivf", CountryId = 6 };
            _validatorMock.Setup(p => p.ValidateAsync(_request, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult { });

            _updateClientService = new UpdateClientService(
                _clientDBServiceMock.Object, 
                _coreServiceMock.Object, 
                _commonClientServiceMock.Object, 
                _objectConvertingServiceMock.Object,
                _getDictionaryListGraphQLServiceMock.Object,
                _clientFetchingGraphQLServiceMock.Object,
                _validatorMock.Object
                );            
        }

        [Fact]
        public async Task UpdateClient_HappyPath()
        {
            init();
            var _result = await _updateClientService.UpdateClient(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);

            var _clientId = _result.Data;
            Assert.Equal(1, _clientId);
        }

        [Fact]
        public async Task UpdateClient_InValidTenantId_ShallReturnTenantBasicInfoIdHeaderRequired()
        {
            init();
            _request.TenantId = -1;
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("TenantBasicInfoId header required", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_EmptyTenantName_ShallReturnTenantNameHeaderRequired()
        {
            init();
            _request.TenantName = String.Empty;
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("TenantName header required", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_InvalidClientId_ShallReturnProductIdInvalidRequest()
        {
            init();
            _request.ClientId = -1;
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("Invalid Request", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_NotExistTenantId_ShallReturnErrorInTenantDB()
        {
            init();
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = false
            });
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("Error in Tenant DB", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_NotExistClientId_ShallReturnClientDoesNotExist()
        {
            ClientModel _clientModel = null;
            init();
            _clientFetchingGraphQLServiceMock.Setup(p => p.GetClientsByCodeAsync(It.IsAny<GetClientListModel>())).ReturnsAsync(_clientModel);
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("The client does not exist.", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_NotExistAddress_ShallReturnInvalidCountryId()
        {
            init();
            _request.CountryId = 6;
            _getDictionaryListGraphQLServiceMock.Setup(p => p.GetProvinceCityPairListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<ProvinceCityPairModel> { });
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("Invalid country id.", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_FailToUpdate_ShallReturnFailedToUpdateNewClient()
        {
            init();
            _clientDBServiceMock.Setup(p => p.UpdateClientAsync(It.IsAny<ClientModel>(), It.IsAny<Domain.Entities.Client>(), It.IsAny<Address>(), It.IsAny<IDbConnection>())).ReturnsAsync(false);
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("Failed to update new client", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateClient_FailToSendToServiceBus_ShallReturnFailToBeSentToServiceBus()
        {
            init();
            _commonClientServiceMock.Setup(p => p.SendUpdateMessageAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClientMessageV1>())).ReturnsAsync(false);
            var result = await _updateClientService.UpdateClient(_request);

            Assert.Equal("Fail to be sent to service bus", result.Message);
            Assert.False(result.Success);
        }
    }
}
