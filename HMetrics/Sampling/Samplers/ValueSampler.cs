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
    public class ValueSampler<T> : NamedSampler
    {
        private Histogram<T> _histogram;

        public ValueSampler(string name) : base(name) { }

        public List<Sample<T>> GetAllSamples(bool reset)
        {
            return _histogram.GetAllSamples(reset);
        }

        public void Reset()
        {
            _histogram.Reset();
        }

        public void Sample(T value)
        {
            _histogram.Sample(value);
        }

        public void Sample(T value, DateTime timeStamp)
        {
            _histogram.Sample(value, timeStamp);
        }

        public override ReportEntry AsReportEntry(Stack<string> contextStack, bool reset)
        {
            ReportEntry result = new ReportEntry();
            result.ContextStack = contextStack;
            foreach (Sample<T> sample in _histogram.GetAllSamples(reset))
            {
                result.JsonSamples.Add(sample.ToJson());
            }
            return result;
        }
    }
}
