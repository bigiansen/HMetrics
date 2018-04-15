using HMetrics.Reporting;
using HMetrics.Sampling.Samplers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Contexts
{
    public class HMetricsContext
    {
        public string Name { get; set; }

        private HMetricsContext _parent = null;
        private Dictionary<string, HMetricsContext> _children = null;

        internal List<NamedSampler> Samplers { get; set; } = new List<NamedSampler>();

        public IntegerAccumulatorSampler Meter(string name, int samplingTimeWindow)
        {
            var sampler = new IntegerAccumulatorSampler(name, samplingTimeWindow);
            Samplers.Add(sampler);
            return sampler;
        }

        public TimeSampler Timer(string name)
        {
            var sampler = new TimeSampler(name);
            Samplers.Add(sampler);
            return sampler;
        }

        public ValueSampler<T> Histogram<T>(string name)
        {
            var sampler = new ValueSampler<T>(name);
            Samplers.Add(sampler);
            return sampler;
        }

        public enum ReportMode
        {
            Standard, IncludeChildren
        }        

        public HMetricsContext(string name)
        {
            Name = name;
        }

        internal string GetContextName(string contextSeparator = ".")
        {
            if(_parent == null)
            {
                return Name;
            }
            else
            {
                return _parent.GetContextName(contextSeparator) + contextSeparator + Name;
            }
        }

        internal Stack<string> GetContextStack()
        {
            List<string> result = new List<string>();
            result.Add(this.Name);

            HMetricsContext currentCtx = this;
            while(currentCtx.GetParent() != null)
            {
                currentCtx = currentCtx.GetParent();
                result.Add(currentCtx.Name);
            }
            result.Reverse();
            return new Stack<string>(result);            
        }

        public HMetricsContext GetParent()
        {
            return _parent;
        }

        public HMetricsContext GetChild(string name)
        {
            if(_children.ContainsKey(name))
            {
                return _children[name];
            }
            else
            {
                HMetricsContext ctx = new HMetricsContext(name);
                ctx._parent = this;
                _children.Add(name, ctx);
                return ctx;
            }
        }

        internal List<ReportEntry> Report(bool reset, ReportMode mode = ReportMode.Standard)
        {
            List<ReportEntry> result = new List<ReportEntry>();
            result.AddRange(ReportStandard(reset));
            if (mode == ReportMode.IncludeChildren && _children != null)
            {
                result.AddRange(ReportChildren(reset));
            }
            return result;
        }

        private List<ReportEntry> ReportStandard(bool reset)
        {
            List<ReportEntry> result = new List<ReportEntry>();
            foreach(NamedSampler sampler in Samplers)
            {
                result.Add(sampler.AsReportEntry(this.GetContextStack(), reset));
            }
            return result;
        }

        private List<ReportEntry> ReportChildren(bool reset)
        {
            List<ReportEntry> result = new List<ReportEntry>();
            foreach(HMetricsContext childCtx in _children.Values)
            {
                result.AddRange(childCtx.Report(reset, ReportMode.IncludeChildren));
            }
            return result;
        }
    }
}