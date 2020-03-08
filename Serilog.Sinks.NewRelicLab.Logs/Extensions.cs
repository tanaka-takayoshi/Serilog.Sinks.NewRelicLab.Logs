using NewRelic.LogEnrichers.Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sinks.NewRelicLab.Logs
{
    public static class Extensions
    {
        private static readonly string endpoint_us_v1 = "https://log-api.newrelic.com/log/v1";
        private static readonly string endpoint_eu_v1 = "https://log-api.eu.newrelic.com/log/v1";

        public static LoggerConfiguration NewRelicLogs(this LoggerSinkConfiguration config, Endpoint endpoint = Endpoint.US_V1)
        {
            var url = endpoint switch
            {
                Endpoint.US_V1 => endpoint_us_v1,
                Endpoint.EU_V1 => endpoint_eu_v1,
                _ => throw new ArgumentOutOfRangeException(nameof(endpoint))
            };
            return config.NewRelicLogs(url);
        }

        public static LoggerConfiguration NewRelicLogs(this LoggerSinkConfiguration config, string url)
            => config.Http(url, textFormatter: new NewRelicFormatter(), batchFormatter: new NewRelicBatchFormatter(), httpClient: new NewRelicHttpClient());
    }

    public enum Endpoint
    {
        US_V1,
        EU_V1
    }
}
