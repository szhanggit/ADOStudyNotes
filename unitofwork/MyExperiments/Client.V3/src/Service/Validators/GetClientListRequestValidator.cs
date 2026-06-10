using FluentValidation;
using TXC.Proto.Client;

namespace Service.Validators
{
    public class GetClientListRequestValidator : AbstractValidator<GetClientListRequest>
    {
        public GetClientListRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.TenantId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(p => p.ClientId).Must(x => x == null || x > 0);
            RuleFor(p => p.PageNumber).Must(x => x == null || x > 0);
            RuleFor(p => p.RowCount).Must(x => x == null || x > 0);
        }
    }
}
