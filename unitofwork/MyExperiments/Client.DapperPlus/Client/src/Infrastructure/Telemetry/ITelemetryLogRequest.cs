using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Telemetry
{
    public interface ITelemetryLogRequest<TCategoryName>
    {
        public void LogRequest<T>(string name, string key, T data);
        public void LogRequest<T>(string name, string key, T data, IDictionary<string, string> otherProperties);
    }
}
