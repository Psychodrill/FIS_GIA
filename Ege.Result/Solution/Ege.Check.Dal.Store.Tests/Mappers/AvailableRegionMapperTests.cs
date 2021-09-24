namespace Ege.Check.Dal.Store.Tests.Mappers
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AvailableRegionMapperTests
    {
        [TestMethod]
        public async Task TestMap()
        {
            var mapper = new AvailableRegionCollectionMapper();
            var sqlDataReader = new Mock<DbDataReader>(MockBehavior.Strict);
            const int ordinalId = 1;
            sqlDataReader.Setup(x => x.GetOrdinal("REGION")).Returns(ordinalId).Verifiable();

            var clousureIterator = new ClosureIterator(2);
            sqlDataReader.Setup(x => x.ReadAsync(CancellationToken.None))
                         .Returns(() => Task.FromResult(clousureIterator.Next()));
            sqlDataReader.Setup(x => x.GetInt32(ordinalId))
                         .Returns((int ordinal) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinal))
                         .Verifiable();

            var regions = (await mapper.Map(sqlDataReader.Object)).ToArray();
            Assert.AreEqual(clousureIterator.MaxIterations, regions.Length);
            foreach (var clousureIteration in clousureIterator.Enumerate(regions))
            {
                Assert.IsNotNull(clousureIteration);
                Assert.IsNotNull(clousureIteration.Value);
                Assert.AreEqual(clousureIteration.IntValue(ordinalId), clousureIteration.Value.Id);
            }

            sqlDataReader.VerifyAll();
        }
    }
}