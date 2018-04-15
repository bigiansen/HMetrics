using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Reporting.ElasticSearch
{
    public class ESReporterConfig
    {
        public string BaseIndex { get; set; }
        public bool UseBaseIndex { get; set; }

        public string Host { get; set; }
        public ushort Port { get; set; }

        public int ReportingIntervalMs { get; set; }

        public bool AutoResetMetrics { get; set; }
    }
}
