using HMetrics.Metrics;
using HMetrics.Reporting;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class TimeSampler : NamedSampler
    {
        private Histogram<double> _histogram = new Histogram<double>();

        public TimeSampler(string name) : base(name) { }

        internal void Sample(double value)
        {
            _histogram.Sample(value);
        }

        public TimeSamplerContext NewContext()
        {
            TimeSamplerContext ctx = new TimeSamplerContext(this);
            ctx.Start();
            return ctx;
        }

        public override ReportEntry AsReportEntry(Stack<string> contextStack, bool reset)
        {
            ReportEntry result = new ReportEntry();
            result.ContextStack = contextStack;
            foreach (Sample<double> sample in _histogram.GetAllSamples(reset))
            {
                result.JsonSamples.Add(sample.ToJson());
            }
            return result;
        }
    }
}
