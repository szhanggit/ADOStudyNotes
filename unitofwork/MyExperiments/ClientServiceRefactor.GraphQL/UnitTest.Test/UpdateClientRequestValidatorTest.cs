using FluentValidation.TestHelper;
using Service.Validators;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class UpdateClientRequestValidatorTest
    {
        [Fact]
        public void HappyPath()
        {
            UpdateClientRequest _updateClientRequest = new UpdateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                TX2UserName = "stzhang",
                ClientId = 8,
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
                AddressStatus = 1,
            };

            var validator = new UpdateClientRequestValidator();
            var result = validator.TestValidate(_updateClientRequest);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_SubUrl_Have8Characters()
        {
            UpdateClientRequest _updateClientRequest = new UpdateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                TX2UserName = "stzhang",
                ClientId = 8,
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
                AddressStatus = 1,
                SubUrl = "http://www.edenred.com",
            };

            var validator = new UpdateClientRequestValidator();
            var result = validator.TestValidate(_updateClientRequest);
            result.ShouldHaveValidationErrorFor(p => p.SubUrl);
        }

        [Fact]
        public void Should_Have_Error_When_SalesEmail_InvalidEmailFormat()
        {
            UpdateClientRequest _updateClientRequest = new UpdateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                TX2UserName = "stzhang",
                ClientId = 8,
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
                AddressStatus = 1,
                SalesEmail = "asdfasdfasdf"
            };

            var validator = new UpdateClientRequestValidator();
            var result = validator.TestValidate(_updateClientRequest);
            result.ShouldHaveValidationErrorFor(p => p.SalesEmail);
        }
    }
}
