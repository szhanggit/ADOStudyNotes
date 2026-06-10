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
    public class GetClientSteps : CommonHelper
    {
        public GetClientSteps()
        {

        }

        [When(@"I get client")]
        public async Task ExecuteAsync()
        {
            GetClientListRequest getClientRequest = new GetClientListRequest
            {
                PageNumber = 1,
                RowCount = 20,
                SearchKeyword = "0000023"
            };

            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            var result = await _repo.GetClientAsync(getClientRequest, _dbConnection);
        }
    }
}
