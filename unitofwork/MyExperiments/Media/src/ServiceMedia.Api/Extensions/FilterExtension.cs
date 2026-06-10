using Microsoft.Extensions.DependencyInjection;
using txc_common_lib.Filters;
using txc_common_lib.Filters.Models;

namespace ServiceMedia.Api.Extensions
{
    public static class FilterExtension
    {
        public static IServiceCollection AddFilter(this IServiceCollection services)
        {
            services.AddScoped<TenantFilter>();
            services.AddScoped<ModelValidationResultFilter>();
            return services;
        }
    }
}
