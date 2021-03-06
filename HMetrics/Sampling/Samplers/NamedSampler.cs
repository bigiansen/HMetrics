﻿using HMetrics.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Sampling.Samplers
{
    public abstract class NamedSampler
    {
        public string Name { get; set; }
        public string[] Tags { get; set; }

        public NamedSampler(string name)
        {
            Name = name;
        }

        public abstract ReportEntry AsReportEntry(Stack<string> contextStack, bool reset);
    }
}