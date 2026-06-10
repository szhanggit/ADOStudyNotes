using Moq;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.MessageContract;
using TXC.Proto.Credit;
using Xunit;

namespace UnitTest.Test
{
    public class CommonClientServiceTest
    {
        private ICommonClientService _commonClientService;
        private Mock<ITX2ServiceBusSender> _tx2ServiceBusSender;
        private Mock<CreditRpc.CreditRpcClient> _credit;

        public CommonClientServiceTest()
        {
            _tx2ServiceBusSender = new Mock<ITX2ServiceBusSender>();
            _tx2ServiceBusSender.Setup(p => p.SendMessageAsync(
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<MessageBody>(), 
                It.IsAny<ESBMessageType>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<int>())).ReturnsAsync(true);
            _credit = new Mock<CreditRpc.CreditRpcClient>();            
            _commonClientService = new CommonClientService(_tx2ServiceBusSender.Object, _credit.Object);
        }

        [Fact]
        public async Task TestSendCreateMessageAsync()
        {
            bool _result = await _commonClientService.SendCreateMessageAsync(9, "", new TXC.Common.MessageContract.Client.ClientMessageV1 { });
            Assert.True(_result);
        }

        [Fact]
        public async Task TestSendUpdateMessageAsync()
        {
            bool _result = await _commonClientService.SendUpdateMessageAsync(9, "", new TXC.Common.MessageContract.Client.ClientMessageV1 { });
            Assert.True(_result);
        }

        [Fact]
        public async Task TestSendCreateBXPMessageAsync()
        {
            bool _result = await _commonClientService.SendCreateBXPMessageAsync(9, "", new TXC.Common.MessageContract.Client.ClientMessageV1 { });
            Assert.True(_result);
        }
    }
}
