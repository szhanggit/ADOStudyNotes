using Repository;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class CheckClientIdAsyncSteps : CommonHelper
    {
        public CheckClientIdAsyncSteps()
        {

        }

        [When(@"I check client by client Id")]
        public async Task ExecuteAsync()
        {
            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            //var result = await _repo.CheckClientIdAsync(1, _dbConnection);

            IClientOperation _clientOperation = new ClientOperation();
            var result = await _clientOperation.CheckClientIdAsync(1, _dbConnection);
        }
    }
}
