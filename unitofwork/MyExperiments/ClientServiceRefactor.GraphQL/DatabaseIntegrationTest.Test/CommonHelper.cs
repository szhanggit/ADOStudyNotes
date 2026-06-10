using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC.Common.CacheManagement.Resolver;
using TXC.Common.Data;
using TXC.Common.MessageContract;

namespace Respository.Test
{
    public class CommonHelper
    {
        protected IDbConnection GetDbConnection()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string _connectionString = config["ConnectionString"];
            IDbConnection _dbConnection = new SqlConnection(_connectionString);
            return _dbConnection;
        }

        protected IDbConnection InitDbConnection<T>(out T _service) where T : new()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string _connectionString = config.GetConnectionString("ConnectionString");

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

        //protected CommonClientService InitCommonClientService()
        //{
        //    var config = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .Build();

        //    Startup startup = new Startup(config);
        //    ServiceCollection sc = new ServiceCollection();
        //    startup.ConfigureServices(sc);
        //    IServiceProvider serviceProvider = sc.BuildServiceProvider();

        //    ITxcCacheReadFactory _txcCacheReadFactory = new TxcCacheReadFactoryGrpc(serviceProvider);
        //    ITX2ServiceBusSender _txcServiceBusSender = new TX2ServiceBusSender(config);
        //    using var channel = GrpcChannel.ForAddress("http://localhost:9022");
        //    CreditRpc.CreditRpcClient client = new CreditRpc.CreditRpcClient(channel);

        //    CommonClientService _commonClientService = new CommonClientService(_txcServiceBusSender, client);
        //    return _commonClientService;
        //}

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
