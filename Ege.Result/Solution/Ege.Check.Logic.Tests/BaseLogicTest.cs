namespace Ege.Check.Logic.Tests
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public abstract class BaseLogicTest
    {
        [NotNull] protected Mock<ICacheFactory> CacheFactory;
        [NotNull] protected Mock<IDbConnectionFactory> ConnectionFactory;

        protected ICacheWrapper DataCacheObject;

        [NotNull] protected MockRepository MockRepository;

        [NotNull]
        protected DbConnection DbConnection { get; set; }

        [TestInitialize]
        public virtual void Init()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
            ConnectionFactory = MockRepository.Create<IDbConnectionFactory>();
            CacheFactory = MockRepository.Create<ICacheFactory>();
            DataCacheObject = null;
            DbConnection = new SqlConnection();
        }
    }
}