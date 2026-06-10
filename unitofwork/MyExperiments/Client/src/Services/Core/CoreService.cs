using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;

namespace Services.Core
{
    public interface ICoreService
    {
        Task<TenantConfig> GetConfig(string ConfigName, int TenantId);
        Task<Response<IDbConnection>> GetDBConnection(int TenantId);
    }
    public class CoreService : ICoreService
    {
        private readonly ITenantDbConnection _tenantDbConnection;
        private readonly ITenantConfigHelper _tenantConfigHelper;

        public CoreService(ITenantDbConnection tenantDbConnection
            , ITenantConfigHelper tenantConfigHelper)
        {
            _tenantDbConnection = tenantDbConnection;
            _tenantConfigHelper = tenantConfigHelper;
        }

        public async Task<TenantConfig> GetConfig(string ConfigName, int TenantId)
        {
            TenantConfig queueNameConfig = await _tenantConfigHelper.GetTenantConfigValue(ConfigName, TenantId);
            return queueNameConfig;
        }

        public async Task<Response<IDbConnection>> GetDBConnection(int TenantId)
        {
            Response<IDbConnection> conn = await _tenantDbConnection.GetTenantDbConnection(TenantId.ToString(), false, default);
            return conn;
        }
    }
}
