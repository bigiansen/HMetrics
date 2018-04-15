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
    [TestClass, TestCategory("IntegerAccumulator")]
    public class IntAccumulatorTests
    {
        private IntegerAccumulatorSampler sampler;
        private const int TIMEWINDOW = 250;

        [TestInitialize]
        public void TestInit()
        {
            sampler = new IntegerAccumulatorSampler("testsampler", TIMEWINDOW);            
        }

        [TestMethod]
        public void Marking_One_AddsCorrectAmount()
        {
            sampler.Mark();

            var samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(0, samples.Count);

            Thread.Sleep(TIMEWINDOW + 10);

            samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(1, samples.Count);
        }

        [TestMethod]
        public void Marking_CustomValue_AddsCorrectAmount()
        {
            sampler.Mark(500);

            var samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(0, samples.Count);

            Thread.Sleep(TIMEWINDOW + 10);

            samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(1, samples.Count);
            Assert.AreEqual(500, samples[0].Value);
        }

        [TestMethod]
        public void SamplingSpeedTest()
        {
            sampler.Mark();

            var samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(0, samples.Count);

            Thread.Sleep(TIMEWINDOW + 10);

            samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(1, samples.Count);

            Thread.Sleep(TIMEWINDOW + 10);

            samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(2, samples.Count);

            Thread.Sleep(TIMEWINDOW + 10);

            samples = sampler.Histogram.GetAllSamples(false);
            Assert.AreEqual(3, samples.Count);
        }
    }
}
