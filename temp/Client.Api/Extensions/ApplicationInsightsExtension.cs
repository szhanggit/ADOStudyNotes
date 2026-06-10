using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Client.Api.Logging;
using Services.Utility.Telemetry;
using TXC.Common.Logging.AppInsights;

namespace Client.Api.Extensions
{
    public static class ApplicationInsightsExtension
    {
        public static IServiceCollection ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));
            services.AddScoped(typeof(ITelemetryLogTrace<>), typeof(TelemetryLogTrace<>));
            services.AddScoped(typeof(ITelemetryLogRequest<>), typeof(TelemetryLogRequest<>));
            services.AddSingleton<ITelemetryInitializer, ClientTelemetryInitializer>();

            services.AddApplicationInsightsTelemetryProcessor<AppInsightsHealthCheckFilter>();

            return services;
        }
    }
}
