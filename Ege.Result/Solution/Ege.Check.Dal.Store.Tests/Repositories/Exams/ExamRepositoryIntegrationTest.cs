namespace Ege.Check.Dal.Store.Tests.Repositories.Exams
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Repositories;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Logic.Models.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/rbdc_Regions/RegionsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_subjects/SubjectsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_Exams/ExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_Participants/ParticipantsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_ParticipantExams/ParticipantExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_Appeals/AppealsTestData.xml")]
    public class ExamRepositoryIntegrationTest : BasePersistentTest
    {
        private ParticipantExamRepository _participantExamRepository;

        [TestInitialize]
        public override void Init()
        {
            _participantExamRepository = new ParticipantExamRepository(new ExamCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task TestGetById()
        {
            ExamCollectionCacheModel res;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                res =
                    await
                    _participantExamRepository.GetByParticipant(connection,
                                                                new ParticipantCacheModel
                                                                    {
                                                                        Code = "000000000001",
                                                                        RegionId = 61
                                                                    });
            }
            Assert.IsNotNull(res);
            var exams = res.Exams.ToArray();
            Assert.AreEqual(2, exams.Length);

            Assert.AreEqual(6, exams[0].ExamId);
            Assert.AreEqual(DateTime.Parse("2014-04-21T00:00:00"), exams[0].ExamDate);
            Assert.AreEqual("Русский язык", exams[0].Subject);
            Assert.AreEqual(2, exams[0].TestMark);
            Assert.AreEqual(5, exams[0].Mark5);
            Assert.AreEqual(36, exams[0].MinMark);
            Assert.AreEqual(1, exams[0].Status);
            Assert.AreEqual(false, exams[0].IsHidden);
            Assert.AreEqual(false, exams[0].HasAppeal);

            Assert.AreEqual(7, exams[1].ExamId);
            Assert.AreEqual(DateTime.Parse("2014-04-28T00:00:00"), exams[1].ExamDate);
            Assert.AreEqual("Математика", exams[1].Subject);
            Assert.AreEqual(3, exams[1].TestMark);
            Assert.AreEqual(2, exams[1].Mark5);
            Assert.AreEqual(24, exams[1].MinMark);
            Assert.AreEqual(2, exams[1].Status);
            Assert.AreEqual(true, exams[1].IsHidden);
            Assert.AreEqual(true, exams[1].HasAppeal);
        }
    }
}