using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling
{
    public class Sample<T>
    {
        public DateTime Date { get; set; }
        public T Value { get; set; }
        public string[] Tags { get; private set; } // Tags are handled by the Reporter

        public Sample(DateTime timeStamp, T value)
        {
            Date = timeStamp;
            Value = value;
        }

        public void SetTags(string[] tags)
        {
            this.Tags = tags;
        }
    }
}