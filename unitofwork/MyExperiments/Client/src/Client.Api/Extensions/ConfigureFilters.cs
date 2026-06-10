using Microsoft.Extensions.DependencyInjection;
using Client.Api.Filters;

namespace Client.Api.Extensions
{
    public static class ConfigureFilters
    {
        public static IServiceCollection RegisterFilter(this IServiceCollection services)
        {
            services.AddScoped<TenantFilter>();
            services.AddScoped<ModelValidationResultFilter>();
            return services;
        }
    }
}
