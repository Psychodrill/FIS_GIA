namespace Ege.Check.Dal.Store.Tests.Repositories.Documents
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Mappers.Staff;
    using Ege.Check.Dal.Store.Repositories;
    using Ege.Check.Dal.Store.Repositories.Documents;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/ap_DocumentUrls/DocumentUrlsTestData.xml")]
    public class DocumentUrlRepositoryIntegrationTest : BasePersistentTest
    {
        [NotNull] private DocumentUrlRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new DocumentUrlRepository(new DocumentUrlsCollectionMapper(),
                                                    new DocumentUrlsDataTableMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetAllTest()
        {
            var expected = new DocumentUrlsCollection
                {
                    Documents = new Collection<DocumentUrl>
                        {
                            new DocumentUrl
                                {
                                    Id = 36,
                                    Name = "Методические рекомендации для сотрудников РЦОИ",
                                    Url =
                                        "http://85.143.100.30/Docs/Методические рекомендации для сотрудников РЦОИ.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 37,
                                    Name = "Руководство администратора системы на региональном уровне",
                                    Url =
                                        "http://85.143.100.30/Docs/Руководство администратора системы на региональном уровне.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 38,
                                    Name = "Инструкция для участников ЕГЭ",
                                    Url = "http://85.143.100.30/Docs/Инструкция для участников ЕГЭ.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 39,
                                    Name = "Руководство по настройке web-сервера для публикации бланков",
                                    Url =
                                        "http://85.143.100.30/Docs/Руководство по настройке web-сервера для публикации бланков.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 40,
                                    Name = "Руководство пользователя по выгрузке изображений для интернет",
                                    Url =
                                        "http://85.143.100.30/Docs/Руководство пользователя по выгрузке изображений для интернет.pdf"
                                },
                            new DocumentUrl
                                {
                                    Id = 41,
                                    Name = "Дистрибутив модуля выгрузки бланков",
                                    Url = "http://85.143.100.30/Docs/TestReader_5_4_BlanksExport.rar"
                                }
                        }
                };

            DocumentUrlsCollection actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetAll(connection));
            }

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Documents);
            var expectedDocuments = expected.Documents.ToArray();
            var actualDocuments = actual.Documents.ToArray();
            Assert.AreEqual(expectedDocuments.Length, actualDocuments.Length);
            Assert.AreEqual(expectedDocuments[0].Name, actualDocuments[0].Name);
            Assert.AreEqual(expectedDocuments[0].Url, actualDocuments[0].Url);
            Assert.AreEqual(expectedDocuments[1].Name, actualDocuments[1].Name);
            Assert.AreEqual(expectedDocuments[1].Url, actualDocuments[1].Url);
            Assert.AreEqual(expectedDocuments[2].Name, actualDocuments[2].Name);
            Assert.AreEqual(expectedDocuments[2].Url, actualDocuments[2].Url);
            Assert.AreEqual(expectedDocuments[3].Name, actualDocuments[3].Name);
            Assert.AreEqual(expectedDocuments[3].Url, actualDocuments[3].Url);
            Assert.AreEqual(expectedDocuments[4].Name, actualDocuments[4].Name);
            Assert.AreEqual(expectedDocuments[4].Url, actualDocuments[4].Url);
            Assert.AreEqual(expectedDocuments[5].Name, actualDocuments[5].Name);
            Assert.AreEqual(expectedDocuments[5].Url, actualDocuments[5].Url);
        }

        [TestMethod]
        public async Task UpdateDocumentsTest()
        {
            var expected = new DocumentUrlsCollection
                {
                    Documents = new Collection<DocumentUrl>
                        {
                            new DocumentUrl
                                {
                                    Id = 36,
                                    Name = "Методические рекомендации для сотрудников РЦОИ",
                                    Url =
                                        "http://85.143.100.30/Docs/Методические рекомендации для сотрудников РЦОИ.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 37,
                                    Name = "Руководство администратора системы на региональном уровне",
                                    Url =
                                        "http://85.143.100.30/Docs/Руководство администратора системы на региональном уровне.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 38,
                                    Name = "Инструкция для участников ЕГЭ",
                                    Url = "http://85.143.100.30/Docs/Инструкция для участников ЕГЭ.docx"
                                },
                            new DocumentUrl
                                {
                                    Id = 39,
                                    Name = "Руководство по настройке web-сервера для публикации бланков",
                                    Url =
                                        "http://85.143.100.30/Docs/Руководство по настройке web-сервера для публикации бланков.docx"
                                },
                            new DocumentUrl
                                {
                                    Name = "Новый тестовый 1",
                                    Url = "http://85.143.100.30/test.pdf"
                                },
                            new DocumentUrl
                                {
                                    Name = "Новый тестовый 2",
                                    Url = "http://85.143.100.30/test.rar"
                                }
                        }
                };

            using (var connection = await ConnectionFactory.CreateAsync())
            {
                await _repository.UpdateDocuments(connection, expected.Documents);
            }

            DocumentUrlsCollection actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetAll(connection));
            }

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Documents);
            var expectedDocuments = expected.Documents.ToArray();
            var actualDocuments = actual.Documents.ToArray();
            Assert.AreEqual(expectedDocuments.Length, actualDocuments.Length);
            Assert.AreEqual(expectedDocuments[0].Name, actualDocuments[0].Name);
            Assert.AreEqual(expectedDocuments[0].Url, actualDocuments[0].Url);
            Assert.AreEqual(expectedDocuments[1].Name, actualDocuments[1].Name);
            Assert.AreEqual(expectedDocuments[1].Url, actualDocuments[1].Url);
            Assert.AreEqual(expectedDocuments[2].Name, actualDocuments[2].Name);
            Assert.AreEqual(expectedDocuments[2].Url, actualDocuments[2].Url);
            Assert.AreEqual(expectedDocuments[3].Name, actualDocuments[3].Name);
            Assert.AreEqual(expectedDocuments[3].Url, actualDocuments[3].Url);
            Assert.AreEqual(expectedDocuments[4].Name, actualDocuments[4].Name);
            Assert.AreEqual(expectedDocuments[4].Url, actualDocuments[4].Url);
            Assert.AreEqual(expectedDocuments[5].Name, actualDocuments[5].Name);
            Assert.AreEqual(expectedDocuments[5].Url, actualDocuments[5].Url);
        }

        [TestMethod]
        public async Task UpdateDocumentsWithEmptyParamListTest()
        {
            var emptyList = new List<DocumentUrl>();

            using (var connection = await ConnectionFactory.CreateAsync())
            {
                await _repository.UpdateDocuments(connection, emptyList);
            }

            DocumentUrlsCollection actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetAll(connection));
            }

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Documents);
            var actualDocuments = actual.Documents.ToArray();
            Assert.AreEqual(0, actualDocuments.Length);
        }
    }
}