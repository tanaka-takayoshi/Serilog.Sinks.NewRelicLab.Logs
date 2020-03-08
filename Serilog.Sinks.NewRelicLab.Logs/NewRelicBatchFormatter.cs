using Serilog.Debugging;
using Serilog.Sinks.Http.BatchFormatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Serilog.Sinks.NewRelicLab.Logs
{
    internal class NewRelicBatchFormatter : BatchFormatter
    {
        internal NewRelicBatchFormatter(long? eventBodyLimitBytes = 256 * 1024) : base(eventBodyLimitBytes)
        {
        }

        public override void Format(IEnumerable<string> logEvents, TextWriter output)
        {
            if (logEvents == null) throw new ArgumentNullException(nameof(logEvents));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (!logEvents.Any())
            {
                SelfLog.WriteLine("log event is empty.");
                return;
            }

            output.Write("[{\"logs\": [");

            var delimStart = string.Empty;

            foreach (var logEvent in logEvents)
            {
                if (string.IsNullOrWhiteSpace(logEvent))
                {
                    continue;
                }

                if (CheckEventBodySize(logEvent))
                {
                    output.Write(delimStart);
                    output.Write(logEvent);
                    delimStart = ",";
                }
            }

            output.Write("]}]");
        }
    }
}
