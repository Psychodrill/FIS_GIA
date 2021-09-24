namespace Ege.Check.Dal.Cache.Tests.CacheFactory
{
    using Ege.Check.Common.Config;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CacheNameProviderTests
    {
        [TestMethod]
        public void TestGetCacheName()
        {
            var provider = new CacheSettingsProvider(new ConfigReaderHelper());
            Assert.AreEqual("TestInstance", provider.GetCacheName());
        }
    }
}