namespace Ege.Check.Dal.Store.Repositories.Answers
// ReSharper restore CheckNamespace
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Mappers.Staff;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/dat_Subjects/SubjectsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_BallSettings/BallSettingsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_TaskSettings/TaskSettingsTestData.xml")]
    public class AnswerCriteriaRepositoryIntegrationTest : BasePersistentTest
    {
        private AnswerCriteriaRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new AnswerCriteriaRepository(new AnswerCriteriaCollectionMapper(),
                                                       new TaskSettingsDataTableMapper(),
                                                       new AnswerCriteriaCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetExamAnswersWithoutCriteriaTest()
        {
            const int subjectCode = 3;
            var actual = new ExamInfoCacheModel();
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetByCode(connection, subjectCode));
            }

            var expected = new ExamInfoCacheModel
                {
                    Threshold = 36,
                    PartB = new Collection<TaskBInfoCacheModel>
                        {
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 1,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 2,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 3,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 4,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                }
                        },
                    WithCriteria = new Collection<TaskWithCriteriaInfoCacheModel>
                        {
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    MaxValue = 3,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    MaxValue = 3,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    MaxValue = 3,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 4,
                                    MaxValue = 3,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 5,
                                    MaxValue = 3,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 6,
                                    MaxValue = 3,
                                },
                        }
                };
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Threshold, actual.Threshold);
            Assert.IsNotNull(actual.PartB);
            var actualPartB = actual.PartB.ToArray();
            var expectedPartB = expected.PartB.ToArray();
            Assert.AreEqual(expectedPartB.Length, actualPartB.Length);
            Assert.AreEqual(expectedPartB[0].Type, actualPartB[0].Type);
            Assert.AreEqual(expectedPartB[0].Number, actualPartB[0].Number);
            Assert.AreEqual(expectedPartB[0].MaxValue, actualPartB[0].MaxValue);
            Assert.AreEqual(expectedPartB[0].LegalSymbols, actualPartB[0].LegalSymbols);
            Assert.AreEqual(expectedPartB[1].Type, actualPartB[1].Type);
            Assert.AreEqual(expectedPartB[1].Number, actualPartB[1].Number);
            Assert.AreEqual(expectedPartB[1].MaxValue, actualPartB[1].MaxValue);
            Assert.AreEqual(expectedPartB[1].LegalSymbols, actualPartB[1].LegalSymbols);
            Assert.AreEqual(expectedPartB[2].Type, actualPartB[2].Type);
            Assert.AreEqual(expectedPartB[2].Number, actualPartB[2].Number);
            Assert.AreEqual(expectedPartB[2].MaxValue, actualPartB[2].MaxValue);
            Assert.AreEqual(expectedPartB[2].LegalSymbols, actualPartB[2].LegalSymbols);
            Assert.AreEqual(expectedPartB[3].Type, actualPartB[3].Type);
            Assert.AreEqual(expectedPartB[3].Number, actualPartB[3].Number);
            Assert.AreEqual(expectedPartB[3].MaxValue, actualPartB[3].MaxValue);
            Assert.AreEqual(expectedPartB[3].LegalSymbols, actualPartB[3].LegalSymbols);
            Assert.IsNotNull(actual.WithCriteria);
            var actualCriteria = actual.WithCriteria.ToArray();
            var expectedCriteria = expected.WithCriteria.ToArray();
            Assert.AreEqual(expectedCriteria.Length, actualCriteria.Length);
            Assert.AreEqual(expectedCriteria[0].Type, actualCriteria[0].Type);
            Assert.AreEqual(expectedCriteria[0].Number, actualCriteria[0].Number);
            Assert.AreEqual(expectedCriteria[0].MaxValue, actualCriteria[0].MaxValue);
            Assert.AreEqual(expectedCriteria[1].Type, actualCriteria[1].Type);
            Assert.AreEqual(expectedCriteria[1].Number, actualCriteria[1].Number);
            Assert.AreEqual(expectedCriteria[1].MaxValue, actualCriteria[1].MaxValue);
            Assert.AreEqual(expectedCriteria[2].Type, actualCriteria[2].Type);
            Assert.AreEqual(expectedCriteria[2].Number, actualCriteria[2].Number);
            Assert.AreEqual(expectedCriteria[2].MaxValue, actualCriteria[2].MaxValue);
            Assert.AreEqual(expectedCriteria[3].Type, actualCriteria[3].Type);
            Assert.AreEqual(expectedCriteria[3].Number, actualCriteria[3].Number);
            Assert.AreEqual(expectedCriteria[3].MaxValue, actualCriteria[3].MaxValue);
            Assert.AreEqual(expectedCriteria[4].Type, actualCriteria[4].Type);
            Assert.AreEqual(expectedCriteria[4].Number, actualCriteria[4].Number);
            Assert.AreEqual(expectedCriteria[4].MaxValue, actualCriteria[4].MaxValue);
            Assert.AreEqual(expectedCriteria[5].Type, actualCriteria[5].Type);
            Assert.AreEqual(expectedCriteria[5].Number, actualCriteria[5].Number);
            Assert.AreEqual(expectedCriteria[5].MaxValue, actualCriteria[5].MaxValue);
        }

        [TestMethod]
        public async Task GetExamAnswersWithCriteriaTest()
        {
            const int subjectCode = 8;
            var actual = new ExamInfoCacheModel();
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetByCode(connection, subjectCode));
            }

            var expected = new ExamInfoCacheModel
                {
                    Threshold = 37,
                    PartB = new Collection<TaskBInfoCacheModel>
                        {
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 1,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 2,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 3,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 4,
                                    MaxValue = 2,
                                    LegalSymbols = "цифры,минус",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 5,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры,минус,запятая",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 6,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 7,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 8,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 9,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 10,
                                    MaxValue = 1,
                                    LegalSymbols = "кириллица,пробелы,дефисы",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 11,
                                    MaxValue = 1,
                                    LegalSymbols = "кириллица,пробелы,дефисы",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 12,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры,минус",
                                },
                            new TaskBInfoCacheModel
                                {
                                    Type = TaskType.B,
                                    Number = 13,
                                    MaxValue = 1,
                                    LegalSymbols = "цифры,минус",
                                },
                        },
                    WithCriteria = new Collection<TaskWithCriteriaInfoCacheModel>
                        {
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    MaxValue = 2,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    MaxValue = 2,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    MaxValue = 2,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 4,
                                    MaxValue = 2,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 5,
                                    MaxValue = 2,
                                },
                            new TaskWithCriteriaInfoCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 0,
                                    MaxValue = 0,
                                    Criteria = new Collection<TaskCriterionCacheModel>
                                        {
                                            new TaskCriterionCacheModel
                                                {
                                                    Type = TaskType.A,
                                                    Number = 6,
                                                    MaxValue = 1,
                                                    Name = "Определение показателя естественного прироста населения в ‰",
                                                },
                                            new TaskCriterionCacheModel
                                                {
                                                    Type = TaskType.A,
                                                    Number = 7,
                                                    MaxValue = 2,
                                                    Name = "Определение величины миграционного прироста населения"
                                                },
                                        }
                                },
                        }
                };
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Threshold, actual.Threshold);
            Assert.IsNotNull(actual.PartB);
            var actualPartB = actual.PartB.ToArray();
            var expectedPartB = expected.PartB.ToArray();
            Assert.AreEqual(expectedPartB.Length, actualPartB.Length);

            Assert.IsNotNull(actual.WithCriteria);
            var actualCriteria = actual.WithCriteria.ToArray();
            var expectedCriteria = expected.WithCriteria.ToArray();
            Assert.AreEqual(expectedCriteria.Length, actualCriteria.Length);
            Assert.AreEqual(expectedCriteria[0].Type, actualCriteria[0].Type);
            Assert.AreEqual(expectedCriteria[0].Number, actualCriteria[0].Number);
            Assert.AreEqual(expectedCriteria[0].MaxValue, actualCriteria[0].MaxValue);
            Assert.AreEqual(expectedCriteria[1].Type, actualCriteria[1].Type);
            Assert.AreEqual(expectedCriteria[1].Number, actualCriteria[1].Number);
            Assert.AreEqual(expectedCriteria[1].MaxValue, actualCriteria[1].MaxValue);
            Assert.AreEqual(expectedCriteria[2].Type, actualCriteria[2].Type);
            Assert.AreEqual(expectedCriteria[2].Number, actualCriteria[2].Number);
            Assert.AreEqual(expectedCriteria[2].MaxValue, actualCriteria[2].MaxValue);
            Assert.AreEqual(expectedCriteria[3].Type, actualCriteria[3].Type);
            Assert.AreEqual(expectedCriteria[3].Number, actualCriteria[3].Number);
            Assert.AreEqual(expectedCriteria[3].MaxValue, actualCriteria[3].MaxValue);
            Assert.AreEqual(expectedCriteria[4].Type, actualCriteria[4].Type);
            Assert.AreEqual(expectedCriteria[4].Number, actualCriteria[4].Number);
            Assert.AreEqual(expectedCriteria[4].MaxValue, actualCriteria[4].MaxValue);
            Assert.AreEqual(expectedCriteria[5].Type, actualCriteria[5].Type);
            Assert.AreEqual(expectedCriteria[5].Number, actualCriteria[5].Number);
            Assert.AreEqual(expectedCriteria[5].MaxValue, actualCriteria[5].MaxValue);
            Assert.IsNotNull(actualCriteria[5].Criteria);
            var actualCriterion = actualCriteria[5].Criteria.ToArray();
            var expectedCriterion = expectedCriteria[5].Criteria.ToArray();
            Assert.AreEqual(expectedCriterion[0].Type, actualCriterion[0].Type);
            Assert.AreEqual(expectedCriterion[0].Number, actualCriterion[0].Number);
            Assert.AreEqual(expectedCriterion[0].MaxValue, actualCriterion[0].MaxValue);
            Assert.AreEqual(expectedCriterion[0].Name, actualCriterion[0].Name);
            Assert.AreEqual(expectedCriterion[1].Type, actualCriterion[1].Type);
            Assert.AreEqual(expectedCriterion[1].Number, actualCriterion[1].Number);
            Assert.AreEqual(expectedCriterion[1].MaxValue, actualCriterion[1].MaxValue);
            Assert.AreEqual(expectedCriterion[1].Name, actualCriterion[1].Name);
        }
    }
}