using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class Float32ValueSampler : ValueSampler<float>
    {
        public Float32ValueSampler(string name) : base(name) { }
    }
}