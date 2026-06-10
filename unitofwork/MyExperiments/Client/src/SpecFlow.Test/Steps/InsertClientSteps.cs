using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TXC.Proto.Client;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public class InsertClientSteps : CommonHelper
    {
        public InsertClientSteps()
        {

        }

        [When(@"I insert client")]
        public async Task ExecuteAsync()
        {
            CreateClientRequest createClientRequest = new CreateClientRequest
            {
                ClientName = "rsClient_00006",
                InvoiceRegisterNumber = "rs10000001",
                Status = 1,
                SecurityAlgorithm = 1,
                SecurityKey = "41509F6A06708BC1",
                NeedNotification = true,
                CanIssue = true,
            };

            string IdentityCode = Guid.NewGuid().ToString();
            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            var result = await _repo.InsertClientAsync(createClientRequest, IdentityCode, _dbConnection);
        }
    }
}
