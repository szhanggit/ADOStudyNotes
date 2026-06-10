using Moq;
using Repository;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.MessageContract;
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
        public Mock<IObjectConvertingService> _objectConvertingService { get; set; }
        public Mock<IDapperOperation> _dapperOperation { get; set; }
        public Mock<ITX2ServiceBusSender> _tx2ServiceBusSender { get; set; }
        public Mock<IDbConnection> _idbConnection { get; set; }


        public ClientServicesFixture()
        {
            _coreServiceMock = new Mock<ICoreService>();
            _createClientService = new Mock<ICreateClientService>();
            _getClientListService = new Mock<IGetClientListService>();
            _updateClientService = new Mock<IUpdateClientService>();
            _commonClientService = new Mock<ICommonClientService>();
            _clientRepository = new Mock<IClientRepository>();
            _objectConvertingService = new Mock<IObjectConvertingService>();
            _dapperOperation = new Mock<IDapperOperation>();
            _tx2ServiceBusSender = new Mock<ITX2ServiceBusSender>();
            _idbConnection = new Mock<IDbConnection>();
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
        }
    }

    [CollectionDefinition("Client Services collection")]
    public class ClientServicesServicesCollection : ICollectionFixture<ClientServicesFixture>
    {

    }
}
