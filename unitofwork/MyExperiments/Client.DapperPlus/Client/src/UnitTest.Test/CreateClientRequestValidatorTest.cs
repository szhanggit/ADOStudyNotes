using FluentValidation.TestHelper;
using Services.Validators;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class CreateClientRequestValidatorTest
    {
        [Fact]
        public void HappyPath()
        {
            CreateClientRequest _createClientRequest = new CreateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
            };

            var validator = new CreateClientRequestValidator();
            var result = validator.TestValidate(_createClientRequest);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_SubUrl_Have8Characters()
        {
            CreateClientRequest _createClientRequest = new CreateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
                SubUrl = "http://www.edenred.com",
            };

            var validator = new CreateClientRequestValidator();
            var result = validator.TestValidate(_createClientRequest);
            result.ShouldHaveValidationErrorFor(p => p.SubUrl);
        }

        [Fact]
        public void Should_Have_Error_When_SalesEmail_InvalidEmailFormat()
        {
            CreateClientRequest _createClientRequest = new CreateClientRequest
            {
                TenantId = 7,
                TenantName = "TW",
                ClientName = "SevenEleven",
                InvoiceRegisterNumber = "InvoiceRegisterNumber",
                Status = 1,
                SecurityAlgorithm = 3,
                SecurityKey = "kljshjfklsjdklfj",
                NeedNotification = true,
                CanIssue = true,
                SalesEmail = "asdfasdfasdf"
            };

            var validator = new CreateClientRequestValidator();
            var result = validator.TestValidate(_createClientRequest);
            result.ShouldHaveValidationErrorFor(p => p.SalesEmail);
        }
    }
}
