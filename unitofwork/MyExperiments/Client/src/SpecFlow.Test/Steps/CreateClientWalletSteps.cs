using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TXC.Common.CacheManagement.Resolver;
using TXC.Common.MessageContract;
using TXC.Proto.Credit;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public class CreateClientWalletSteps : CommonHelper
    {
        public CreateClientWalletSteps()
        {

        }

        [When(@"I create client wallet")]
        public async Task ExecuteAsync()
        {
            CreateClientWalletRequest createClientWalletRequest = new CreateClientWalletRequest
            {
                AccountName = "sdf",
                ClientId = 1,
                TenantId = 9,
                TenantName = "GL"
            };

            ITX2ServiceBusSender _txcServiceBusSender = GetServiceBusSender();
            using var channel = GrpcChannel.ForAddress("http://localhost:9022");
            CreditRpc.CreditRpcClient client = new CreditRpc.CreditRpcClient(channel);
            CommonClientService _commonClientService = new CommonClientService(_txcServiceBusSender, client);            
            var result = await _commonClientService.CreateClientWalletAsync(createClientWalletRequest);
        }
    }
}
