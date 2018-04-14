using HMetrics.Metrics;
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
    }
}
