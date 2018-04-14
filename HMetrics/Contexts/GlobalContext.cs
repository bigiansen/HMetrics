using HMetrics.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics
{
    public static class GlobalContext
    {
        public static Dictionary<string, HMetricsContext> Contexts = new Dictionary<string, HMetricsContext>();

        public static HMetricsContext GetContext(string name)
        {
            if (Contexts.ContainsKey(name))
            {
                return Contexts[name];
            }
            else
            {
                HMetricsContext ctx = new HMetricsContext(name);
                Contexts.Add(name, ctx);
                return ctx;
            }
        }
    }
}