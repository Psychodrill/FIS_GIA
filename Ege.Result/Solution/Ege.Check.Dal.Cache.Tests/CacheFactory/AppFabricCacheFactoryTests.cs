namespace Ege.Check.Dal.Cache.Tests.CacheFactory
{
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AppFabric;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Microsoft.ApplicationServer.Caching;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AppFabricCacheFactoryTests
    {
        [TestMethod]
        public void TestGetCache()
        {
            var mockProvider = new Mock<ICacheSettingsProvider>(MockBehavior.Strict);
            mockProvider.Setup(p => p.GetCacheName()).Returns("TestInstance");
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            var factory = new AppFabricCacheFactory(mockProvider.Object, mockFailureHelper.Object);

            var cache = factory.GetCache() as AppFabricCacheWrapper;
            Assert.IsNotNull(cache);
            var field = typeof (DataCache).GetField("_myName", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual("TestInstance", field.GetValue(cache.CacheObject));
        }

        [TestMethod]
        public void TestGetCacheFailure()
        {
            var mockProvider = new Mock<ICacheSettingsProvider>(MockBehavior.Strict);
            mockProvider.Setup(p => p.GetCacheName()).Returns("NonexistentCache");
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.Failed()).Verifiable();
            var factory = new AppFabricCacheFactory(mockProvider.Object, mockFailureHelper.Object);

            ICacheWrapper cache = null;
            var thread = new Thread(() =>
                {
                    cache = factory.GetCache();
                });
            thread.Start();
            thread.Join();
            Assert.IsNull(cache, "{0}", cache);
            mockFailureHelper.Verify(f => f.Failed(), Times.Once());
        }

        [TestMethod]
        public void TestGetCacheWhileFailed()
        {
            var mockProvider = new Mock<ICacheSettingsProvider>(MockBehavior.Strict);
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(true);
            var factory = new AppFabricCacheFactory(mockProvider.Object, mockFailureHelper.Object);

            var cache = factory.GetCache();
            Assert.IsNull(cache);
        }
    }
}