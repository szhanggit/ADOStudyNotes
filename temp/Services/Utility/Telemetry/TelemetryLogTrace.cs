using Domain.Constants;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Services.Utility.Telemetry
{
    public class TelemetryLogTrace<TCategoryName> : ITelemetryLogTrace<TCategoryName>
    {
        private readonly TelemetryClient _telemetryClient;
        public TelemetryLogTrace(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void LogTrace<T>(string message ,string key, T data, SeverityLevel severityLevel)
        {
            try
            {
                string serializeData = JsonSerializer.Serialize(data);
                IDictionary<string, string> customProperties = new Dictionary<string, string>()
                {          
                    { TelemetryCustomproperty.CategoryName, typeof(TCategoryName).FullName},
                    { key,serializeData}
                };

                _telemetryClient.TrackTrace(message, severityLevel, customProperties);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void LogTrace(string message, SeverityLevel severityLevel)
        {
            _telemetryClient.TrackTrace(message);
        }
        public void LogTrace<T>(string message, string key, T data, SeverityLevel severityLevel, IDictionary<string, string> otherProperties)
        {
            try
            {
                string serializeData = JsonSerializer.Serialize(data);
                Dictionary<string, string> customProperties = new Dictionary<string, string>();
                customProperties.Add(TelemetryCustomproperty.CategoryName, typeof(TCategoryName).FullName);
                customProperties.Add(key, serializeData);

                foreach (var item in otherProperties)
                {
                    customProperties.Add(item.Key,item.Value);
                }
                _telemetryClient.TrackTrace(message, severityLevel, customProperties);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void LogTrace<T>(string message, SeverityLevel severityLevel, IDictionary<string, string> otherProperties)
        {
            try
            {
                Dictionary<string, string> customProperties = new Dictionary<string, string>();
                customProperties.Add(TelemetryCustomproperty.CategoryName, typeof(TCategoryName).FullName);
                foreach (var item in otherProperties)
                {
                    customProperties.Add(item.Key, item.Value);
                }
                _telemetryClient.TrackTrace(message, severityLevel, customProperties);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
