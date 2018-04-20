using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Reporting
{
    public class ReportEntry
    {
        public string SamplerName { get; set; }
        public Stack<String> ContextStack { get; set; }
        public List<string> JsonSamples { get; set; }

        public ReportEntry()
        {
            ContextStack = new Stack<string>();
            JsonSamples = new List<string>();
        }
    }
}