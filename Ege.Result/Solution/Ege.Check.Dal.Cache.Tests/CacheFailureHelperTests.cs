namespace Ege.Check.Dal.Cache.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CacheFailureHelperTests
    {
        [TestMethod]
        public void TestIsCacheFailed()
        {
            var helper = new CacheFailureHelper();
            Assert.IsFalse(helper.IsCacheFailed());
            helper.Failed();
            Assert.IsTrue(helper.IsCacheFailed());
            helper.Up();
            Assert.IsFalse(helper.IsCacheFailed());
        }
    }
}