using FluentValidation;
using Moq;
using Repository;
using Service.BusinessLogic;
using Service.Utility;
using System;
using System.Data;
using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class ClientServicesFixture : IDisposable
    {
        public Mock<ICoreService> _coreServiceMock { get; set; }
        public Mock<ICreateClientService> _createClientService { get; set; }
        public Mock<IGetClientListService> _getClientListService { get; set; }
        public Mock<IUpdateClientService> _updateClientService { get; set; }
        public Mock<ICommonClientService> _commonClientService { get; set; }
        public Mock<IClientRepository> _clientRepository { get; set; }
        public Mock<IAddressRepository> _addressRepository { get; set; }
        public Mock<IObjectConvertingService> _objectConvertingService { get; set; }
        public Mock<IDapperOperation> _dapperOperation { get; set; }
        public Mock<ITX2ServiceBusSender> _tx2ServiceBusSender { get; set; }
        public Mock<IDbConnection> _idbConnection { get; set; }
        public Mock<IGetDictionaryListGraphQLService> _getDictionaryListGraphQLServiceMock { get; set; }
        public Mock<IClientFetchingGraphQLService> _clientFetchingGraphQLServiceMock { get; set; }
        public Mock<IClientAddressFetchingGraphQLService> _clientAddressFetchingGraphQLServiceMock { get; set; }
        public Mock<IClientDBService> _clientDBServiceMock { get; set; }
        public Mock<ISecurityKeyService> _securityKeyServiceMock { get; set; }
        public Mock<IValidator<CreateClientRequest>> _createClientValidatorMock { get; set; }
        public Mock<IValidator<UpdateClientRequest>> _updateClientValidatorMock { get; set; }
        public Mock<IValidator<GetClientListRequest>> _getClientValidatorMock { get; set; }

        public ClientServicesFixture()
        {
            _coreServiceMock = new Mock<ICoreService>();
            _createClientService = new Mock<ICreateClientService>();
            _getClientListService = new Mock<IGetClientListService>();
            _updateClientService = new Mock<IUpdateClientService>();
            _commonClientService = new Mock<ICommonClientService>();
            _clientRepository = new Mock<IClientRepository>();
            _addressRepository = new Mock<IAddressRepository>();
            _objectConvertingService = new Mock<IObjectConvertingService>();
            _dapperOperation = new Mock<IDapperOperation>();
            _tx2ServiceBusSender = new Mock<ITX2ServiceBusSender>();
            _idbConnection = new Mock<IDbConnection>();
            _getDictionaryListGraphQLServiceMock = new Mock<IGetDictionaryListGraphQLService>();
            _clientFetchingGraphQLServiceMock = new Mock<IClientFetchingGraphQLService>();
            _clientAddressFetchingGraphQLServiceMock = new Mock<IClientAddressFetchingGraphQLService>();
            _clientDBServiceMock = new Mock<IClientDBService>();
            _securityKeyServiceMock = new Mock<ISecurityKeyService>();
            _createClientValidatorMock = new Mock<IValidator<CreateClientRequest>>();
            _updateClientValidatorMock = new Mock<IValidator<UpdateClientRequest>>();
            _getClientValidatorMock = new Mock<IValidator<GetClientListRequest>>();
        }

        public void Dispose()
        {
            _coreServiceMock = null;
            _createClientService = null;
            _getClientListService = null;
            _updateClientService = null;
            _commonClientService = null;
            _clientRepository = null;
            _objectConvertingService = null;
            _dapperOperation = null;
            _tx2ServiceBusSender = null;
            _idbConnection = null;
            _getDictionaryListGraphQLServiceMock = null;
            _clientFetchingGraphQLServiceMock = null;
            _clientAddressFetchingGraphQLServiceMock = null;
            _clientDBServiceMock = null;
            _securityKeyServiceMock = null;
            _createClientValidatorMock = null;
            _updateClientValidatorMock = null;
            _getClientValidatorMock = null;
            _addressRepository = null;
        }
    }

    [CollectionDefinition("Client Services collection")]
    public class ClientServicesServicesCollection : ICollectionFixture<ClientServicesFixture>
    {

    }
}
