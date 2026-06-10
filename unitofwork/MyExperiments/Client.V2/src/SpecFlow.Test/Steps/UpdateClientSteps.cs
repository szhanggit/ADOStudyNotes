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
    public class UpdateClientSteps : CommonHelper
    {
        public UpdateClientSteps()
        {

        }

        [When(@"I update client by client Id")]
        public async Task ExecuteAsync()
        {
            UpdateClientRequest updateClientRequest = new UpdateClientRequest { 
                ClientId = 5,
                ClientName = "rsClient_00001",
                IdentityCode = "900000000000000000019",
                InvoiceRegisterNumber = "rs10000003",
                Status = 1,
                SecurityAlgorithm = 1,
                SecurityKey = "41509F6A06708BC1",
                NeedNotification = true,
                CanIssue = true,                
            };

            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            var result = await _repo.UpdateClientAsync(updateClientRequest, _dbConnection);
        }
    }
}
