using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class Float128ValueSampler : ValueSampler<decimal>
    {
        public Float128ValueSampler(string name) : base(name) { }
    }
}
