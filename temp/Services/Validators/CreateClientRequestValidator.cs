using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Proto.Client;

namespace Services.Validators
{
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.TenantId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(p => p.ClientName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100);
            RuleFor(p => p.InvoiceRegisterNumber)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100);
            RuleFor(p => p.Status).Must(x => x == 0 || x == 1).WithMessage("Client status can only be 0 or 1.");
            RuleFor(p => p.SecurityAlgorithm).Must(x => x >= 1).Must(x => x == 1 || x == 2).WithMessage("Security algorithm can only be 1 or 2.");
            RuleFor(p => p.SecurityKey)
                .MaximumLength(32).WithMessage("SecurityKey cannot be longer than 32 characters.");

            RuleFor(p => p.MandatoryAutoBilling).Must(x => x == null || x == false || x == true);
            RuleFor(p => p.InvoiceTitle).Must(x => x == null || x.Length <= 100);
            RuleFor(p => p.SubUrl).Must(x => x == null || x.Length <= 6);
            RuleFor(p => p.EmailProviderCode).Must(x => x == null || x.Length <= 8);
            RuleFor(p => p.EmailSenderName).Must(x => x == null || x.Length <= 255).WithMessage("Email sender name shall be maximum 255 characters.");
            RuleFor(p => p.EmailSenderAddress).Must(x => x == null || x.Length <= 255).WithMessage("Email sender address shall be maximum 255 characters.").EmailAddress().WithMessage("Invalid EmailSenderAddress.");
            RuleFor(p => p.SmsProviderCode).Must(x => x == null || x.Length <= 8);
            RuleFor(p => p.SmsSenderName).Must(x => x == null || x.Length <= 255).WithMessage("Sms sender name shall be maximum 255 characters.");
            RuleFor(p => p.SmsEntityId).Must(x => x == null || x.Length <= 30).WithMessage("SMS EntityId shall be maximum 30 characters.");
            RuleFor(p => p.SalesEmail).Must(x => x == null || x.Length <= 255).WithMessage("Sales Email shall be maximum 255 characters.").EmailAddress().WithMessage("Invalid SalesEmail.");
            RuleFor(p => p.ContactName).Must(x => x == null || x.Length <= 30).WithMessage("Contact name shall be maximum 30 characters.");
            RuleFor(p => p.ContactEmail).Must(x => x == null || x.Length <= 255).WithMessage("Contact Email shall be maximum 255 characters.").EmailAddress().WithMessage("Invalid ContactEmail.");
            RuleFor(p => p.ContactPhone).Must(x => x == null || x.Length <= 50).WithMessage("Contact phone shall be maximum 50 characters."); //CNM 有email没有phone
            RuleFor(p => p.Memo).Must(x => x == null || x.Length <= 2000);
            RuleFor(p => p.Description).Must(x => x == null || x.Length <= 500);
            RuleFor(p => p.NotificationProviderCodeId).Must(x => x == null || x > 0);
            RuleFor(p => p.LogoMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.BannerMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.EmailHeaderMediaId).Must(x => x == null || x > 0).WithMessage("Invalid EmailHeaderMediaId.");
            RuleFor(p => p.EmailFooterMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.VoucherIssuerId).Must(x => x == null || x > 0);
            RuleFor(p => p.BusinessTypeId).Must(x => x == null || x > 0);
            RuleFor(p => p.CountryId).NotNull().WithMessage("The CountryId is required.");
            RuleFor(p => p.StateOrProvinceId).NotNull().WithMessage("The StateOrProvinceId field is required.");
            RuleFor(p => p.CityId).NotNull().WithMessage("The CountryId is required.");
            RuleFor(p => p.AddressStatus).NotNull().WithMessage("AddressStatus required.").Must(x => x == 0 || x == 1).WithMessage("AddressStatus can only be 0 or 1.");
        }
    }
}
