using Moq;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.Data.TenantDbConnection;
using TXC.Common.Domain;
using Xunit;

namespace UnitTest.Test
{
    public class CoreServiceTest
    {
        private ICoreService _coreService = null;
        private Mock<ITenantDbConnection> _tenantDbConnection = null;
        private Mock<ITenantConfigHelper> _tenantConfigHelper = null;

        public CoreServiceTest()
        {
            TenantConfig _tenantConfig = new TenantConfig { 
                ConfigName = "ServiceBusQueueName",
                Value = "QueueName"
            };
            Response<IDbConnection> response = new Response<IDbConnection>();
            response.Success = true;
            response.Message = "Success";
            _tenantDbConnection = new Mock<ITenantDbConnection>();
            _tenantDbConnection.Setup(p => p.GetTenantDbConnection(It.IsAny<string>(), It.IsAny<bool>(), default)).ReturnsAsync(response);
            _tenantConfigHelper = new Mock<ITenantConfigHelper>();
            _tenantConfigHelper.Setup(p => p.GetTenantConfigValue(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(_tenantConfig);
            _coreService = new CoreService(_tenantDbConnection.Object, _tenantConfigHelper.Object);
        }

        [Fact]
        public async Task TestGetConfig()
        {
            TenantConfig result = await _coreService.GetConfig("ConfigName", 9);
            Assert.Equal("ServiceBusQueueName", result.ConfigName);
            Assert.Equal("QueueName", result.Value);
        }

        [Fact]
        public async Task TestGetDBConnection()
        {
            Response<IDbConnection> result  = await _coreService.GetDBConnection(9);
            Assert.True(result.Success);
            Assert.Equal("Success", result.Message);
        }
    }
}
