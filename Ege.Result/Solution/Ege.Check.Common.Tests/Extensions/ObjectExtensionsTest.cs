namespace Ege.Check.Common.Tests.Extensions
{
    using System;
    using System.Linq;
    using Ege.Check.Common.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectExtensionsTest
    {
        [TestMethod]
        public void TestToEnumerable()
        {
            var a = new object();
            var res = a.ToEnumerable().ToArray();
            Assert.AreEqual(1, res.Length);
            Assert.AreEqual(a, res[0]);
        }

        [TestMethod]
        public void TestToEnumerableNull()
        {
            object a = null;
            var res = a.ToEnumerable().ToArray();
            Assert.AreEqual(1, res.Length);
            Assert.IsNull(res[0]);
        }

        [TestMethod]
        public void TestToEnumerableNotNullNull()
        {
            object a = null;
            var res = a.ToEnumerableNotNull().ToArray();
            Assert.AreEqual(0, res.Length);
        }

        [TestMethod]
        public void TestToEnumerableNotNullStructNull()
        {
            DateTime? a = null;
            var res = a.ToEnumerableNotNull().ToArray();
            Assert.AreEqual(0, res.Length);
        }

        [TestMethod]
        public void TestToEnumerableNotNullNotNull()
        {
            var a = new object();
            var res = a.ToEnumerableNotNull().ToArray();
            Assert.AreEqual(1, res.Length);
            Assert.AreEqual(a, res[0]);
        }

        [TestMethod]
        public void TestToEnumerableNotNullStructNotNull()
        {
            DateTime? a = new DateTime();
            var res = a.ToEnumerableNotNull().ToArray();
            Assert.AreEqual(1, res.Length);
            Assert.AreEqual(a, res[0]);
        }
    }
}