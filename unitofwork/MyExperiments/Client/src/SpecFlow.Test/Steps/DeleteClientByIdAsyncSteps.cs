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
    public class DeleteClientByIdAsyncSteps : CommonHelper
    {
        public DeleteClientByIdAsyncSteps()
        {

        }

        [When(@"I delete a client by client Id")]
        public async Task ExecuteAsync()
        {
            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            await _repo.DeleteClientByIdAsync(8, _dbConnection);
        }
    }
}
