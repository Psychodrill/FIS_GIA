namespace Ege.Check.Dal.StoreRepositories.Appeals
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Repositories;
    using Ege.Check.Dal.Store.Repositories.Appeals;
    using Ege.Check.Logic.Models.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/ap_Participants/ParticipantsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_ParticipantExams/ParticipantExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_Appeals/AppealsTestData.xml")]
    public class AppealRepositoryIntegrationTest : BasePersistentTest
    {
        private AppealRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new AppealRepository(new AppealCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetAppealsTest()
        {
            var expected = new[]
                {
                    new AppealCacheModel
                        {
                            Date = new DateTime(2015, 03, 04, 12, 0, 0),
                            Status = 1
                        },
                    new AppealCacheModel
                        {
                            Date = new DateTime(2015, 03, 05, 12, 0, 0),
                            Status = 3
                        }
                };

            var participant = new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                };

            const int examId = 7;

            var kvp = new KeyValuePair<ParticipantCacheModel, int>(participant, examId);

            AppealCacheModel[] appeals;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                appeals = (await _repository.GetAppeals(connection, kvp)).Appeals.ToArray();
            }
            Assert.AreEqual(2, appeals.Length);
            Assert.AreEqual(expected[0].Date, appeals[0].Date);
            Assert.AreEqual(expected[0].Status, appeals[0].Status);
            Assert.AreEqual(expected[1].Date, appeals[1].Date);
            Assert.AreEqual(expected[1].Status, appeals[1].Status);
        }
    }
}