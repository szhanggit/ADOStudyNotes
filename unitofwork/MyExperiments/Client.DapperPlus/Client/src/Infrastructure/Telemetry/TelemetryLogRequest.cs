using Domain.Constants;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Telemetry
{
    public class TelemetryLogRequest<TCategoryName> : ITelemetryLogRequest<TCategoryName>
    {
        private readonly TelemetryClient _telemetry;

        public TelemetryLogRequest(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        public void LogRequest<T>(string name, string key, T data)
        {
            try
            {
                string serializeData = JsonSerializer.Serialize(data);
                RequestTelemetry requestTelemetry = new RequestTelemetry();
                requestTelemetry.Name = name;
                requestTelemetry.Properties.Add(TelemetryCustomproperty.CategoryName, typeof(TCategoryName).FullName);
                requestTelemetry.Properties.Add(key, serializeData);
                _telemetry.TrackRequest(requestTelemetry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LogRequest<T>(string name, string key, T data, IDictionary<string, string> otherProperties)
        {
            try
            {
                string serializeData = JsonSerializer.Serialize(data);
                RequestTelemetry requestTelemetry = new RequestTelemetry();
                requestTelemetry.Name = name;
                requestTelemetry.Properties.Add(TelemetryCustomproperty.CategoryName, typeof(TCategoryName).FullName);
                requestTelemetry.Properties.Add(key, serializeData);

                foreach (var item in otherProperties)
                {
                    requestTelemetry.Properties.Add(item.Key, item.Value);
                }

                _telemetry.TrackRequest(requestTelemetry);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
