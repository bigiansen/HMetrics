using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class Float64ValueSampler : ValueSampler<double>
    {
        public Float64ValueSampler(string name) : base(name) { }
    }
}