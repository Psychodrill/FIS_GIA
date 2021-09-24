namespace Ege.Check.Dal.Store.Repositories.ExamCancellation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/rbdc_Regions/RegionsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_subjects/SubjectsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_Exams/ExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_Participants/ParticipantsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_ParticipantExams/ParticipantExamsTestData.xml")]
    public class ExamCancellationRepositoryIntegrationTest : BasePersistentTest
    {
        [NotNull] private ExamCancellationRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new ExamCancellationRepository(new CancelledExamMapper(), new CancelledParticipantExamCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetCancellationWithResult()
        {
            var expected = new CancelledExamsPage
                {
                    Count = 1,
                    Page = new Collection<CancelledExam>
                        {
                            new CancelledExam
                                {
                                    RegionId = 61,
                                    RegionName = "Ростовская область",
                                    Code = "000000000001",
                                    ExamGlobalId = 7,
                                    Date = new DateTime(2014, 04, 28, 0, 0, 0),
                                    SubjectName = "Математика",
                                }
                        }
                };

            const int regionId = 61;

            CancelledExamsPage actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetCancelled(connection, regionId, 5, 0));
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.IsNotNull(expected.Page);
            var expectedPages = expected.Page.ToArray();
            var actualPages = actual.Page.ToArray();
            Assert.AreEqual(expectedPages.Length, actualPages.Length);
            Assert.AreEqual(expectedPages[0].RegionId, actualPages[0].RegionId);
            Assert.AreEqual(expectedPages[0].RegionName, actualPages[0].RegionName);
            Assert.AreEqual(expectedPages[0].Code, actualPages[0].Code);
            Assert.AreEqual(expectedPages[0].ExamGlobalId, actualPages[0].ExamGlobalId);
            Assert.AreEqual(expectedPages[0].Date, actualPages[0].Date);
            Assert.AreEqual(expectedPages[0].SubjectName, actualPages[0].SubjectName);
        }

        [TestMethod]
        public async Task GetCancellationWithoutResult()
        {
            const int regionId = 62;

            CancelledExamsPage actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetCancelled(connection, regionId, 5, 0));
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }
    }
}