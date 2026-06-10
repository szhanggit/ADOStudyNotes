using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Validators;

namespace ServiceClient.Api.Extensions
{
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
