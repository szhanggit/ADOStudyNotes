using FluentValidation.TestHelper;
using Service.Validators;
using TXC.Proto.Client;
using Xunit;

namespace UnitTest.Test
{
    public class GetClientListRequestValidatorTest
    {
        [Fact]
        public void HappyPath()
        {
            GetClientListRequest _getClientListRequest = new GetClientListRequest
            {
                ClientId = 10,
                TenantName = "TW",
                TenantId = 7,
            };

            var validator = new GetClientListRequestValidator();
            var result = validator.TestValidate(_getClientListRequest);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Have_Error_When_PageNumber_Negative()
        {
            GetClientListRequest _getClientListRequest = new GetClientListRequest
            {
                ClientId = 10,
                TenantName = "TW",
                TenantId = 7,
                PageNumber = -1,
            };

            var validator = new GetClientListRequestValidator();
            var result = validator.TestValidate(_getClientListRequest);
            result.ShouldHaveValidationErrorFor(p => p.PageNumber);
        }
    }
}
