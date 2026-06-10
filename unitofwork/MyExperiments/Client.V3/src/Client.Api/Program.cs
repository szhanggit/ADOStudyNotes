using Client.Api.Extensions;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Repository;
using ServiceClient.Api.Extensions;
using Services.Core;
using Services.gRPCServices;
using System;
using System.Collections.Generic;
using TXC.Common.CacheManagement;
using TXC.Common.Logging.ErrorHandler;
using TXC.Common.MessageContract;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var currentEnvironment = builder.Environment;
const string CORS_NAME = "client_service_CORSE_NAME";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CORS_NAME, builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});
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
builder.Services.AddScoped<ICreateClientService, CreateClientService>();
builder.Services.AddScoped<ICreateBXPClientService, CreateBXPClientService>();
builder.Services.AddScoped<IUpdateClientService, UpdateClientService>();
builder.Services.AddScoped<IGetClientListService, GetClientListService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICoreService, CoreService>();
builder.Services.AddScoped<ICommonClientService, CommonClientService>();
builder.Services.AddScoped<IObjectConvertingService, ObjectConvertingService>();

builder.Services.AddSwaggerGen(c =>
{
    var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization do not add 'BEARER' ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
});
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

// add secrets from CSI environment variables
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.UseKestrel(options =>
{
    // for gRPC
    options.ListenAnyIP(9005, o => o.Protocols = HttpProtocols.Http2);
});
var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    // local development swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client.Api v1");
        });
    }
    else
    {
        // ingress special configuration swagger
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Servers = new System.Collections.Generic.List<OpenApiServer>
                        {
                        new OpenApiServer { Url = $"https://{httpReq.Host.Value}/client" }
                        });
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "Client.Api v1");
        });
    }
}


app.UseRouting();
app.UseCors(CORS_NAME);

// global error handler
app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<ClientService>();
    endpoints.MapGrpcReflectionService();
    endpoints.MapControllers();
});


app.Run();
