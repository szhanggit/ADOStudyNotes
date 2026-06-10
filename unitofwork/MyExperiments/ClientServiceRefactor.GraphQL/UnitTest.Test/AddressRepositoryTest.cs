using Domain.Entities;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Test
{
    [Collection("Client Services collection")]
    public class AddressRepositoryTest
    {
        private ClientServicesFixture _servicesFixture;
        private ITestOutputHelper _output;
        private Mock<IAddressRepository> _addressRepositoryMock;
        private Mock<Context> _contextMock;
        private Mock<Address> _addressMock;
        private Mock<IDbTransaction> _transactionMock;

        public AddressRepositoryTest(ClientServicesFixture servicesFixture, ITestOutputHelper output)
        {
            _servicesFixture = servicesFixture;
            _output = output;
            _addressRepositoryMock = _servicesFixture._addressRepository;
            _contextMock = new Mock<Context>();
            _addressMock = new Mock<Address>();
            _transactionMock = new Mock<IDbTransaction>();
        }

        [Fact]
        public async Task CreateAddress_HappyPath_ShallReturnSuccess()
        {
            await _addressRepositoryMock.Object.CreateAddress(_addressMock.Object, _transactionMock.Object);
        }
    }
}
