using Service.Protos;


public static class GrpcClients
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration config)
    {
        string endpoint = config["GrpcEndPoint"].ToString();
        services.AddGrpcClient<Products.ProductsClient>(o=> o.Address = new Uri(endpoint));
        services.AddGrpcClient<Orders.OrdersClient>(o => o.Address = new Uri(endpoint));
        return services;
    }
}

