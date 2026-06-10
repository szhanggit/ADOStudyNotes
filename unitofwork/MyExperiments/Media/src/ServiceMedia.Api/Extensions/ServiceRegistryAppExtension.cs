using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ServiceMedia.Api.Extensions
{
    public static class ServiceRegistryAppExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(c => new ConsulClient(cfg =>
            {
                cfg.Address = new Uri(configuration.GetSection("Configuration:ConsulAddress").Value);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            //var host = Dns.GetHostAddresses(Dns.GetHostName())
            //    .FirstOrDefault(ha => ha.AddressFamily == AddressFamily.InterNetwork)
            //    .ToString();
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var serviceName = configuration.GetSection("Configuration:ServiceName").Value;
            var host = configuration.GetSection("Configuration:ServiceHost").Value;
            var port = Convert.ToInt32(configuration.GetSection("Configuration:ServicePort").Value);

            var tcpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                TCP = $"{host}:{port}"
            };

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"http://{host}:{port}/HealthCheck",
                Method = "GET"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { tcpCheck, httpCheck },
                ID = serviceName,
                Name = serviceName,
                Address = host,
                Port = port
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true).GetAwaiter().GetResult();
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true).GetAwaiter().GetResult();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
            });

            return app;
        }
    }
}
