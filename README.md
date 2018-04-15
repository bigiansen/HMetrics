## HMetrics

Barebones metrics library. A simple interface for creating and reporting metrics.
Includes an ElasticSearch Reporter.

Creating a metric:
```cs
var ctx = HMetricsContextManager.GetContext("test");
var timer = ctx.Timer("timer")
```
  
Using the ElasticSearch reporter:
```cs
ESReporterConfig config = new ESReporterConfig()
{
    AutoResetMetrics = true,
    BaseIndex = "metrics",
    Host = "192.168.1.111",
    Port = 9200,
    ReportingIntervalMs = 2000,
    UseBaseIndex = true
};
ESReporter reporter = new ESReporter(config);
reporter.Start();
```
