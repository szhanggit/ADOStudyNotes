using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceMedia.Api.Logging;
using TXC.Common.Logging;
using TXC.Common.Logging.AppInsights;

namespace ServiceMedia.Api.Extensions
{
    public static class ApplicationInsightsExtension
    {
        public static IServiceCollection AddApplicationInsight(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));
            services.AddScoped(typeof(ITelemetryLogTrace<>), typeof(TelemetryLogTrace<>));
            services.AddSingleton<ITelemetryInitializer, MediaTelemetryInitializer>();

            services.AddApplicationInsightsTelemetryProcessor<AppInsightsHealthCheckFilter>();
            return services;
        }
    }
}
