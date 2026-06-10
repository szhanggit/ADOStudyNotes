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

        public ClientRepositoryTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _dopperOperationMock = _servicesFixture._dapperOperation;
            _tx2ServiceBusSenderMock = _servicesFixture._tx2ServiceBusSender;
            _idbConnectionMock = _servicesFixture._idbConnection;
            _clientRepository = new ClientRepository(_dopperOperationMock.Object, _tx2ServiceBusSenderMock.Object);
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
        }

        [Fact]
        public async Task GenerateClientIdentityAsync_HappyPath_ShallReturnSuccess()
        {
            init();
            string token = await _clientRepository.GenerateClientIdentityAsync(generateClientIdentityCode, _idbConnectionMock.Object);
            Assert.Equal("ClientIdentity", token);
        }

        [Fact]
        public async Task InsertClientAsync_HappyPath_ShallReturnSuccess()
        {
            string IdentityCode = "ClientIdentity";
            CreateClientRequest createClientRequest = new CreateClientRequest { };
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommandWithReturn<string>, string>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()));
            int? ClientId = await _clientRepository.InsertClientAsync(createClientRequest, IdentityCode, _idbConnectionMock.Object);
        }


        [Fact]
        public async Task UpdateClientAsync_HappyPath_ShallReturnSuccess()
        {
            UpdateClientRequest updateClientRequest = new UpdateClientRequest { };
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommand, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>())).ReturnsAsync(1);
            int? ClientId = await _clientRepository.UpdateClientAsync(updateClientRequest, _idbConnectionMock.Object);
            Assert.True(ClientId.HasValue);
            Assert.Equal(1, ClientId.Value);
        }


        [Fact]
        public async Task GetClientAsync_HappyPath_ShallReturnSuccess()
        {
            GetClientListRequest _getClientListRequest = new GetClientListRequest { };
            _dopperOperationMock.Setup(p => p.ProcessSql<SelectMany<ClientListItem>, IEnumerable<ClientListItem>>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>())).ReturnsAsync(new List<ClientListItem>());
            var result = await _clientRepository.GetClientAsync(_getClientListRequest, _idbConnectionMock.Object);
        }

        [Fact]
        public async Task CheckIfValidAddress_HappyPath_ShallReturnSuccess()
        {
            GetClientListRequest _getClientListRequest = new GetClientListRequest { };
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommandWithReturn<int>, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()));
            var result = await _clientRepository.CheckIfValidAddress(1, 1, 1, _idbConnectionMock.Object);
        }

        [Fact]
        public async Task CheckClientIdAsync_HappyPath_ShallReturnSuccess()
        {
            GetClientListRequest _getClientListRequest = new GetClientListRequest { };
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommandWithReturn<int>, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()));
            var result = await _clientRepository.CheckClientIdAsync(1, _idbConnectionMock.Object);
        }

        [Fact]
        public async Task DeleteClientByIdAsync_HappyPath_ShallReturnSuccess()
        {
            int ClientId = 1;
            _dopperOperationMock.Setup(p => p.ProcessSql<ExecuteCommandWithReturn<int>, int>(It.IsAny<IDbConnection>(), It.IsAny<CommandDefinition>()));
            await _clientRepository.DeleteClientByIdAsync(ClientId, _idbConnectionMock.Object);
        }
    }
}
