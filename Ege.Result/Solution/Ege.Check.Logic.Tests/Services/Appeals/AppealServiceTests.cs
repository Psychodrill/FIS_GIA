namespace Ege.Check.Logic.Tests.Services.Appeals
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AppealCollection;
    using Ege.Check.Dal.Store.Repositories.Appeals;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Participant.Appeals;
    using Ege.Logic.BaseTests;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AppealServiceTests : BaseLogicTest
    {
        [NotNull] private Mock<IAppealCollectionCache> _appealCollectionCache;
        [NotNull] private Mock<IAppealRepository> _appealRepository;
        [NotNull] private AppealService _appealService;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            _appealRepository = MockRepository.Create<IAppealRepository>();
            _appealCollectionCache = MockRepository.Create<IAppealCollectionCache>();
            _appealService = new AppealService(_appealRepository.Object, _appealCollectionCache.Object,
                                               ConnectionFactory.Object, CacheFactory.Object);
        }

        [TestMethod]
        public async Task GetAppealsTestInCache()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var collectionCacheModel = new AppealCollectionCacheModel
                {
                    Appeals = new Collection<AppealCacheModel>
                        {
                            new AppealCacheModel
                                {
                                    Date = new DateTime(2014, 3, 2),
                                    Status = 1,
                                }
                        }
                };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _appealCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns(collectionCacheModel)
                                  .Verifiable("There was no method call IAppealCollectionCache::Get");

            var result = await _appealService.GetAppeals(kvp);
            Assert.AreSame(collectionCacheModel, result);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetAppealsTestNotInCache()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var collectionCacheModel = new AppealCollectionCacheModel
                {
                    Appeals = new Collection<AppealCacheModel>
                        {
                            new AppealCacheModel
                                {
                                    Date = new DateTime(2014, 3, 2),
                                    Status = 1,
                                }
                        }
                };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _appealCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns((AppealCollectionCacheModel) null)
                                  .Verifiable("There was no method call IAppealCollectionCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _appealRepository.Setup(x => x.GetAppeals(DbConnection, kvp))
                             .Returns(Task.FromResult(collectionCacheModel))
                             .Verifiable("There was no method call IAppealRepository::GetAppeals");

            _appealCollectionCache.Setup(x => x.Put(DataCacheObject, kvp, collectionCacheModel))
                                  .Verifiable("There was no method call IAppealCollectionCache::Put");

            var result = await _appealService.GetAppeals(kvp);
            Assert.AreSame(collectionCacheModel, result);
            MockRepository.VerifyAll();
        }
    }
}