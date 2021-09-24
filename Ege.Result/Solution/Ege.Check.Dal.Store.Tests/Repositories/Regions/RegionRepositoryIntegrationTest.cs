namespace Ege.Check.Dal.Store.Repositories.Regions
// ReSharper restore CheckNamespace
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../TestData/Tables/rbdc_Regions/RegionsTestData.xml")]
    public class RegionRepositoryIntegrationTest : BasePersistentTest
    {
        [NotNull] private RegionRepository _regionRepository;

        [TestInitialize]
        public override void Init()
        {
            _regionRepository = new RegionRepository(new AvailableRegionCollectionMapper());
            base.Init();
        }

        [TestMethod]
        public async Task GetAvailableRegionsTest()
        {
            AvailableRegion[] regions;
            using (var connection = await ConnectionFactory.CreateAsync())
            {
                regions = (await _regionRepository.GetAvailableRegions(connection)).ToArray();
            }
            Assert.AreEqual(65, regions.Length);
            Assert.AreEqual(1, regions[0].Id);
            Assert.AreEqual(2, regions[1].Id);
            Assert.AreEqual(3, regions[2].Id);
            Assert.AreEqual(5, regions[3].Id);
        }
    }
}