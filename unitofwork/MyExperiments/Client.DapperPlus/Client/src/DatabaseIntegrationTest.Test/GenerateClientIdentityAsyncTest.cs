using Domain.Entities;
using Domain.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace RespositoryTest.Test
{
    public class GenerateClientIdentityAsyncTest : CommonHelper
    {
        [Fact]
        public async Task GenerateClientIdentityAsyncTest_HappyPath_Success()
        {
            int TenantId = 9;
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            string ClientCode = await _databaseService.GenerateClientIdentityAsync(TenantId, _dbConnection);
        }
    }
}
