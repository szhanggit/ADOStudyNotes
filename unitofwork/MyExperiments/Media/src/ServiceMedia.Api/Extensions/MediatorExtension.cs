using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Services.Queries.User;

namespace ServiceMedia.Api.Extensions
{
    public static class MediatorExtension
    {
       public static void AddMediatR(this IServiceCollection services)
       {

            services.AddMediatR(typeof(GetAllSampleQuery).Assembly);
       }
    }
}
