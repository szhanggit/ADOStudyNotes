using Domain.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TXC.Common.Services.KeyVault;

namespace Client.Api.Extensions
{
    public static class KeyVaultExtension
    {
        public static IServiceCollection ConfigureKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IKeyVaultServices, KeyVaultService>();
            return services;
        }
    }
}
