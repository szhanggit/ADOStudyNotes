using System.Collections.Generic;

namespace Services.Utility.Telemetry
{
    public interface ITelemetryLogRequest<TCategoryName>
    {
        public void LogRequest<T>(string name, string key, T data);
        public void LogRequest<T>(string name, string key, T data, IDictionary<string, string> otherProperties);
    }
}
