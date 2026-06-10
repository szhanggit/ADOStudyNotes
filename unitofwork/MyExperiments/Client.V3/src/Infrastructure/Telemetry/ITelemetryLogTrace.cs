using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Telemetry
{
    public interface ITelemetryLogTrace<TCategoryName>
    {
        public void LogTrace<T>(string message, string key, T data, SeverityLevel severityLevel);
        public void LogTrace(string message, SeverityLevel severityLevel);
        public void LogTrace<T>(string message, string key, T data, SeverityLevel severityLevel, IDictionary<string, string> otherProperties);
        public void LogTrace<T>(string message, SeverityLevel severityLevel, IDictionary<string, string> otherProperties);

    }
}
