using Domain.Models;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Moq;
using Repository;
using Services.Core;
using Services.Utility;
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

namespace Services.Tests
{
    [Collection("Client Services collection")]
    public class CreateClientServiceTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<ICoreService> _coreServiceMock;
        private readonly Mock<ICommonClientService> _commonClientServiceMock;
        private readonly Mock<IObjectConvertingService> _objectConvertingServiceMock;
        private readonly Mock<IValidator<CreateClientRequest>> _validatorMock;
        private readonly Mock<ISecurityKeyService> _securityKeyServiceMock;
        private ICreateClientService _createClientService;
        private CreateClientRequest _request;
        private IEnumerable<ClientListItem> _clientList;

        public CreateClientServiceTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _clientRepositoryMock = _servicesFixture._clientRepository;
            _coreServiceMock = _servicesFixture._coreServiceMock;
            _commonClientServiceMock = _servicesFixture._commonClientService;
            _objectConvertingServiceMock = _servicesFixture._objectConvertingService;
            _securityKeyServiceMock = _servicesFixture._securityKeyService;
            _clientList = new List<ClientListItem> { };
            _validatorMock = new Mock<IValidator<CreateClientRequest>>();
        }

        private void init()
        {
            _clientRepositoryMock.Setup(p => p.InsertClientAsync(It.IsAny<CreateClientRequest>(), It.IsAny<string>(), It.IsAny<IDbConnection>())).ReturnsAsync(1);
            _clientRepositoryMock.Setup(p => p.GenerateClientIdentityAsync(It.IsAny<GenerateClientIdentityCodeModel>(), It.IsAny<IDbConnection>())).ReturnsAsync("ClientIdentity");
            _clientRepositoryMock.Setup(p => p.CheckIfValidAddress(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<IDbConnection>())).ReturnsAsync(Tuple.Create(true, "asdf"));
            _clientRepositoryMock.Setup(p => p.DeleteClientByIdAsync(It.IsAny<int>(), It.IsAny<IDbConnection>()));
            _coreServiceMock.Setup(p => p.GetConfig(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new TXC.Common.Domain.TenantConfig
            {
                Value = "asdf"
            });
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = true
            });
            _commonClientServiceMock.Setup(p => p.SendCreateMessageAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClientMessageV1>())).ReturnsAsync(true);
            _commonClientServiceMock.Setup(p => p.CreateClientWalletAsync(It.IsAny<CreateClientWalletRequest>())).ReturnsAsync(new TXC.Proto.Credit.ProtoBaseResponse { Success = true });
            _validatorMock.Setup(p => p.ValidateAsync(It.IsAny<CreateClientRequest>(), default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _createClientService = new CreateClientService(_clientRepositoryMock.Object, _coreServiceMock.Object, _commonClientServiceMock.Object, _objectConvertingServiceMock.Object, _securityKeyServiceMock.Object, _validatorMock.Object);
            _request = new CreateClientRequest { TenantId = 7, TenantName = "TW" };
            _clientRepositoryMock.Setup(p => p.CheckClientByNameAsync(It.IsAny<GetClientListRequest>(), It.IsAny<IDbConnection>())).ReturnsAsync(false);
        }

        [Fact]
        public async Task CreateClient_HappyPath()
        {
            init();
            var _result = await _createClientService.CreateClient(_request);
            Assert.NotNull(_result);
            Assert.True(_result.Success);
            Assert.Equal("Success", _result.Message);
        }

        [Fact]
        public async Task CreateClient_InValidTenantId_ShallReturnTenantBasicInfoIdHeaderRequired()
        {
            init();
            _request.TenantId = -1;
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("TenantBasicInfoId header required", result.Message);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateClient_EmptyTenantName_ShallReturnTenantNameHeaderRequired()
        {
            init();
            _request.TenantName = String.Empty;
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("TenantName header required", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_NotExistTenantId_ShallReturnErrorInTenantDB()
        {
            init();
            _coreServiceMock.Setup(p => p.GetDBConnection(It.IsAny<int>())).ReturnsAsync(new Response<IDbConnection>
            {
                Success = false
            });
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Error in Tenant DB", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_EmptyIdentityCode_ShallReturnFailedToGenerateClientIdentityCode()
        {
            init();
            _clientRepositoryMock.Setup(p => p.GenerateClientIdentityAsync(It.IsAny<GenerateClientIdentityCodeModel>(), It.IsAny<IDbConnection>()))
                .ReturnsAsync(string.Empty);
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Failed to generate client identity code", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_NotExistAddress_ShallReturnInvalidCountryId()
        {
            init();
            _request.CountryId = 6;
            _clientRepositoryMock.Setup(p => p.CheckIfValidAddress(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<IDbConnection>())).ReturnsAsync(Tuple.Create(false, "Invalid country id."));
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Invalid country id.", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_NotExistAddress_ShallReturnFailedToCreateNewClient()
        {
            init();
            _clientRepositoryMock.Setup(p => p.InsertClientAsync(It.IsAny<CreateClientRequest>(), It.IsAny<string>(), It.IsAny<IDbConnection>()));
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Failed to create new client", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_NotExistAddress_ShallReturnFailedToCreateNewWallet()
        {
            init();
            _commonClientServiceMock.Setup(p => p.CreateClientWalletAsync(It.IsAny<CreateClientWalletRequest>()))
                .ReturnsAsync(new TXC.Proto.Credit.ProtoBaseResponse { Success = false });
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Failed to create new wallet", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_FailToSendToServiceBus_ShallReturnFailToBeSentToServiceBus()
        {
            init();
            _commonClientServiceMock.Setup(p => p.SendCreateMessageAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClientMessageV1>())).ReturnsAsync(false);
            var result = await _createClientService.CreateClient(_request);

            Assert.Equal("Fail to be sent to service bus", result.Message);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateClient_Validator()
        {
            init();
            var validationResult = new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>() { new FluentValidation.Results.ValidationFailure("SecurityAlgorithm", "Blank SecurityAlgorithm.") }
            };
            _validatorMock.Setup(p => p.ValidateAsync(It.IsAny<CreateClientRequest>(), default)).ReturnsAsync(validationResult);
            var _result = await _createClientService.CreateClient(_request);
            Assert.Equal("Blank SecurityAlgorithm.", _result.Message);
        }
    }
}
