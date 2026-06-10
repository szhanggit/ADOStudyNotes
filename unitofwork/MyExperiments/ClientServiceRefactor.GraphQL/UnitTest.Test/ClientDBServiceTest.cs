using Domain.Entities;
using Domain.Models;
using Moq;
using Repository;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Test
{
    public class ClientDBServiceTest
    {
        private Mock<IClientUnitOfWork> _clientUnitOfWorkMock;
        private IClientDBService _clientDBService;
        private Mock<IDbConnection> _dbConnectionMock;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IAddressRepository> _addressRepositoryMock;

        public ClientDBServiceTest()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _clientUnitOfWorkMock = new Mock<IClientUnitOfWork>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _addressRepositoryMock = new Mock<IAddressRepository>();
            _clientUnitOfWorkMock.Setup(p => p.ClientRepository).Returns(_clientRepositoryMock.Object);
            _clientRepositoryMock.Setup(p => p.GetNewSequenceIdAsync()).ReturnsAsync(10);
            _addressRepositoryMock.Setup(p => p.CreateAddress(It.IsAny<Address>(), It.IsAny<IDbTransaction>()));
            _clientDBService = new ClientDBService(_clientUnitOfWorkMock.Object);
            _clientUnitOfWorkMock.Setup(p => p.AddressRepository).Returns(_addressRepositoryMock.Object);
            _clientRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Client>(), It.IsAny<IDbTransaction>())).ReturnsAsync(100);
        }

        [Fact]
        public async Task TestGenerateClientIdentityAsync()
        {            
            string _result = await _clientDBService.GenerateClientIdentityAsync(9, _dbConnectionMock.Object);
            Assert.Equal("900000000000000001010", _result);
        }

        [Fact]
        public async Task TestInsertClientAsync()
        {            
            Domain.Entities.Client client = new Domain.Entities.Client();
            Address address = new Address();
            int? _clientId = await _clientDBService.InsertClientAsync(client, address, _dbConnectionMock.Object);
            Assert.Equal(100, _clientId);
        }

        [Fact]
        public async Task TestUpdateClientAsync()
        {
            ClientModel origin = new ClientModel();
            Client client = new Client();
            Address address = new Address();
            bool _result = await _clientDBService.UpdateClientAsync(origin, client, address, _dbConnectionMock.Object);  
            Assert.True(_result);
        }

        [Fact]
        public async Task TestDeleteClientByIdAsync()
        {
            await _clientDBService.DeleteClientByIdAsync(9, _dbConnectionMock.Object);
        }

        [Fact]
        public async Task TestDeleteClientByIdWithAddressIdAsync()
        {
            Client _client = new Client { 
                Address_Id = 1001,
            };
            _clientRepositoryMock.Setup(p => p.GetAsync(It.IsAny<int>())).ReturnsAsync(_client);
            await _clientDBService.DeleteClientByIdAsync(9, _dbConnectionMock.Object);
        }
    }
}
