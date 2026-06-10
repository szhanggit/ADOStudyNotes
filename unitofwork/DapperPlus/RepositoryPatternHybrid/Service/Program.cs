using Microsoft.Data.SqlClient;
using RepositoryDapper;
using Service.Services;
using TXC.Common.RepositoryCore;
using static RepositoryDapper.Repositories.OrderDetailRepo;
using static RepositoryDapper.Repositories.OrderRepo;
using static RepositoryDapper.Repositories.ProductRepo;
using static RepositoryDapper.UnitOfWork.SalesUnit;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddScoped(d => new Context() { Connection = new SqlConnection() });
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesUnitOfWork, SalesUnitOfWork>();


var app = builder.Build();

//// Configure the HTTP request pipeline.
app.MapGrpcService<OrderService>();
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcReflectionService();

//app.UseRouting();
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<OrderService>();
    endpoints.MapGrpcService<ProductService>();
    endpoints.MapGrpcReflectionService();
});*/

app.Run();
