using HMetrics.Metrics;
using HMetrics.Reporting;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public class IntegerAccumulatorSampler : NamedSampler
    {
        private Histogram<int> _histogram = new Histogram<int>();
        private int _accumulator = 0;
        private int _timeWindow;
        private Task _cyclingTask;
        private object _cycleLock = new object();

        public IntegerAccumulatorSampler(string name, int timeWindowMs) 
            : base(name)
        {
            this._timeWindow = timeWindowMs;
        }

        public override ReportEntry AsReportEntry(Stack<string> contextStack, bool reset)
        {
            ReportEntry result = new ReportEntry();
            result.ContextStack = contextStack;
            foreach(Sample<int> sample in _histogram.GetAllSamples(reset))
            {
                result.JsonSamples.Add(sample.ToJson());
            }
            return result;
        }

        public void Mark(int amount = 1)
        {
            lock (_cycleLock)
            {
                _accumulator += amount;
            }
        }

        private void InitCycling()
        {
            _cyclingTask = Task.Run(() =>
            {
                while(true)
                {
                    Thread.Sleep(_timeWindow);
                    int value;
                    lock (_cycleLock)
                    {
                        value = _accumulator;
                        _accumulator = 0;
                    }
                    _histogram.Sample(value);
                }
            });
        }
    }
}