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
    public class CreateBXPClientSteps : CommonHelper
    {
        public CreateBXPClientSteps()
        {

        }

        [When(@"I create BXP client")]
        public async Task ExecuteAsync()
        {
            GenerateClientIdentityCodeModel generateClientIdentityCode = new GenerateClientIdentityCodeModel
            {
                SequenceName = "client.seq_client_identity_code",
                IsFixReturnLength = true,
                ReturnLength = 20,
                PaddingCharacter = '0'
            };
            Domain.Entities.Client createBXPClientRequest = new Domain.Entities.Client
            {
                ClientName = "rsClient_00011",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                //CountryId = 6,
                //StateOrProvinceId = 10,
                //CityId = 10,
                //Postcode = "Postcode",
                //District = "District",
                //DetailAddressLine = "DetailAddressLine",
                //Longitude = 12.3f,
                //Latitude = 123.2f,
                //AddressStatus = 1
            };

            string SecurityKey = "securityKey2";

            ClientRepository _repo;
            IDbConnection _dbConnection = InitDbConnection<ClientRepository>(out _repo);

            IClientOperation _clientOperation = new ClientOperation();
            await _clientOperation.CreateBXPClientAsync(createBXPClientRequest, SecurityKey, generateClientIdentityCode, _dbConnection);
        }
    }
}
