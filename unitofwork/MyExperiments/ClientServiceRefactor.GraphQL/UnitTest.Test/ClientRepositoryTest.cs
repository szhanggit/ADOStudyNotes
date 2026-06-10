using Dapper;
using Domain.Models;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Proto.Client;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Test
{
    [Collection("Client Services collection")]
    public class ClientRepositoryTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private Mock<IDapperOperation> _dopperOperationMock;
        private Mock<ITX2ServiceBusSender> _tx2ServiceBusSenderMock;
        private IClientRepository _clientRepository;
        private GenerateClientIdentityCodeModel generateClientIdentityCode;
        private Mock<IDbConnection> _idbConnectionMock;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<Context> _contextMock;

        public ClientRepositoryTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _dopperOperationMock = _servicesFixture._dapperOperation;
            _tx2ServiceBusSenderMock = _servicesFixture._tx2ServiceBusSender;
            _idbConnectionMock = _servicesFixture._idbConnection;
            _clientRepositoryMock = _servicesFixture._clientRepository;
            _contextMock = new Mock<Context>();
        }

        private void init()
        {
            generateClientIdentityCode = new GenerateClientIdentityCodeModel
            {
                SequenceName = "client.seq_client_identity_code",
                IsFixReturnLength = true,
                ReturnLength = 20,
                PaddingCharacter = '0'
            };
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommandWithReturn<string>, string>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>())).ReturnsAsync("ClientIdentity");
            _clientRepositoryMock.Setup(p => p.GetNewSequenceIdAsync()).ReturnsAsync(1);
            _clientRepository = new ClientRepository(_contextMock.Object);
        }

        [Fact]
        public async Task GetNewSequenceIdAsync_HappyPath_ShallReturnSuccess()
        {
            int _newId = await _clientRepositoryMock.Object.GetNewSequenceIdAsync();
            Assert.Equal(0, _newId);
        }
    }
}