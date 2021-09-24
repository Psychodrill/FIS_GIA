namespace FbsUnitTestCommon
{
    using System;

    using Fbs.Core.BatchCheck;

    using NUnit.Framework;

    [TestFixture]
    public class BatchCheckFilterTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseInvalidFilterTest()
        {
            string filterString = "Filter$1$2";
            BatchCheckFilter filter = BatchCheckFilter.Parse(filterString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseInvalidFilter2Test()
        {
            string filterString = "Filter%2a";
            BatchCheckFilter filter = BatchCheckFilter.Parse(filterString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseInvalidFilter3Test()
        {
            string filterString = "Filter%18";
            BatchCheckFilter filter = BatchCheckFilter.Parse(filterString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseInvalidFilter4Test()
        {
            string filterString = "Filter%";
            BatchCheckFilter filter = BatchCheckFilter.Parse(filterString);
        }

        [Test]
        public void ParseTest()
        {
            string filterString = "Filter% 11 %2";
            BatchCheckFilter filter = BatchCheckFilter.Parse(filterString);
            Assert.IsNotNull(filter);
            Assert.IsNotNull(filter.SubjectIds);
            Assert.AreEqual(2, filter.SubjectIds.Length);
            Assert.AreEqual(11, filter.SubjectIds[0]);
            Assert.AreEqual(2, filter.SubjectIds[1]);
        }

    }
}
