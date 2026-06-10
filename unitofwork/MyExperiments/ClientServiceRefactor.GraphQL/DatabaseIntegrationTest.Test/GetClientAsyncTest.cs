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
    public class GetClientAsyncTest : CommonHelper
    {
        [Fact]
        public async Task GetClientAsyncTest_SearchByClientId_Success()
        {
            //GetClientListModel request = new GetClientListModel { 
            //    ClientId = 1
            //};
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context { 
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<int, List<ClientAddress>> s = await _databaseService.GetClientAsync(request, _dbConnection);

            //Assert.Equal(1, s.Item1);
        }

        [Fact]
        public async Task GetClientAsyncTest_SearchBySearchingKey_Success()
        {
            //GetClientListModel request = new GetClientListModel
            //{
            //    SearchKeyWord = "0000",
            //    PageNumber = 1,
            //    RowCount = 10
            //};
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<int, List<ClientAddress>> s = await _databaseService.GetClientAsync(request, _dbConnection);
        }

        [Fact]
        public async Task GetClientAsyncTest_SearchBySearchingKey_Failed()
        {
            //GetClientListModel request = new GetClientListModel
            //{
            //    SearchKeyWord = "AAAA",
            //    PageNumber = 1,
            //    RowCount = 10
            //};
            //IDbConnection _dbConnection = GetDbConnection();
            //Context context = new Context
            //{
            //    Connection = _dbConnection
            //};
            //IClientUnitOfWork _unit = new UnitOfWork(context);
            //IClientDBService _databaseService = new ClientDBService(_unit);
            //Tuple<int, List<ClientAddress>> s = await _databaseService.GetClientAsync(request, _dbConnection);
        }
    }
}