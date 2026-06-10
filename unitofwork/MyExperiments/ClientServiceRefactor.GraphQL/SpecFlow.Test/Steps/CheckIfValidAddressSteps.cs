using Repository;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public class CheckIfValidAddressSteps : CommonHelper
    {
        public CheckIfValidAddressSteps()
        {

        }

        [When(@"I check if it is a valid address")]
        public async Task ExecuteAsync()
        {
            ClientRepository _clientRepository;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _clientRepository);
            Tuple<bool,string> result = await _clientRepository.CheckIfValidAddress(11, 8, 6, _dbConnection);
        }
    }
}
