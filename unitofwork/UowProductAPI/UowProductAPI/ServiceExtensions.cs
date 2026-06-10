using UowProductAPI.Infrastructure;
using UowProductAPI.Interfaces;

namespace UowProductAPI
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddTransient<IProductRepository, ProductRepository>();
        }
    }
}
