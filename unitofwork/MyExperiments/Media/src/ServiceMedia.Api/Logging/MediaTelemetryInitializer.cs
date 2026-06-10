using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMedia.Api.Logging
{
    public class MediaTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IConfiguration _configuration;
        public MediaTelemetryInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Component.Version = _configuration["ApplicationInsights:AppVersion"];

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
                telemetry.Context.Cloud.RoleName = _configuration["ApplicationInsights:RoleName"];
        }
    }
}
