using HMetrics.Metrics;
using HMetrics.Reporting;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace HMetrics.Sampling.Samplers
{
    public class TimeSampler : NamedSampler
    {
        internal Histogram<double> Histogram = new Histogram<double>();

        public TimeSampler(string name) : base(name) { }

        internal void Sample(double value)
        {
            Histogram.Sample(value);
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
            foreach (Sample<double> sample in Histogram.GetAllSamples(reset))
            {
                sample.SetTags(this.Tags);
                result.JsonSamples.Add(sample.ToJson());
            }
            return result;
        }
    }
}
