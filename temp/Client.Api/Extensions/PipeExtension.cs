using Infrastructure.Pipes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.Extensions
{
    public static class PipeExtension
    {
        public static void ConfigurePipe(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>),typeof(UserIdPipe<,>));
        }
    }
}
