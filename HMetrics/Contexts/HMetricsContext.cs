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

        public IntegerAccumulatorSampler Meter(string name, int samplingTimeWindow, string commaSeparatedTags = "")
        {
            var sampler = new IntegerAccumulatorSampler(name, samplingTimeWindow);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }      

        public TimeSampler Timer(string name, string commaSeparatedTags = "")
        {
            var sampler = new TimeSampler(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }

        public IntegerValueSampler IntegerHistogram(string name, string commaSeparatedTags = "")
        {
            var sampler = new IntegerValueSampler(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }

        public Float32ValueSampler Float32Histogram(string name, string commaSeparatedTags = "")
        {
            var sampler = new Float32ValueSampler(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }

        public Float64ValueSampler Float64Histogram(string name, string commaSeparatedTags = "")
        {
            var sampler = new Float64ValueSampler(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }

        public Float128ValueSampler Float128Histogram(string name, string commaSeparatedTags = "")
        {
            var sampler = new Float128ValueSampler(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
            Samplers.Add(sampler);
            return sampler;
        }        

        public ValueSampler<T> CustomValueHistogram<T>(string name, string commaSeparatedTags = "")
        {
            var sampler = new ValueSampler<T>(name);
            sampler.Tags = ParseTags(commaSeparatedTags);
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

        private static string[] ParseTags(string commaSeparatedTags)
        {
            if(string.IsNullOrWhiteSpace(commaSeparatedTags))
            {
                return new string[0];
            }
            string[] tags = commaSeparatedTags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tags.Length; i++) // Clean tags, just in case
            {
                tags[i] = tags[i].Trim();
            }
            return tags;
        }
    }
}