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
    public class CheckClientIdAsyncTest : CommonHelper
    {
        [Fact]
        public async Task CheckClientIdAsyncTest_ExistingClientId_Success()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            var _result = await _databaseService.CheckClientIdAsync(1, _dbConnection);

            Assert.Equal(1, _result);
        }

        [Fact]
        public async Task CheckClientIdAsyncTest_NotExistClientId_Fail()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            var _result = await _databaseService.CheckClientIdAsync(-1, _dbConnection);

            Assert.Equal(0, _result);
        }
    }
}
