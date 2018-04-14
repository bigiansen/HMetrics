using HMetrics.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics
{
    public static class HMetrics
    {
        public static HMetricsContext Context(string name)
        {
            return ContextManager.GetContext(name);
        }
    }
}
