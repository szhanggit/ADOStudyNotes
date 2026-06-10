using FluentValidation;
using FluentValidation.AspNetCore;
using Service.Validators;
using System.Diagnostics.CodeAnalysis;

namespace Service.Extensions
{
    [ExcludeFromCodeCoverageAttribute]
    public static class FluentValidationExtension
    {
        public static void AddFluentValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<UpdateClientRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GetClientListRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateClientRequestValidator>();
            services.AddFluentValidationAutoValidation();
        }
    }
}
