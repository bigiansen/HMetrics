using HMetrics.Sampling;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Metrics
{
    internal class Histogram<T>
    {
        private List<Sample<T>> _samples = new List<Sample<T>>();

        public Histogram() { }

        /// <summary>
        /// Add sample to histogram. TimeStamp defaults to DateTime.UtcNow.
        /// </summary>
        /// <param name="value"></param>
        public void Sample(T value)
        {
            lock (_samples)
            {
                _samples.Add(new Sample<T>(DateTime.UtcNow, value));
            }
        }

        /// <summary>
        /// Add sample to histogram, with a user-defined TimeStamp value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        public void Sample(T value, DateTime timeStamp)
        {
            lock (_samples)
            {
                _samples.Add(new Sample<T>(timeStamp, value));
            }
        }

        /// <summary>
        /// Returns a snapshot of all the current samples
        /// </summary>
        /// <returns></returns>
        public List<Sample<T>> GetAllSamples(bool reset = false)
        {
            List<Sample<T>> result = GetSamplesCopy();
            if(reset)
            {
                Reset();
            }
            return result;
        }

        /// <summary>
        /// Clear all samples.
        /// </summary>
        public void Reset()
        {
            lock(_samples)
            {
                _samples.Clear();
            }
        }

        private List<Sample<T>> GetSamplesCopy()
        {
            List<Sample<T>> result = new List<Sample<T>>();
            lock (_samples)
            {
                result = _samples.ToList().ToJson().FromJson<List<Sample<T>>>();
            }
            return result;
        }
    }
}
