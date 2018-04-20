using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Reporting.ElasticSearch
{
    public class ESReport
    {
        public List<string> JsonLines = new List<string>();

        public ESReport() { }

        public static ESReport FromReportEntries(IEnumerable<ReportEntry> entries, ESReporterConfig config)
        {
            ESReport report = new ESReport();
            report.GenerateJsonFromEntries(entries, config);
            return report;
        }

        private void GenerateJsonFromEntries(IEnumerable<ReportEntry> entries, ESReporterConfig config)
        {
            foreach (ReportEntry entry in entries)
            {
                foreach(string sample in entry.JsonSamples)
                {
                    JsonLines.Add(GetJsonHeaderLine(entry, config));
                    JsonLines.Add(sample);
                }
            }
        }

        private string GetJsonHeaderLine(ReportEntry entry, ESReporterConfig config)
        {
            string contextIdx = string.Join(".", entry.ContextStack);
            if (config.AppendSamplerNameToIndex)
            {
                contextIdx += $".{entry.SamplerName}";
            }
            if (config.UseBaseIndex)
            {
                return "{ \"index\" : { \"_index\" : \"" + config.BaseIndex + '.' + contextIdx + "\", \"_type\" : \"json\"}}";
            }
            else
            {
                return "{ \"index\" : { \"_index\" : \"" + contextIdx + "\", \"_type\" : \"json\"}}";
            }
        }

    }
}
