using Domain.Entities;
using Domain.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace RespositoryTest.Test
{
    public class UpdateClientAsyncTest : CommonHelper
    {
        [Fact]
        public async Task UpdateClientAsyncTest_NotExistClientCode_Failure()
        {
            //Has existing address, and need to update old address.
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);

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
                Client_Name = "sdfsdfdsf",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = "____________________________",
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };
            Address address = new Address
            {
                State_Province_Id = 0,
                Status = 1,
                Detail_Address_Line = "asdfasdfsdf",
                City_Id = 3,
                Country_Id = 4,
                Latitude = 234.23,
                Longitude = 234,
                District = "sdfasdfsaf",
                Postcode = "asdfasdfasdf_update",
            };

            bool _updateSuccess = await _databaseService.UpdateClientAsync(client, address, _dbConnection);
            Assert.False(_updateSuccess);
        }

        [Fact]
        public async Task UpdateClientAsyncTest_ClientName_Success()
        {
            //No existing address, and need to create new address.
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);

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
                Client_Name = "sdfsdfdsf_update",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = "sdfsdf",
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };
            Address address = new Address
            {
                State_Province_Id = 0,
                Status = 1,
                Detail_Address_Line = "asdfasdfsdf",
                City_Id = 3,
                Country_Id = 4,
                Latitude = 234.23,
                Longitude = 234,
                District = "sdfasdfsaf",
                Postcode = "asdfasdfasdf",
            };

            bool _updateSuccess = await _databaseService.UpdateClientAsync(client, address, _dbConnection);
            Assert.True(_updateSuccess);
        }

        [Fact]
        public async Task UpdateClientAsyncTest_Address_Success()
        {
            //Has existing address, and need to update old address.
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);

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
                Client_Name = "sdfsdfdsf",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = "sdfsdf",
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };
            Address address = new Address
            {
                State_Province_Id = 0,
                Status = 1,
                Detail_Address_Line = "asdfasdfsdf",
                City_Id = 3,
                Country_Id = 4,
                Latitude = 234.23,
                Longitude = 234,
                District = "sdfasdfsaf",
                Postcode = "asdfasdfasdf_update",
            };

            bool _updateSuccess = await _databaseService.UpdateClientAsync(client, address, _dbConnection);
            Assert.True(_updateSuccess);
        }

        [Fact]
        public async Task UpdateClientAsyncTest_UpdateClientWithoutAddress_Success()
        {
            //No existing address, and do not need to create new address.
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);

            Client client = new Client
            {
                Description = "asdfasdf_update",
                Sales_Email = "xxx@gmail.com_update",
                Security_Algorithm = 2,
                Security_Key = "sdfsdfsdf_update",
                Sms_Entity_Id = "asdfsdf_update",
                SMS_Provider_Code = "adsfsdf",
                SMS_Sender_Name = "sdfsdf_update",
                Apply_Email_Subject = false,
                Status = 1,
                Sub_URL = "sdfd",
                Email_Sender_Address = "asdfsdf_update",
                Email_Sender_Name = "sdfsdfs_update",
                Banner_Media_Id = 1,
                Business_Type_Id = 1,
                Can_Issue = true,
                Client_Name = "sdfsdfdsf_update",
                Contact_Email = "xxx@gmail.com_update",
                Contact_Name = "sdfasdfds_update",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = "sdfsdf",
                Invoice_Register_Number = "sdfasdfsd_update",
                Invoice_Title = "dsfasf_update",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf_update",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };

            bool _updateSuccess = await _databaseService.UpdateClientAsync(client, null, _dbConnection);
            Assert.True(_updateSuccess);
        }

        [Fact]
        public async Task UpdateClientAsyncTest_UpdateJustClientWithAddress_Success()
        {
            //No existing address, and do not need to create new address.
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);

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
                Client_Name = "sdfsdfdsf",
                Contact_Email = "xxx@gmail.com",
                Contact_Name = "sdfasdfds",
                Contact_Phone = "sdfsdfds",
                Email_Footer_Media_Id = 2,
                Email_Header_Media_Id = 2,
                Email_Provider_Code = "sdfsdf",
                Identity_Code = "sdfsdf",
                Invoice_Register_Number = "sdfasdfsd",
                Invoice_Title = "dsfasf",
                Logo_Media_Id = 3,
                Mandatory_Auto_Billing = true,
                Memo = "sdfsdf",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4
            };

            bool _updateSuccess = await _databaseService.UpdateClientAsync(client, null, _dbConnection);
            Assert.True(_updateSuccess);
        }
    }
}
