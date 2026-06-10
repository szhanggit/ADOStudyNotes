using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Core;
using System;
using System.Linq;
using System.Reflection;

namespace ServiceMedia.Api.Extensions
{
    public static class CoreServiceExtension
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
        {
            var ns = typeof(CreateMediaService).Namespace;
            typeof(CreateMediaService).Assembly.GetTypes().Where(t => string.Equals(t.Namespace, ns, StringComparison.Ordinal) && t.IsInterface == false)
                .ToList()
                .ForEach(fe =>
                {
                    services.AddScoped(fe);
                });



            return services;
        }
    }
}
