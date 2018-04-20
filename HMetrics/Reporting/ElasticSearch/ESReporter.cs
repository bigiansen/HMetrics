using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMetrics.Reporting.ElasticSearch
{
    public class ESReporter : BaseReporter
    {
        internal ESReporterConfig Configuration { get; set; }
        internal HttpClient WebClient { get; set; }
        internal string BulkUrl { get; set; }

        public ESReporter(ESReporterConfig config) : base(config.AutoResetMetrics)
        {
            this.Configuration = config;
            WebClient = new HttpClient();
            BulkUrl = $"http://{Configuration.Host}:{Configuration.Port}/_bulk";
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while(true)
                {
                    Thread.Sleep(Configuration.ReportingIntervalMs);
                    SendReport();
                }
            });
        }

        internal override void SendReport()
        {
            ESReport report;
            using (var scope = JsConfig.BeginScope())
            {
                scope.DateHandler = DateHandler.ISO8601;
                report = ESReport.FromReportEntries(GetReport(), Configuration);
            }

            string fullJson = string.Join("\n", report.JsonLines) + '\n';
            if (string.IsNullOrWhiteSpace(fullJson))
            {
                return;
            }

            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            try
            {
                var response = wc.UploadString(BulkUrl, fullJson);
                Debug.WriteLine(response);
            }
            catch(WebException webex)
            {
                Debug.WriteLine(webex.Message);
                Console.Out.WriteLine(webex.Message);
            }            
        }
    }
}
