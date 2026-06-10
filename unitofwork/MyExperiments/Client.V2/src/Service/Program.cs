using Microsoft.AspNetCore.Mvc;
using Service.Extensions;
using Service.gRPCController;
using Service.Mapper;
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
//builder.Services.AddScoped<ICreateClientService, CreateClientService>();
//builder.Services.AddScoped<ICreateBXPClientService, CreateBXPClientService>();
//builder.Services.AddScoped<IUpdateClientService, UpdateClientService>();
//builder.Services.AddScoped<IGetClientListService, GetClientListService>();
//builder.Services.AddScoped<IClientRepository, ClientRepository>();
//builder.Services.AddScoped<ICoreService, CoreService>();
//builder.Services.AddScoped<ICommonClientService, CommonClientService>();
//builder.Services.AddScoped<IObjectConvertingService, ObjectConvertingService>();

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

Console.WriteLine("Starting..." + builder.Environment.EnvironmentName);

builder.Configuration.AddEnvironmentVariables();






var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ClientService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
