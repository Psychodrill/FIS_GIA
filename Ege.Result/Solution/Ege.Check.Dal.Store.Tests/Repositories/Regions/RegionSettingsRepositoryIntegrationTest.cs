namespace Ege.Check.Dal.Store.Repositories.Regions
// ReSharper restore CheckNamespace
{
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Mappers.Staff;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/ap_ExamSettings/ExamSettingsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_GekDocuments/GekDocumentsTestData.xml")]
    [DeploymentItem("../../TestData/Tables/ap_RegionInfo/RegionInfoTestData.xml")]
    public class RegionSettingsRepositoryIntegrationTest : BasePersistentTest
    {
        [NotNull] private RegionSettingsRepository _repository;

        [TestInitialize]
        public override void Init()
        {
            _repository = new RegionSettingsRepository(new RegionSettingsMapper(), new ExamSettingsMapper(),
                                                       new GekDocumentMapper(), new ExamSettingsDataTableMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetAvailableRegionsTest()
        {
            const int regionId = 10;
            var expected = new RegionSettingsCacheModel
                {
                    Servers = new BlanksServerCacheModel
                        {
                            Common = "http://ege.karelia.ru/ab",
                            Composition = string.Empty,
                        },
                    Info = new RegionInfoCacheModel
                        {
                            HotlinePhone = string.Empty,
                            Info = string.Empty,
                        },
                };
            RegionSettingsCacheModel actual;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                actual = (await _repository.GetSettingsForParticipant(connection))[regionId];
            }

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Info.Info, actual.Info.Info);
            Assert.AreEqual(expected.Info.HotlinePhone, actual.Info.HotlinePhone);
            Assert.AreEqual(expected.Servers.Common, actual.Servers.Common);
            Assert.AreEqual(expected.Servers.Composition, actual.Servers.Composition);
            Assert.IsNotNull(actual.Settings);
        }
    }
}