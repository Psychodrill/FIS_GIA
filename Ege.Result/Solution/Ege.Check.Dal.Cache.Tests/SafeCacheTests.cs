namespace Ege.Check.Dal.Cache.Tests
{
    using System;
    using Ege.Check.Dal.Cache.AppFabric;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;
    using Microsoft.ApplicationServer.Caching;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SafeCacheTests
    {
        private static readonly ICacheWrapper Cache;

        static SafeCacheTests()
        {
            Cache = new AppFabricCacheWrapper(new DataCacheFactory(new DataCacheFactoryConfiguration()).GetCache("TestInstance"));
        }

        [TestMethod]
        public void TestTryGet()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(false);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var expected = new object();
            var actual = cache.TryGet(Cache, c => expected);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void TestTryGetWithParameter()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(false);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet(Cache, (c, i) => (object) i, 666);
            Assert.AreEqual(666, (int) actual);
        }

        [TestMethod]
        public void TestTryGetFailure()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(false);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.Failed());
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet<object>(Cache, c => { throw new CacheException("", new DataCacheException()); });
            Assert.IsNull(actual);
            mockFailureHelper.Verify(f => f.Failed(), Times.Once);
        }

        [TestMethod]
        public void TestTryGetWithParameterFailure()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(false);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.Failed());
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet<object, int>(Cache, (c, i) => { throw new CacheException("", new DataCacheException()); }, 0);
            Assert.IsNull(actual);
            mockFailureHelper.Verify(f => f.Failed(), Times.Once);
        }

        [TestMethod]
        public void TestTryGetWhileFailed()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet(Cache, c => new object());
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTryGetWithParameterWhileFailed()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet(Cache, (c, i) => new object(), 0);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTryGetWhileGetProhibited()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet(Cache, c => new object());
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTryGetWithParameterWhileGetProhibited()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.IsGetProhibited()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var actual = cache.TryGet(Cache, (c, i) => new object(), 0);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTryPut()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var expected = new object();
            object actual = null;
            cache.TryPut(Cache, (c, o) => { actual = o; }, expected);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void TestTryPutWithKey()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            var expected = new object();
            object actual = null;
            string key = null;
            cache.TryPut(Cache, (c, s, o) =>
                {
                    actual = o;
                    key = s;
                }, "key", expected);
            Assert.AreSame(expected, actual);
            Assert.AreEqual("key", key);
        }

        [TestMethod]
        public void TestTryPutFailure()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.Failed());
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            cache.TryPut(Cache, (c, o) => { throw new CacheException("", new DataCacheException()); }, new object());
            mockFailureHelper.Verify(f => f.Failed(), Times.Once);
        }

        [TestMethod]
        public void TestTryPutWithKeyFailure()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(false);
            mockFailureHelper.Setup(f => f.Failed());
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            cache.TryPut(Cache, (c, s, o) => { throw new CacheException("", new DataCacheException()); }, "key", new object());
            mockFailureHelper.Verify(f => f.Failed(), Times.Once);
        }

        [TestMethod]
        public void TestTryPutWithKeyWhenFailed()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            cache.TryPut(Cache, (c, s, o) => { Assert.Fail(); }, "key", new object());
        }

        [TestMethod]
        public void TestTryPutWhenFailed()
        {
            var mockFailureHelper = new Mock<ICacheFailureHelper>(MockBehavior.Strict);
            mockFailureHelper.Setup(f => f.IsCacheFailed()).Returns(true);
            var cache = new SafeCacheImpl(mockFailureHelper.Object);
            cache.TryPut(Cache, (c, o) => { Assert.Fail(); }, new object());
        }

        internal class SafeCacheImpl : SafeCache
        {
            public SafeCacheImpl([NotNull] ICacheFailureHelper cacheFailureHelper) : base(cacheFailureHelper)
            {
            }

            public new T TryGet<T>(ICacheWrapper cache, [NotNull] Func<ICacheWrapper, T> getFunc)
                where T : class
            {
                return base.TryGet(cache, getFunc);
            }

            public new T TryGet<T, TParam>(ICacheWrapper cache, [NotNull] Func<ICacheWrapper, TParam, T> getFunc,
                                           TParam param)
                where T : class
            {
                return base.TryGet(cache, getFunc, param);
            }

            public new void TryPut<TObj>(ICacheWrapper cache, [NotNull] Action<ICacheWrapper, TObj> putAction, TObj obj)
            {
                base.TryPut(cache, putAction, obj);
            }

            public new void TryPut<TObj>(ICacheWrapper cache, [NotNull] Action<ICacheWrapper, string, TObj> putAction,
                                         string key, TObj obj)
            {
                base.TryPut(cache, putAction, key, obj);
            }
        }
    }
}