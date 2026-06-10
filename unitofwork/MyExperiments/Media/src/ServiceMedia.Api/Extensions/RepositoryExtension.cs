using Microsoft.Extensions.DependencyInjection;
using Repository;
using System.Data.SqlClient;
using TXC.Common.RepositoryCore;
using static Repository.MediaUnit;
using static Repository.Repositories.MediaRepo;

namespace ServiceMedia.Api.Extensions
{
    public static  class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped(d=> new Context() { Connection = new SqlConnection()});
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMediaRepository,MediaRepository>();
            services.AddScoped<IMediaUnitOfWork, MediaUnitOfWork>();
            return services;
        }
    }
}
