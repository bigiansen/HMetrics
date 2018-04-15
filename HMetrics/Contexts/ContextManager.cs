using HMetrics.Contexts;
using HMetrics.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics
{
    public static class HMetricsContextManager
    {
        private static Dictionary<string, HMetricsContext> _contexts = new Dictionary<string, HMetricsContext>();

        public static HMetricsContext GetContext(string name)
        {
            if (_contexts.ContainsKey(name))
            {
                return _contexts[name];
            }
            else
            {
                HMetricsContext ctx = new HMetricsContext(name);
                _contexts.Add(name, ctx);
                return ctx;
            }
        }

        internal static List<HMetricsContext> GetAllLvl1Contexts()
        {
            return _contexts.Values.ToList();
        }

        internal static List<ReportEntry> Report(bool resetMetrics)
        {
            List<ReportEntry> result = new List<ReportEntry>();
            var contexts = GetAllLvl1Contexts();
            foreach (HMetricsContext ctx in contexts)
            {
                result.AddRange(ctx.Report(resetMetrics, HMetricsContext.ReportMode.IncludeChildren));
            }
            return result;
        }
    }
}