using HMetrics.Sampling.Samplers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMetrics.Tests.Sampling
{
    [TestClass, TestCategory("IntegerValueSampler")]
    public class IntegerValueSamplerTests
    {
        private IntegerValueSampler _sampler;

        [TestInitialize]
        public void TestInit()
        {
            _sampler = new IntegerValueSampler("testsampler");
        }

        [TestMethod]
        public void Sampling_One_Stores()
        {
            _sampler.Sample(222);

            var samples = _sampler.GetAllSamples(true);

            Assert.AreEqual(1, samples.Count);
            Assert.AreEqual(222, samples[0].Value);
        }

        [TestMethod]
        public void Sampling_Multiple_Stores()
        {
            _sampler.Sample(111);
            _sampler.Sample(222);
            _sampler.Sample(333);
            _sampler.Sample(444);

            var samples = _sampler.GetAllSamples(true);

            Assert.AreEqual(4, samples.Count);
            samples.ForEach(s => Assert.IsTrue(s.Value == 111 | s.Value == 222 | s.Value == 333 | s.Value == 444));
        }
    }
}
