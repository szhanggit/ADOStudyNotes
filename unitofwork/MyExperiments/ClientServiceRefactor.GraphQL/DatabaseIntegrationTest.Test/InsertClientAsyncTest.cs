using Domain.Entities;
using Domain.Models;
using Repository;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Respository.Test
{
    public class InsertClientAsyncTest : CommonHelper
    {
        [Fact]
        public async Task InsertClientAsyncTest_WithAddress_Success()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            string ClientCode = "asdfasdfsadf";
            Client client = new Client {
                Description = "asdfasdf",
                Sales_Email = "xxx@gmail.com",
                Security_Algorithm = 2,
                Security_Key = "sdfsdfsdf",
                Sms_Entity_Id = "asdfsdf",
                SMS_Provider_Code = "adsfsdf",
                SMS_Sender_Name = "sdfsdf",
                Apply_Email_Subject = false,
                Status = 1,
                Sub_URL = "sdfd",
                Email_Sender_Address = "asdfsdf",
                Email_Sender_Name = "sdfsdfs",
                Banner_Media_Id = 1,
                Business_Type_Id = 1,
                Can_Issue = true,
                Client_Name = "Test100",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = ClientCode,
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };
            Address address = new Address {
                State_Province_Id = 0,
                Status = 1,
                Detail_Address_Line = "asdfasdfsdf",
                City_Id = 3,
                Country_Id = 4,
                Latitude = 234.23,
                Longitude = 234,
                District = "sdfasdfsaf",
                PostCode = "asdfasdfasdf",
            };
            await _databaseService.InsertClientAsync(client, address, _dbConnection);
        }

        [Fact]
        public async Task InsertClientAsyncTest_WithoutAddress_Success()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            string ClientCode = "sdfsdf004";
            Client client = new Client
            {
                Description = "asdfasdf",
                Sales_Email = "xxx@gmail.com",
                Security_Algorithm = 2,
                Security_Key = "sdfsdfsdf",
                Sms_Entity_Id = "asdfsdf",
                SMS_Provider_Code = "adsfsdf",
                SMS_Sender_Name = "sdfsdf",
                Apply_Email_Subject = false,
                Status = 1,
                Sub_URL = "sdfd",
                Email_Sender_Address = "asdfsdf",
                Email_Sender_Name = "sdfsdfs",
                Banner_Media_Id = 1,
                Business_Type_Id = 1,
                Can_Issue = true,
                Client_Name = "Test104",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = ClientCode,
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };

            await _databaseService.InsertClientAsync(client, null, _dbConnection);
        }
    }
}
