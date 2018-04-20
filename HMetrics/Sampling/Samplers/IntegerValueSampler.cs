using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class IntegerValueSampler : ValueSampler<int>
    {
        public IntegerValueSampler(string name) : base(name) { }
    }
}
