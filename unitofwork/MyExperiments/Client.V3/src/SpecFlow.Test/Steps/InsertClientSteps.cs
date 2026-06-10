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
    public class InsertClientSteps : CommonHelper
    {
        public InsertClientSteps()
        {

        }

        [When(@"I insert client")]
        public async Task ExecuteAsync()
        {
            string IdentityCode = Guid.NewGuid().ToString();
            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);

            GenerateClientIdentityCodeModel generateClientIdentityCode = new GenerateClientIdentityCodeModel
            {
                SequenceName = "client.seq_client_identity_code",
                IsFixReturnLength = true,
                ReturnLength = 20,
                PaddingCharacter = '0'
            };

            Domain.Entities.Client client = new Domain.Entities.Client {
                ClientName = "rsClient_00009",
                InvoiceRegisterNumber = "rs10000001",
                Status = 1,
                SecurityAlgorithm = 1,
                SecurityKey = "41509F6A06708BC1",
                NeedNotification = true,
                CanIssue = true,
                //CountryId = 6,
                //StateOrProvinceId = 8,
                //CityId = 10,
                //AddressStatus = 1,
                //Postcode = "asdfasdf",
                //District = "asdfsadf",
                //DetailAddressLine = "asdfasdfasdf",
            };

            IClientOperation _clientOperation = new ClientOperation();
            await _clientOperation.InsertClientAsync(9, client, generateClientIdentityCode, _dbConnection);
        }
    }
}
