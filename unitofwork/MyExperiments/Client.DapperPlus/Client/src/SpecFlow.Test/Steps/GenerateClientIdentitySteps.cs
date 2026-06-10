using Domain.Models;
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
    public class GenerateClientIdentitySteps : CommonHelper
    {
        public GenerateClientIdentitySteps()
        {

        }

        [When(@"I get client identity")]
        public async Task ExecuteAsync()
        {
            GenerateClientIdentityCodeModel generateClientIdentityCodeModel = new GenerateClientIdentityCodeModel
            {
                SequenceName = "client.seq_client_identity_code",
                IsFixReturnLength = true,
                ReturnLength = 20,
                PaddingCharacter = '0'
            };

            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);
            var result = await _repo.GenerateClientIdentityAsync(generateClientIdentityCodeModel, _dbConnection);
        }
    }
}
