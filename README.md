# Serilog.Sinks.NewRelicLab.Logs
A serilog sink that sends logs to New Relic Logs

Note: This is an unofficial package. Since this is an experimental library, please consider using more robust log shipping method (e.g. fluentd).

## Requirements

- New Relic Logs subscription ([License Key](https://docs.newrelic.com/docs/accounts/install-new-relic/account-setup/license-key) or [Insert API key](https://docs.newrelic.com/docs/apis/get-started/intro-apis/types-new-relic-api-keys#event-insert-key) is required)
- New Relic APM subscription if you'd like to enable Logs in Context
- Serilog 2.5.0 or above (Serilog will be installed as one of dependencies)

## Usage

1. Add [Serilog.Sinks.NewRelicLab.Logs](https://www.nuget.org/packages/Serilog.Sinks.NewRelicLab.Logs) package.

2. Use [NewRelicLogs()](https://github.com/tanaka-takayoshi/Serilog.Sinks.NewRelicLab.Logs/blob/master/Serilog.Sinks.NewRelicLab.Logs/Extensions.cs#L14) extentione method to configure. Here is [an example code](https://github.com/tanaka-takayoshi/Serilog.Sinks.NewRelicLab.Logs/tree/master/examples/WebApp) using with ASP.NET Core.

  ```cs 
  using NewRelic.LogEnrichers.Serilog; //This using is required for `WithNewRelicLogsInContext` method.
  using Serilog.Sinks.NewRelicLab.Logs;
  ```
  
  ```cs
  Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .Enrich.WithNewRelicLogsInContext()
  .WriteTo.NewRelicLogs()
  .CreateLogger();
  ```
  
3. Add one of the following ENVIRONMENT VARIABLE to start the process: `NEW_RELIC_LICENSE_KEY` is for a New Relic License Key, or `NEW_RELIC_INSERT_KEY` is for an Insert API key.
  
4. Output your log with Serilog.

  ```cs
  Serilog.Log.Information("Web Host launched!");
  ```
  
5. You will see your log in New Relic Logs.

## Troubleshooting

Enabling Selflog in Serilof will help you what's happing.

```cs
var file = File.CreateText(@"./selflog.txt");
Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
```
