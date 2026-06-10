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
    public class DeleteClientByIdAsyncTest : CommonHelper
    {
        [Fact]
        public async Task DeleteClientByIdAsyncTest_ClientIdWithAddress_Success()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            await _databaseService.DeleteClientByIdAsync(30, _dbConnection);
        }

        [Fact]
        public async Task DeleteClientByIdAsyncTest_ClientIdWithoutAddress_Success()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            await _databaseService.DeleteClientByIdAsync(23, _dbConnection);
        }

        [Fact]
        public async Task DeleteClientByIdAsyncTest_NotExistClientId_Fail()
        {
            IDbConnection _dbConnection = GetDbConnection();
            Context context = new Context
            {
                Connection = _dbConnection
            };
            IClientUnitOfWork _unit = new UnitOfWork(context);
            IClientDBService _databaseService = new ClientDBService(_unit);
            await _databaseService.DeleteClientByIdAsync(23000, _dbConnection);
        }
    }
}
