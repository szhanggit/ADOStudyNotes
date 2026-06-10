using Infrastructure.Pipes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceMedia.Api.Extensions
{
    public static class PipeExtension
    {
        public static void AddPipe(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>),typeof(UserIdPipe<,>));
        }
    }
}
