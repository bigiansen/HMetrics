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

        public List<NamedSampler> Samplers { get; set; } = new List<NamedSampler>();

        public HMetricsContext(string name)
        {
            Name = name;
        }

        public string GetContextName(string contextSeparator = ".")
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
    }
}