using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TXC.Common.MessageContract;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Credit;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public class SendUpdateMessageSteps : CommonHelper
    {
        public SendUpdateMessageSteps()
        {

        }

        [When(@"I send update message to service bus")]
        public async Task ExecuteAsync()
        {
            ClientMessageV1 message = new ClientMessageV1 { };
            ITX2ServiceBusSender _txcServiceBusSender = GetServiceBusSender();
            CreditRpc.CreditRpcClient client = GetCreditRpcClient();
            CommonClientService _commonClientService = new CommonClientService(_txcServiceBusSender, client);
            var s = await _commonClientService.SendUpdateMessageAsync(9, "", message);
        }
    }
}
