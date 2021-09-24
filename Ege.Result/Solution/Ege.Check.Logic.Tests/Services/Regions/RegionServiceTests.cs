namespace Ege.Check.Logic.Tests.Services.Regions
{
    using Ege.Logic.BaseTests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RegionServiceTests : BaseLogicTest
    {
        //[NotNull] private Mock<IRegionMemoryCache> _regionCache;
        //[NotNull] private Mock<IRegionRepository> _regionRepository;
        //[NotNull] private RegionService _regionService;

        //[TestInitialize]
        //public override void Init()
        //{
        //    base.Init();
        //    _regionCache = MockRepository.Create<IRegionMemoryCache>();
        //    _regionRepository = MockRepository.Create<IRegionRepository>();
        //    _regionService = new RegionService(_regionCache.Object, _regionRepository.Object, ConnectionFactory.Object,
        //                                       CacheFactory.Object);
        //}

        //[TestMethod]
        //public async Task TestGetAvailableRegionInCache()
        //{
        //    var availableRegions = new[]
        //        {
        //            new AvailableRegion(),
        //            new AvailableRegion()
        //        };

        //    CacheFactory.Setup(x => x.GetCache())
        //                .Returns(DataCacheObject)
        //                .Verifiable("There was no method call CacheFactory::GetCache");

        //    _regionCache.Setup(x => x.GetAvailableRegions(DataCacheObject))
        //                .Returns(availableRegions)
        //                .Verifiable("There was no method call IRegionCache::GetAvailableRegions");
        //    var result = await _regionService.GetAvailableRegions();
        //    Assert.AreEqual(availableRegions, result);
        //    MockRepository.VerifyAll();
        //}

        //[TestMethod]
        //public async Task TestGetAvailableRegionNotInCache()
        //{
        //    ICollection<AvailableRegion> availableRegions = new[]
        //        {
        //            new AvailableRegion(),
        //            new AvailableRegion()
        //        };

        //    CacheFactory.Setup(x => x.GetCache())
        //                .Returns(DataCacheObject)
        //                .Verifiable("Not called CacheFactory::GetCache");

        //    _regionCache.Setup(x => x.GetAvailableRegions(DataCacheObject))
        //                .Returns((AvailableRegion[]) null)
        //                .Verifiable("There was no method call IRegionCache::GetAvailableRegions");
        //    ConnectionFactory.Setup(x => x.CreateAsync())
        //                     .Returns(() => Task.FromResult(DbConnection))
        //                     .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");
        //    _regionRepository.Setup(x => x.GetAvailableRegions(DbConnection))
        //                     .Returns(Task.FromResult(availableRegions))
        //                     .Verifiable("There was no method call IRegionRepository::GetAvailableRegions");
        //    _regionCache.Setup(x => x.PutAvailableRegions(DataCacheObject, availableRegions))
        //                .Verifiable("There was no method call IRegionCache::PutAvailableRegions");

        //    var result = await _regionService.GetAvailableRegions();
        //    Assert.AreEqual(availableRegions, result);
        //    MockRepository.VerifyAll();
        //}
    }
}