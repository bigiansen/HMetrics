using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Reporting.ElasticSearch
{
    public class ESReporterConfig
    {
        public string BaseIndex = null;
        public bool UseBaseIndex = true;

        public string Host = "0.0.0.0";
        public ushort Port = 0;

        public int ReportingIntervalMs = -1;

        public bool AutoResetMetrics = true;

        public bool AppendSamplerNameToIndex = true;
    }
}
