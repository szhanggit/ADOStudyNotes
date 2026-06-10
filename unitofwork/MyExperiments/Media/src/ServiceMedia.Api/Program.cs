
using Api.Extensions;
using Infrastructure.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServiceMedia.Api.Extensions;
using Services.CDN;
using Services.GrpcService;
using System;
using TXC.Common.CacheManagement;
using TXC.Common.Logging.ErrorHandler;
using TXC.Common.MessageContract;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
const string CORS_NAME = "media_service_CORSE_NAME";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CORS_NAME, builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});

builder.Services.AddRepository();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataOperation(configuration);
builder.Services.AddPipe();
builder.Services.AddServiceBus(configuration);
builder.Services.AddMediatR();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddFluentValidation(configuration);
//builder.Services.AddJwtAuth(Configuration);
builder.Services.AddSwagger(configuration);
builder.Services.AddKeyVault(configuration);
builder.Services.AddAzureStorage(configuration);
builder.Services.AddStoragePath(configuration);
builder.Services.AddCdn(configuration);
builder.Services.AddApplicationInsight(configuration);
builder.Services.AddGraphQLExtService(configuration);
builder.Services.AddFilter();
builder.Services.AddCached(configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSingleton<ITenantConfigHelper, TenantConfigHelper>();
builder.Services.AddSingleton<ICdnHelper, CdnHelper>();
builder.Services.AddSingleton<ITX2ServiceBusSender, TX2ServiceBusSender>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddCoreService(configuration);

Console.WriteLine("Starting..." +builder.Environment.EnvironmentName);

// add secrets from CSI environment variables
builder.Configuration.AddEnvironmentVariables();

//Configure Web Builder            
builder.WebHost.UseKestrel(options =>
    {
        // for REST
        options.ListenAnyIP(8001, o => o.Protocols =
            HttpProtocols.Http1AndHttp2);

        // for gRPC
        options.ListenAnyIP(9001, o => o.Protocols =
            HttpProtocols.Http2);

    });

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
}

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    // local development swagger
    if (app.Environment.IsDevelopment())
    {
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceMedia.Api v1");
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
                        new OpenApiServer { Url = $"https://{httpReq.Host.Value}/media" }
                        });
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "ServiceMedia.Api v1");
        });
    }
}

app.UseRouting();

app.UseCors(CORS_NAME);

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<MediaService>();
    endpoints.MapControllers();

    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

});

app.MapGrpcReflectionService();

app.Run();