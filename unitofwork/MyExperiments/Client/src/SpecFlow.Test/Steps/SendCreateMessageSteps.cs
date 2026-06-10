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
    public class SendCreateMessageSteps : CommonHelper
    {
        public SendCreateMessageSteps()
        {

        }

        [When(@"I send create message to service bus")]
        public async Task ExecuteAsync()
        {
            ClientMessageV1 message = new ClientMessageV1 { };
            ITX2ServiceBusSender _txcServiceBusSender = GetServiceBusSender();
            CreditRpc.CreditRpcClient client = GetCreditRpcClient();
            CommonClientService _commonClientService = new CommonClientService(_txcServiceBusSender, client);
            var s = await _commonClientService.SendCreateMessageAsync(9, "", message);
        }
    }
}
