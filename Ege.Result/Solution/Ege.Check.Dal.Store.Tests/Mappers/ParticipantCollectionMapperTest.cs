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
    public class ParticipantCollectionMapperTest
    {
        [TestMethod]
        public async Task TestMap()
        {
            var mapper = new ParticipantCollectionMapper();
            var sqlDataReader = new Mock<DbDataReader>(MockBehavior.Strict);

            //const int rbdIdOrdinal = 1;
            const int hashOrdinal = 2;
            const int codeOrdinal = 3;
            const int documentOrdinal = 4;
            const int regionOrdinal = 5;

            //sqlDataReader.Setup(x => x.GetOrdinal("ParticipantRbdId")).Returns(rbdIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ParticipantHash")).Returns(hashOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ParticipantCode")).Returns(codeOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("DocumentNumber")).Returns(documentOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("RegionId")).Returns(regionOrdinal).Verifiable();

            var clousureIterationNumber = 0;
            const int maxClousureIterations = 2;
            sqlDataReader.Setup(x => x.ReadAsync(CancellationToken.None)).Returns(() =>
                {
                    var res = Task.FromResult(clousureIterationNumber < maxClousureIterations);
                    clousureIterationNumber++;
                    return res;
                });

            //sqlDataReader.Setup(x => x.GetGuid(rbdIdOrdinal)).Returns((int ordinalId) => ClousureIterator.ToGuid(clousureIterationNumber, ordinalId)).Verifiable();
            sqlDataReader.Setup(x => x.GetString(hashOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(clousureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(codeOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(codeOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(clousureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(documentOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(documentOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(clousureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(regionOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterationNumber, ordinalId))
                         .Verifiable();

            var participantCacheModels = (await mapper.Map(sqlDataReader.Object)).Participants.ToArray();
            Assert.AreEqual(maxClousureIterations, participantCacheModels.Length);
            for (var i = 1; i <= maxClousureIterations; i++)
            {
                var value = participantCacheModels[i - 1];
                Assert.IsNotNull(value);
                Assert.AreEqual(ClosureIterator.ToString(i, hashOrdinal), value.Hash);
                Assert.AreEqual(ClosureIterator.ToString(i, codeOrdinal), value.Code);
                Assert.AreEqual(ClosureIterator.ToString(i, documentOrdinal), value.Document);
                Assert.AreEqual(ClosureIterator.ToInt(i, regionOrdinal), value.RegionId);
            }
            sqlDataReader.VerifyAll();
        }
    }
}