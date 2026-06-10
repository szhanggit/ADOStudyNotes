using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Proto.Client;

namespace Services.Validators.Client
{
    public class CreateBXPClientValidator : AbstractValidator<CreateBXPClientRequest>
    {
        public CreateBXPClientValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(request => request.Postcode)
                .MaximumLength(20);

            RuleFor(request => request.DetailAddressLine)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(request => request.InvoiceTitle)
                .MaximumLength(100);


            RuleFor(request => request.CityId)
             .NotEqual(0);

            RuleFor(request => request.Latitude)
             .NotEqual(0);

            RuleFor(request => request.Longitude)
             .NotEqual(0);

            RuleFor(request => request.ClientName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(20);

            RuleFor(request => request.CountryId)
             .NotEqual(0);

            RuleFor(request => request.District)
             .MaximumLength(100);

            RuleFor(request => request.InvoiceRegisterNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        }
    }
}
