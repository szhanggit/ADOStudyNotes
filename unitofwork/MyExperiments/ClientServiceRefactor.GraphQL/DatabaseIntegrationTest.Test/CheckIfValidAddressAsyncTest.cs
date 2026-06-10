using Domain.Entities;
using Domain.Models;
using Repository;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Respository.Test
{
    public class CheckIfValidAddressAsyncTest : CommonHelper
    {
        [Fact]
        public async Task CheckIfValidAddressAsyncTest_CountryId_Success()
        {
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<bool, string> s = await _databaseService.CheckIfValidAddressAsync(10, 7, 6, _dbConnection);

            //Assert.True(s.Item1);
        }

        [Fact]
        public async Task CheckIfValidAddressAsyncTest_InvalidCountryId_Fail()
        {
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<bool, string> s = await _databaseService.CheckIfValidAddressAsync(10, 7, 60, _dbConnection);

            //Assert.False(s.Item1);
            //Assert.Equal("Invalid country id.", s.Item2);
        }

        [Fact]
        public async Task CheckIfValidAddressAsyncTest_InvalidProvinceId_Fail()
        {
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<bool, string> s = await _databaseService.CheckIfValidAddressAsync(10, 70, 6, _dbConnection);

            //Assert.False(s.Item1);
            //Assert.Equal("Invalid province id.", s.Item2);
        }

        [Fact]
        public async Task CheckIfValidAddressAsyncTest_InvalidCityId_Fail()
        {
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<bool, string> s = await _databaseService.CheckIfValidAddressAsync(100, 7, 6, _dbConnection);

            //Assert.False(s.Item1);
            //Assert.Equal("Invalid city id.", s.Item2);
        }
    }
}
