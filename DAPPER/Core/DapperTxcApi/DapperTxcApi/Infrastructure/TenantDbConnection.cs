using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DapperTxcApi.Infrastructure
{
    public interface ITenantDbConnection
    {
        Task<Response<IDbConnection>> GetTenantDbConnection(string tenantId, bool isReadReplica, CancellationToken cancellationToken);
        Task<Response<IDbConnection>> GetTenantDbConnection(bool isReadReplica, CancellationToken cancellationToken);
    }

    public class TenantDbConnection : ITenantDbConnection
    {
        private readonly IConfiguration configuration;
        private string _tenantId;
        private IDbConnection existingConnection = null;

        public TenantDbConnection(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Response<IDbConnection>> GetTenantDbConnection(string tenantId, bool isReadReplica, CancellationToken cancellationToken)
        {
            _tenantId = tenantId;
            return await GetTenantDbConnection(isReadReplica, cancellationToken);
        }

        public async Task<Response<IDbConnection>> GetTenantDbConnection(bool isReadReplica, CancellationToken cancellationToken)
        {
            try
            {
                existingConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

                return Response.Success<IDbConnection>("Success", existingConnection);
            }
            catch (Exception ex)
            {
                return Response.Fail<IDbConnection>(ex.Message, null);
            }
        }
    }
}
