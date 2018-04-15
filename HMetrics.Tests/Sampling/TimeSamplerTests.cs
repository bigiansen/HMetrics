using HMetrics.Sampling.Samplers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMetrics.Tests.Sampling
{
    [TestClass]
    public class TimeSamplerTests
    {
        private TimeSampler sampler;
        private const int MAX_INACCURACY_MS = 20;
        [TestInitialize]
        public void TestInit()
        {
            sampler = new TimeSampler("testTimer");
        }

        [TestMethod]
        public void TestSampling()
        {
            using (var ctx = sampler.NewContext())
            {
                Thread.Sleep(250);
            }

            var samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(1, samples.Count);
        }

        [TestMethod]
        public void TestAccuracy()
        {
            using (var ctx = sampler.NewContext())
            {
                Thread.Sleep(250);
            }

            var samples = sampler.Histogram.GetAllSamples(false);

            int minAcceptableValue = 250 - MAX_INACCURACY_MS;
            int maxAcceptableValue = 250 + MAX_INACCURACY_MS;

            bool isAcceptable = samples[0].Value >= minAcceptableValue && samples[0].Value <= maxAcceptableValue;
            Assert.IsTrue(isAcceptable);
        }
    }
}
