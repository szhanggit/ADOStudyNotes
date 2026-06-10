using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TXC.Common.Data.TenantDbConnection;
using Services.Queries.User;
using System;
using System.Data;
using System.Data.SqlClient;
using TXC.Common.Data;

namespace ServiceMedia.Api.Extensions
{
    public static class DataOperationExtension
    {
        public static void AddDataOperation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDapperOperation>(dapper => new DapperOperation());    
            services.AddScoped<IDbCommand>(cmd => new SqlCommand { CommandTimeout = Convert.ToInt32(configuration.GetSection("SqlCommand:CommandTimeout").Value) });

            services.AddScoped<ITenantDbConnection, TenantDbConnection>();
            services.Configure<ShardConfiguration>(options => configuration.GetSection("ShardConfiguration").Bind(options));
            services.AddSingleton<ITenantShardMapHelper, TenantShardMapHelper>();
        }


    }
}
