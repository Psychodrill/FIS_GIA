namespace Ege.Check.Dal.Store.Repositories.Participants
// ReSharper restore CheckNamespace
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/ap_Participants/ParticipantsTestData.xml")]
    public class ParticipantRepositoryIntegrationTest : BasePersistentTest
    {
        [NotNull] private ParticipantRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new ParticipantRepository(new ParticipantCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetByHashTest()
        {
            var expected = new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = "111111",
                    RegionId = 61
                };

            const string hash = "805f6a91f58481c048d801782b24b355";
            ParticipantCacheModel[] participants;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                participants = (await _repository.GetByHash(connection, hash)).Participants.ToArray();
            }
            Assert.AreEqual(1, participants.Length);
            Assert.AreEqual(expected.Code, participants[0].Code);
            Assert.AreEqual(expected.Document, participants[0].Document);
            Assert.AreEqual(expected.RegionId, participants[0].RegionId);
        }
    }
}