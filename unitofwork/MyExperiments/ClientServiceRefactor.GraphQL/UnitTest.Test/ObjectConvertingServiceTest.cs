using Domain.Entities;
using Domain.Models;
using TXC.Common.MessageContract.Client;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class ObjectConvertingServiceTest : CommonHelper
    {
        [Fact]
        public void ConvertUpdateClientRequestToClientMessageV1_HappyPath_ShallReturnSuccess()
        {
            UpdateClientRequest _updateClientRequest = new UpdateClientRequest
            {
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                Description = "Description",
                SecurityKey = "SecurityKey",
                DetailAddressLine = "DetailAddressLine",
                SmsEntityId = "SmsEntityId",
                District = "District",
                SmsProviderCode = "SmsProviderCode",
                AddressStatus = 1,
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 2,
                Status = 3,
                ApplyEmailSubject = true,
                BannerMediaId = 3,
                SubUrl = "SubUrl",
                EmailSenderAddress = "EmailSenderAddress",
                BusinessTypeId = 1,
                EmailSenderName = "EmailSenderName",
                CanIssue = true,
                CityId = 3,
                ClientId = 3,
                ClientName = "sdf",
                ContactEmail = "sdfsdf",
                ContactName = "asdfsdf",
                ContactPhone = "324234234",
                CountryId = 2,
                EmailFooterMediaId = 2,
                EmailHeaderMediaId = 3,
                EmailProviderCode = "EmailProviderCode",
                IdentityCode = "IdentityCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 23,
                LogoMediaId = 2,
                Longitude = 66,
                MandatoryAutoBilling = null,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 2,
                Postcode = "Postcode",
                TenantId = 7,
                TenantName = "TW",
                TX2UserName = "stzhang",
                VoucherIssuerId = 2
            };

            ClientMessageV1 _result = GetObjectConvertingService().ConvertUpdateClientRequestToClientMessageV1(_updateClientRequest);
        }

        [Fact]
        public void ConvertCreateClientRequestToClientMessageV1_HappyPath_ShallReturnSuccess()
        {
            CreateClientRequest _createClientRequest = new CreateClientRequest
            {
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                Description = "Description",
                SecurityKey = "SecurityKey",
                DetailAddressLine = "DetailAddressLine",
                SmsEntityId = "SmsEntityId",
                District = "District",
                SmsProviderCode = "SmsProviderCode",
                AddressStatus = 1,
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 2,
                Status = 3,
                ApplyEmailSubject = true,
                BannerMediaId = 3,
                SubUrl = "SubUrl",
                EmailSenderAddress = "EmailSenderAddress",
                BusinessTypeId = 1,
                EmailSenderName = "EmailSenderName",
                CanIssue = true,
                CityId = 3,
                ClientName = "sdf",
                ContactEmail = "sdfsdf",
                ContactName = "asdfsdf",
                ContactPhone = "324234234",
                CountryId = 2,
                EmailFooterMediaId = 2,
                EmailHeaderMediaId = 3,
                EmailProviderCode = "EmailProviderCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 23,
                LogoMediaId = 2,
                Longitude = 66,
                MandatoryAutoBilling = null,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 2,
                Postcode = "Postcode",
                TenantId = 7,
                TenantName = "TW",
                TX2UserName = "stzhang",
                VoucherIssuerId = 2,
            };

            ClientMessageV1 _result = GetObjectConvertingService().ConvertCreateClientRequestToClientMessageV1(_createClientRequest, null, null);

        }

        [Fact]
        public void CreateBXPClientRequestToClientMessageV1_HappyPath_ShallReturnSuccess()
        {
            CreateBXPClientRequest _createClientRequest = new CreateBXPClientRequest { 
                StateOrProvinceId = 1,
                CityId = 2,
                ClientName = "sdfsdf",
                CountryId = 6,
                DetailAddressLine = "DetailAddressLine",
                District = "District",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 23,
                Longitude = 34,
                Postcode = "Postcode",
                TenantId = 3,
                TenantName = "TenantName"
            };
            int? ClientId = 4;
            string ClientCode = "ClientCode";
            string securityKey = "securityKey";

            ClientMessageV1 _result = GetObjectConvertingService().CreateBXPClientRequestToClientMessageV1(_createClientRequest, ClientId, ClientCode, securityKey);
        }

        [Fact]
        public void CombineClientAddressIntoClientAddressModel_HappyPath_ShallReturnSuccess()
        {
            Domain.Entities.Client client = new Domain.Entities.Client { 
                Sales_Email = "Sales_Email",
                Security_Algorithm = 1,
                Security_Key = "Security_Key",
                Sms_Entity_Id = "Sms_Entity_Id",
                SMS_Provider_Code = "SMS_Provider_Code",
                SMS_Sender_Name = "SMS_Sender_Name",
                Status = 1,
                Sub_URL = "Sub_URL",
                Apply_Email_Subject = true,
                Email_Sender_Address = "Email_Sender_Address",
                Email_Sender_Name = "Email_Sender_Name",
                Address_Id = 234,
                Banner_Media_Id = 3,
                Business_Type_Id = 34,
                Description = "Description",
                Can_Issue = true,
                Client_Id = 3,
                Client_Name = "adfsdf",
                Contact_Email = "Contact_Email",
                Contact_Name = "Contact_Name",
                Contact_Phone = "Contact_Phone",
                Email_Footer_Media_Id = 3,
                Email_Header_Media_Id = 5,
                Identity_Code = "Identity_Code",
                Invoice_Register_Number = "Invoice_Register_Number",
                Email_Provider_Code = "Email_Provider_Code",
                Invoice_Title = "Invoice_Title",
                Logo_Media_Id = 5,
                Mandatory_Auto_Billing = true,
                Memo = "Memo",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4,                
            };
            Address address = new Address { 
                State_Province_Id = 3,
                Detail_Address_Line = "Detail_Address_Line",
                District = "District",
                Status = 1,
                Address_Id = 234,
                City_Id = 3,
                Country_Id = 4,
                Latitude = 34,
                Longitude = 45,
                PostCode = "PostCode"
            };

            ClientAddressModel result = GetObjectConvertingService().CombineClientAddressIntoClientAddressModel(client, address);
        }

        [Fact]
        public void ConvertClientModelToClientListItem_HappyPath_ShallReturnSuccess()
        {
            Domain.Models.ClientModel _client = new Domain.Models.ClientModel { 
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SMSProviderCode = "SMSProviderCode",
                SMSSenderName = "SMSSenderName",
                Status = 1,
                SubURL = "SubURL",
                ApplyEmailSubject = true,
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                Description = "Description",
                AddressId = 234,
                BannerMediaId = 3,
                BusinessTypeId = 45,
                CanIssue = true,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                EmailFooterMediaId = 4,
                EmailHeaderMediaId = 5,
                EmailProviderCode = "EmailProviderCode",
                IdentityCode = "IdentityCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                LogoMediaId = 4,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 5,
                VoucherIssuerId = 5
            };

            ClientListItem result = GetObjectConvertingService().ConvertClientModelToClientListItem(_client);
        }

        [Fact]
        public void ConvertClientModelToClientListItemWithoutAddress_HappyPath_ShallReturnSuccess()
        {
            Domain.Models.ClientModel _client = new Domain.Models.ClientModel { 
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                Description = "Description",
                AddressId = 234,
                ApplyEmailSubject = true,
                BannerMediaId = 4,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SMSProviderCode = "SMSProviderCode",
                SMSSenderName = "SMSSenderName",
                CanIssue = true,
                Status = 1,
                SubURL = "SubURL",
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                BusinessTypeId = 7,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                EmailFooterMediaId = 6,
                EmailHeaderMediaId = 7,
                EmailProviderCode = "EmailProviderCode",
                IdentityCode = "IdentityCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                LogoMediaId = 5,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 7,
                VoucherIssuerId = 546
            };

            ClientListItem result = GetObjectConvertingService().ConvertClientModelToClientListItemWithoutAddress(_client);
        }

        [Fact]
        public void ConvertClientEntityToClientListItem_HappyPath_ShallReturnSuccess()
        {
            Domain.Entities.Client client = new Domain.Entities.Client
            {
                Sales_Email = "Sales_Email",
                Security_Algorithm = 1,
                Security_Key = "Security_Key",
                Sms_Entity_Id = "Sms_Entity_Id",
                SMS_Provider_Code = "SMS_Provider_Code",
                SMS_Sender_Name = "SMS_Sender_Name",
                Status = 1,
                Sub_URL = "Sub_URL",
                Apply_Email_Subject = true,
                Email_Sender_Address = "Email_Sender_Address",
                Email_Sender_Name = "Email_Sender_Name",
                Address_Id = 234,
                Banner_Media_Id = 3,
                Business_Type_Id = 34,
                Description = "Description",
                Can_Issue = true,
                Client_Id = 3,
                Client_Name = "adfsdf",
                Contact_Email = "Contact_Email",
                Contact_Name = "Contact_Name",
                Contact_Phone = "Contact_Phone",
                Email_Footer_Media_Id = 3,
                Email_Header_Media_Id = 5,
                Identity_Code = "Identity_Code",
                Invoice_Register_Number = "Invoice_Register_Number",
                Email_Provider_Code = "Email_Provider_Code",
                Invoice_Title = "Invoice_Title",
                Logo_Media_Id = 5,
                Mandatory_Auto_Billing = true,
                Memo = "Memo",
                Need_Notification = true,
                Notification_Provider_Code_Id = 4,
                Voucher_Issuer_Id = 4,
            };

            ClientListItem result = GetObjectConvertingService().ConvertClientEntityToClientListItem(client);
        }

        [Fact]
        public void ConvertCreateClientRequestToClientEntity_HappyPath_ShallReturnSuccess()
        {
            CreateClientRequest request = new CreateClientRequest { 
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SmsProviderCode = "SmsProviderCode",
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 4,
                Status = 1,
                SubUrl = "SubUrl",
                AddressStatus = 1,
                ApplyEmailSubject = true,
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                BannerMediaId = 4,
                BusinessTypeId = 6,
                CanIssue = true,
                CityId = 64,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                CountryId = 6,
                Description = "Description",
                DetailAddressLine = "DetailAddressLine",
                District = "District",
                EmailFooterMediaId = 6,
                EmailHeaderMediaId = 8,
                EmailProviderCode = "EmailProviderCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 56,
                Longitude = 560,
                LogoMediaId = 6,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 100,
                Postcode = "Postcode",
                TenantId = 4,
                TenantName = "TenantName",
                TX2UserName = "stzhang",
                VoucherIssuerId = 4,                
            };
            string _identityCode = "ClientCode";

            Domain.Entities.Client result = GetObjectConvertingService().ConvertCreateClientRequestToClientEntity(request, _identityCode);
        }

        [Fact]
        public void ConvertCreateClientRequestToAddressEntity_HappyPath_ShallReturnSuccess()
        {
            CreateClientRequest request = new CreateClientRequest {
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SmsProviderCode = "SmsProviderCode",
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 4,
                Status = 1,
                SubUrl = "SubUrl",
                AddressStatus = 1,
                ApplyEmailSubject = true,
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                BannerMediaId = 4,
                BusinessTypeId = 6,
                CanIssue = true,
                CityId = 64,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                CountryId = 6,
                Description = "Description",
                DetailAddressLine = "DetailAddressLine",
                District = "District",
                EmailFooterMediaId = 6,
                EmailHeaderMediaId = 8,
                EmailProviderCode = "EmailProviderCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 56,
                Longitude = 560,
                LogoMediaId = 6,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 100,
                Postcode = "Postcode",
                TenantId = 4,
                TenantName = "TenantName",
                TX2UserName = "stzhang",
                VoucherIssuerId = 4,
            };

            Address result = GetObjectConvertingService().ConvertCreateClientRequestToAddressEntity(request);
        }

        [Fact]
        public void ConvertUpdateClientRequestToClientEntity_HappyPath_ShallReturnSuccess()
        {
            UpdateClientRequest request = new UpdateClientRequest {
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SmsProviderCode = "SmsProviderCode",
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 4,
                Status = 1,
                SubUrl = "SubUrl",
                AddressStatus = 1,
                ApplyEmailSubject = true,
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                BannerMediaId = 4,
                BusinessTypeId = 6,
                CanIssue = true,
                CityId = 64,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                CountryId = 6,
                Description = "Description",
                DetailAddressLine = "DetailAddressLine",
                District = "District",
                EmailFooterMediaId = 6,
                EmailHeaderMediaId = 8,
                EmailProviderCode = "EmailProviderCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 56,
                Longitude = 560,
                LogoMediaId = 6,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 100,
                Postcode = "Postcode",
                TenantId = 4,
                TenantName = "TenantName",
                TX2UserName = "stzhang",
                VoucherIssuerId = 4,
            };

            Domain.Entities.Client result = GetObjectConvertingService().ConvertUpdateClientRequestToClientEntity(request);
        }

        [Fact]
        public void ConvertUpdateClientRequestToAddressEntity_HappyPath_ShallReturnSuccess()
        {
            UpdateClientRequest request = new UpdateClientRequest {
                SalesEmail = "SalesEmail",
                SecurityAlgorithm = 1,
                SecurityKey = "SecurityKey",
                SmsEntityId = "SmsEntityId",
                SmsProviderCode = "SmsProviderCode",
                SmsSenderName = "SmsSenderName",
                StateOrProvinceId = 4,
                Status = 1,
                SubUrl = "SubUrl",
                AddressStatus = 1,
                ApplyEmailSubject = true,
                EmailSenderAddress = "EmailSenderAddress",
                EmailSenderName = "EmailSenderName",
                BannerMediaId = 4,
                BusinessTypeId = 6,
                CanIssue = true,
                CityId = 64,
                ClientName = "ClientName",
                ContactEmail = "ContactEmail",
                ContactName = "ContactName",
                ContactPhone = "ContactPhone",
                CountryId = 6,
                Description = "Description",
                DetailAddressLine = "DetailAddressLine",
                District = "District",
                EmailFooterMediaId = 6,
                EmailHeaderMediaId = 8,
                EmailProviderCode = "EmailProviderCode",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                InvoiceTitle = "InvoiceTitle",
                Latitude = 56,
                Longitude = 560,
                LogoMediaId = 6,
                MandatoryAutoBilling = true,
                Memo = "Memo",
                NeedNotification = true,
                NotificationProviderCodeId = 100,
                Postcode = "Postcode",
                TenantId = 4,
                TenantName = "TenantName",
                TX2UserName = "stzhang",
                VoucherIssuerId = 4,
            };

            Address result = GetObjectConvertingService().ConvertUpdateClientRequestToAddressEntity(request);
        }
    }
}
