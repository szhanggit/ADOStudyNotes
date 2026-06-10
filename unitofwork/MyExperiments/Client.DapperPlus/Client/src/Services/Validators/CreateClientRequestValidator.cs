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
            RuleFor(p => p.Status)
                .NotEmpty()
                .NotNull()
                .Must(x => x == 0 || x == 1);
            RuleFor(p => p.SecurityAlgorithm)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(p => p.SecurityKey)
                .NotEmpty()
                .NotNull()
                .MaximumLength(32).WithMessage("SecurityKey cannot be longer than 32 characters.");
            RuleFor(p => p.NeedNotification)
                .NotEmpty()
                .NotNull()
                .Must(x => x == false || x == true);
            RuleFor(p => p.CanIssue)
                .NotEmpty()
                .NotNull()
                .Must(x => x == false || x == true);

            RuleFor(p => p.MandatoryAutoBilling).Must(x => x == null || x == false || x == true);
            RuleFor(p => p.InvoiceTitle).Must(x => x == null || x.Length <= 100);
            RuleFor(p => p.SubUrl).Must(x => x == null || x.Length <= 6);
            RuleFor(p => p.EmailProviderCode).Must(x => x == null || x.Length <= 8);
            RuleFor(p => p.EmailSenderName).Must(x => x == null || x.Length <= 255);
            RuleFor(p => p.EmailSenderAddress).Must(x => x == null || x.Length <= 255).EmailAddress();
            RuleFor(p => p.ApplyEmailSubject).Empty().Must(x => x == null || x == false || x == true);
            RuleFor(p => p.SmsProviderCode).Must(x => x == null || x.Length <= 8);
            RuleFor(p => p.SmsSenderName).Must(x => x == null || x.Length <= 255);
            RuleFor(p => p.SmsEntityId).Must(x => x == null || x.Length <= 30);
            RuleFor(p => p.SalesEmail).Must(x => x == null || x.Length <= 255).EmailAddress();
            RuleFor(p => p.ContactName).Must(x => x == null || x.Length <= 30);
            RuleFor(p => p.ContactEmail).Must(x => x == null || x.Length <= 255).EmailAddress();
            RuleFor(p => p.ContactPhone).Must(x => x == null || x.Length <= 50);
            RuleFor(p => p.Memo).Must(x => x == null || x.Length <= 2000);
            RuleFor(p => p.Description).Must(x => x == null || x.Length <= 500);
            RuleFor(p => p.MandatoryAutoBilling).Empty().Must(x => x == null || x == false || x == true);
            RuleFor(p => p.NotificationProviderCodeId).Must(x => x == null || x > 0);
            RuleFor(p => p.LogoMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.BannerMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.EmailHeaderMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.EmailFooterMediaId).Must(x => x == null || x > 0);
            RuleFor(p => p.VoucherIssuerId).Must(x => x == null || x > 0);
            RuleFor(p => p.BusinessTypeId).Must(x => x == null || x > 0);
        }
    }
}
