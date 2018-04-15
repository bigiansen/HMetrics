using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class TimeSamplerContext : IDisposable
    {
        private Stopwatch _stopWatch;
        private TimeSampler _parentSampler;

        public TimeSamplerContext(TimeSampler parent)
        {
            _parentSampler = parent;
        }

        public void Start()
        {
            _stopWatch = Stopwatch.StartNew();
        }

        public void Stop()
        {
            _stopWatch.Stop();
        }

        public double GetElapsed()
        {
            return _stopWatch.Elapsed.TotalMilliseconds;
        }

        public void Dispose()
        {
            _parentSampler.Sample(_stopWatch.Elapsed.TotalMilliseconds);
            _stopWatch.Stop();
        }
    }
}
