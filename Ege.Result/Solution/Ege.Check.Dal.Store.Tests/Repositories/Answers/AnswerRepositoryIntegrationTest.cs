namespace Ege.Check.Dal.Store.Repositories.Answers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/ap_Participants/ParticipantsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_ParticipantExams/ParticipantExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_Exams/ExamsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/dat_Subjects/SubjectsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_Answers/AnswersTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_BlankInfo/BlankInfoTestData.xml")]
    public class AnswerRepositoryIntegrationTest : BasePersistentTest
    {
        private AnswerRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new AnswerRepository(new AnswerCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetExamAnswersTest()
        {
            var participant = new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                };

            const int examId = 7;

            var expectedEx = new AnswerCollectionCacheModel
                {
                    PrimaryMark = 1,
                    TestMark = 3,
                    Mark5 = 2,
                    IsHidden = true,
                };
            var expectedAns = new[]
                {
                    new AnswerCacheModel
                        {
                            Answer = "Тестовый ответ",
                            Type = TaskType.A,
                            Number = 1,
                            Mark = 1
                        },
                    new AnswerCacheModel
                        {
                            Answer = "Тестовый ответ1",
                            Type = TaskType.A,
                            Number = 2,
                            Mark = 0
                        },
                    new AnswerCacheModel
                        {
                            Answer = "Тестовый ответ3",
                            Type = TaskType.A,
                            Number = 3,
                            Mark = 3
                        }
                };
            const string barcode = "fake_barcode";
            const int blankType = 1;
            const int pageCount = 1;
            const int projectBatchId = 100;
            const string projectName = "project";

            var kvp = new KeyValuePair<ParticipantCacheModel, int>(participant, examId);

            AnswerCollectionCacheModel actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetExamAnswers(connection, kvp));
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedEx.PrimaryMark, actual.PrimaryMark);
            Assert.AreEqual(expectedEx.TestMark, actual.TestMark);
            Assert.AreEqual(expectedEx.Mark5, actual.Mark5);
            Assert.AreEqual(expectedEx.IsHidden, actual.IsHidden);
            Assert.IsNotNull(actual.Answers);
            var answers = actual.Answers.ToArray();
            Assert.AreEqual(3, answers.Length);
            Assert.AreEqual(expectedAns[0].Answer, answers[0].Answer);
            Assert.AreEqual(expectedAns[0].Type, answers[0].Type);
            Assert.AreEqual(expectedAns[0].Number, answers[0].Number);
            Assert.AreEqual(expectedAns[0].Mark, answers[0].Mark);
            Assert.AreEqual(expectedAns[1].Answer, answers[1].Answer);
            Assert.AreEqual(expectedAns[1].Type, answers[1].Type);
            Assert.AreEqual(expectedAns[1].Number, answers[1].Number);
            Assert.AreEqual(expectedAns[1].Mark, answers[1].Mark);
            Assert.AreEqual(expectedAns[2].Answer, answers[2].Answer);
            Assert.AreEqual(expectedAns[2].Type, answers[2].Type);
            Assert.AreEqual(expectedAns[2].Number, answers[2].Number);
            Assert.AreEqual(expectedAns[2].Mark, answers[2].Mark);
            Assert.AreEqual(1, actual.Blanks.Count);
            var blank = actual.Blanks.First();
            Assert.IsNotNull(blank);
            Assert.AreEqual(blank.Barcode, barcode);
            Assert.AreEqual(blank.BlankType, blankType);
            Assert.AreEqual(blank.PageCount, pageCount);
            Assert.AreEqual(blank.ProjectName, projectName);
            Assert.AreEqual(blank.ProjectBatchId, projectBatchId);
        }
    }
}