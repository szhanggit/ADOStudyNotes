using Moq;
using Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Test
{
    public class SecurityKeyServiceTest
    {
        private Mock<IDammAlgorithm> _dammAlgorithmMock;

        public SecurityKeyServiceTest()
        {
            _dammAlgorithmMock = new Mock<IDammAlgorithm>();
            _dammAlgorithmMock.Setup(x => x.CalculateCheckSum(It.IsAny<string>())).Returns(It.IsAny<int>());
        }

        [Fact]
        public void GenerateSecurityKey_16length_ShallReturnSecurityKey()
        {
            ISecurityKeyService _securityKeyService = new SecurityKeyService(_dammAlgorithmMock.Object);
            string _identity = _securityKeyService.GenerateSecurityKey(16);
            Assert.Equal(17, _identity.Length);
        }

        [Fact]
        public void GenerateSecurityKey_32length_ShallReturnSecurityKey()
        {
            ISecurityKeyService _securityKeyService = new SecurityKeyService(_dammAlgorithmMock.Object);
            string _identity = _securityKeyService.GenerateSecurityKey(32);
            Assert.Equal(33, _identity.Length);
        }
    }
}
