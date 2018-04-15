using HMetrics.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Reporting
{
    public abstract class BaseReporter
    {
        internal bool AutoResetMetrics { get; set; }

        public BaseReporter(bool autoResetMetrics)
        {
            AutoResetMetrics = autoResetMetrics;
        }

        public List<ReportEntry> GetReport()
        {
            List<ReportEntry> entries = new List<ReportEntry>();
            var lvl1 = HMetricsContextManager.GetAllLvl1Contexts();
            foreach(HMetricsContext ctx in lvl1)
            {
                entries.AddRange(ctx.Report(AutoResetMetrics, HMetricsContext.ReportMode.IncludeChildren));
            }
            return entries;
        }

        internal abstract void SendReport();
    }
}
