using AutoMapper;
using Grpc.Net.Client;
using Infrastructure.Mapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Core;
using SpecFlow.Test.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.CacheManagement;
using TXC.Common.CacheManagement.Interface;
using TXC.Common.CacheManagement.Operation;
using TXC.Common.CacheManagement.Resolver;
using TXC.Common.Data;
using TXC.Common.MessageContract;
using TXC.Proto.Credit;

namespace SpecFlow.Test.Steps
{
    public class CommonHelper
    {
        protected IDbConnection InitDbConnection<T>(out T _service) where T : new()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string environmentName = DataManager.GetData().Environment;
            string _connectionString = DataManager.GetData().connectionStrings[$"{environmentName.ToLower()}{DataManager.GetData().parameters["TenantBasicInfoId"]}"];

            Startup startup = new Startup(config);
            ServiceCollection sc = new ServiceCollection();
            startup.ConfigureServices(sc);
            IServiceProvider serviceProvider = sc.BuildServiceProvider();

            ITxcCacheReadFactory _txcCacheReadFactory = new TxcCacheReadFactoryGrpc(serviceProvider);
            IDapperOperation _dapperOperation = new DapperOperation();
            ITX2ServiceBusSender _txcServiceBusSender = new TX2ServiceBusSender(config);
            T objItem = new T();
            object[] args = new object[] { _dapperOperation, _txcServiceBusSender };
            objItem = (T)Activator.CreateInstance(typeof(T), args);
            IDbConnection _dbConnection = new SqlConnection(_connectionString);
            _service = objItem;
            return _dbConnection;
        }

        protected CommonClientService InitCommonClientService()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Startup startup = new Startup(config);
            ServiceCollection sc = new ServiceCollection();
            startup.ConfigureServices(sc);
            IServiceProvider serviceProvider = sc.BuildServiceProvider();

            ITxcCacheReadFactory _txcCacheReadFactory = new TxcCacheReadFactoryGrpc(serviceProvider);
            ITX2ServiceBusSender _txcServiceBusSender = new TX2ServiceBusSender(config);
            using var channel = GrpcChannel.ForAddress("http://localhost:9022");
            CreditRpc.CreditRpcClient client = new CreditRpc.CreditRpcClient(channel);

            CommonClientService _commonClientService = new CommonClientService(_txcServiceBusSender, client);
            return _commonClientService;
        }

        protected ITX2ServiceBusSender GetServiceBusSender()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            Startup startup = new Startup(config);
            ServiceCollection sc = new ServiceCollection();
            startup.ConfigureServices(sc);
            IServiceProvider serviceProvider = sc.BuildServiceProvider();

            ITxcCacheReadFactory _txcCacheReadFactory = new TxcCacheReadFactoryGrpc(serviceProvider);
            ITX2ServiceBusSender _txcServiceBusSender = new TX2ServiceBusSender(config);
            return _txcServiceBusSender;
        }

        protected CreditRpc.CreditRpcClient GetCreditRpcClient()
        {
            ITX2ServiceBusSender _txcServiceBusSender = GetServiceBusSender();
            using var channel = GrpcChannel.ForAddress("http://localhost:9022");
            CreditRpc.CreditRpcClient client = new CreditRpc.CreditRpcClient(channel);
            return client;
        }

        //protected IObjectConvertingService GetObjectConvertingService()
        //{
        //    MapperConfiguration mapperConfig = new MapperConfiguration(
        //    cfg =>
        //    {
        //        cfg.AddProfile(new AutoMapperProfile());
        //    });
        //    IMapper _mapper = new Mapper(mapperConfig);
        //    IObjectConvertingService ObjectConvertingService = new ObjectConvertingService(_mapper);
        //    return ObjectConvertingService;
        //}
    }
}
