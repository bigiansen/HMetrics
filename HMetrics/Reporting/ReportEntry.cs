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
        public Stack<String> ContextStack { get; set; }
        public List<string> JsonSamples { get; set; }
    }
}
