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
        internal Histogram<int> Histogram = new Histogram<int>();
        internal int Accumulator = 0;
        private int _timeWindow;
        private Task _cyclingTask;
        private object _cycleLock = new object();
        private bool _started = false;

        public IntegerAccumulatorSampler(string name, int timeWindowMs) 
            : base(name)
        {
            this._timeWindow = timeWindowMs;            
        }

        public override ReportEntry AsReportEntry(Stack<string> contextStack, bool reset)
        {
            ReportEntry result = new ReportEntry();
            result.ContextStack = contextStack;
            foreach(Sample<int> sample in Histogram.GetAllSamples(reset))
            {
                sample.SetTags(this.Tags);
                result.JsonSamples.Add(sample.ToJson());
            }
            return result;
        }

        public void Mark(int amount = 1)
        {
            if(!_started)
            {
                _started = true;
                InitCycling();
            }
            lock (_cycleLock)
            {
                Accumulator += amount;
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
                        value = Accumulator;
                        Accumulator = 0;
                    }
                    Histogram.Sample(value);
                }
            });
        }
    }
}