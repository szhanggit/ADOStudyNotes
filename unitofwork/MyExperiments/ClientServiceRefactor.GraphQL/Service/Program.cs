using Microsoft.AspNetCore.Mvc;
using Repository;
using Service.BusinessLogic;
using Service.Extensions;
using Service.gRPCController;
using Service.Mapper;
using Service.Utility;
using Service.Utility.GraphQLClient;
using System.Data.SqlClient;
using TXC.Common.CacheManagement;
using TXC.Common.MessageContract;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var currentEnvironment = builder.Environment;
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureDataOperations(configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddFluentValidation(configuration);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddGrpcRegistrations(configuration);

// register gRPC services
builder.Services.AddTransient<ICreateClientService, CreateClientService>();
builder.Services.AddTransient<IUpdateClientService, UpdateClientService>();
builder.Services.AddTransient<IGetClientListService, GetClientListService>();
builder.Services.AddTransient<ICoreService, CoreService>();
builder.Services.AddTransient<ICommonClientService, CommonClientService>();
builder.Services.AddTransient<IObjectConvertingService, ObjectConvertingService>();
builder.Services.AddTransient<IClientDBService, ClientDBService>();
builder.Services.AddTransient<IClientUnitOfWork, UnitOfWork>();
builder.Services.AddTransient(d => new Context() { Connection = new SqlConnection() });

builder.Services.ConfigureKeyVault(configuration);
builder.Services.ConfigureAzureStorage(configuration);
builder.Services.ConfigureDirectoryConfig(configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCached(configuration);
builder.Services.AddSingleton<ITX2ServiceBusSender, TX2ServiceBusSender>();
builder.Services.AddSingleton<ITenantConfigHelper, TenantConfigHelper>();
builder.Services.AddTransient<IClientFetchingGraphQLService, ClientFetchingGraphQLService>();
builder.Services.AddTransient<IClientGraphQLClient, ClientGraphQLClient>();
builder.Services.AddTransient<IGeneralGraphQLClient, GeneralGraphQLClient>();
builder.Services.AddTransient<IGetDictionaryListGraphQLService, GetDictionaryListGraphQLService>();
builder.Services.AddTransient<IGraphQLGatewayClient, GraphQLGatewayClient>();
builder.Services.AddTransient<IClientAddressFetchingGraphQLService, ClientAddressFetchingGraphQLService>();
builder.Services.AddTransient<ISecurityKeyService, SecurityKeyService>();
builder.Services.AddTransient<IDammAlgorithm, DammAlgorithm>();
builder.Services.AddTransient<IClientHelperService, ClientHelperService>();

Console.WriteLine("Starting..." + builder.Environment.EnvironmentName);

builder.Configuration.AddEnvironmentVariables();






var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ClientService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
