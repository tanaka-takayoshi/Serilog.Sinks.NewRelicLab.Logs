using Serilog.Debugging;
using Serilog.Sinks.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Serilog.Sinks.NewRelicLab.Logs
{
    public sealed class NewRelicHttpClient : IHttpClient
    {
        private readonly HttpClient client;
        private readonly bool isKeyConfigured = false;

        public NewRelicHttpClient()
        {
            client = new HttpClient();
            var licenseKey = Environment.GetEnvironmentVariable("NEW_RELIC_LICENSE_KEY");
            if (!string.IsNullOrEmpty(licenseKey))
            {
                isKeyConfigured = true;
                SelfLog.WriteLine("X-License-Key is configured by NEW_RELIC_LICENSE_KEY environment variable.");
                client.DefaultRequestHeaders.Add("X-License-Key", licenseKey);
            }
            else
            {
                var insertKey = Environment.GetEnvironmentVariable("NEW_RELIC_INSERT_KEY");
                if (!string.IsNullOrEmpty(insertKey))
                {
                    isKeyConfigured = true;
                    SelfLog.WriteLine("X-Insert-Key is configured by NEW_RELIC_INSERT_KEY environment variable.");
                    client.DefaultRequestHeaders.Add("X-Insert-Key", insertKey);
                }
                else
                {
                    SelfLog.WriteLine("No key is configured for New Relic Logs API.");
                }
            }

        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            if (!isKeyConfigured)
            {
                SelfLog.WriteLine("No key is configured for New Relic Logs API.");
                return Task.FromResult(new HttpResponseMessage());
            }
            else
            {
                return client.PostAsync(requestUri, content);
            }
        }

        public void Dispose() => client.Dispose();
    }
}
