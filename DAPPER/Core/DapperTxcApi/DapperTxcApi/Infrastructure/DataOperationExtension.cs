using DapperTxcApi.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTxcApi.Infrastructure
{
    public static class DataOperationExtension
    {
        public static void AddDataOperation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDapperOperation>(dapper => new DapperOperation());
            services.AddScoped<IDbCommand>(cmd => new SqlCommand { CommandTimeout = Convert.ToInt32(configuration.GetSection("SqlCommand:CommandTimeout").Value) });

            services.AddScoped<ITenantDbConnection, TenantDbConnection>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }
    }
}
