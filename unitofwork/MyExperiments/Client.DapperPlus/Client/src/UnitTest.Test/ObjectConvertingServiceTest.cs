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
    }
}
