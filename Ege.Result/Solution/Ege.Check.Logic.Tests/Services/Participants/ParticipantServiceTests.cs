namespace Ege.Check.Logic.Tests.Services.Participants
{
    using System;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Participants;
    using Ege.Check.Dal.Store.Repositories.Participants;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Participant.Participants;
    using Ege.Logic.BaseTests;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ParticipantServiceTests : BaseLogicTest
    {
        [NotNull]
        private Mock<IParticipantCache> _participantCache;
        [NotNull]
        private Mock<IParticipantRepository> _participantRepository;
        [NotNull]
        private ParticipantService _participantService;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            _participantRepository = MockRepository.Create<IParticipantRepository>();
            _participantCache = MockRepository.Create<IParticipantCache>();
            _participantService = new ParticipantService(_participantRepository.Object, _participantCache.Object,
                                                         ConnectionFactory.Object, CacheFactory.Object);
        }

        [TestMethod]
        public async Task GetParticipantsTestInCache()
        {
            const string hash = "hash";
            const string code = "code";
            const int region = 1;

            var p1 = new ParticipantCacheModel();
            var p2 = new ParticipantCacheModel();
            var participants = new[] { p1, p2 };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns(new ParticipantCollectionCacheModel { Participants = participants })
                             .Verifiable("There was no method call IParticipantCache::Get");

            var result = await _participantService.Get(hash, code, null, region);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Participant);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestNotInCache()
        {
            const string hash = "hash";
            const string code = "code";
            const int region = 1;

            var p1 = new ParticipantCacheModel();
            var p2 = new ParticipantCacheModel();
            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns((ParticipantCollectionCacheModel)null)
                             .Verifiable("There was no method call IParticipantCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _participantRepository.Setup(x => x.GetByHash(DbConnection, hash))
                                  .Returns(Task.FromResult(participants))
                                  .Verifiable("There was no method call IParticipantRepository::GetByHash");

            _participantCache.Setup(x => x.Put(DataCacheObject, hash, participants))
                             .Verifiable("There was no method call IParticipantCache::Put");

            var result = await _participantService.Get(hash, code, null, region);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Participant);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestInCacheWithCode()
        {
            const string hash = "hash";
            const string code = "code";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
                {
                    Code = code,
                    RegionId = regionId,
                };

            var p2 = new ParticipantCacheModel
                {
                    Code = code,
                    RegionId = regionId,
                };

            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns(participants)
                             .Verifiable("There was no method call IParticipantCache::Get");

            var result = await _participantService.Get(hash, code, null, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsFalse(result.Collision);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestNotInCacheWithCode()
        {
            const string hash = "hash";
            const string code = "code";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
                {
                    Code = code,
                    RegionId = regionId,
                };

            var p2 = new ParticipantCacheModel
                {
                    Code = code,
                    RegionId = regionId,
                };
            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns((ParticipantCollectionCacheModel)null)
                             .Verifiable("There was no method call IParticipantCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _participantRepository.Setup(x => x.GetByHash(DbConnection, hash))
                                  .Returns(Task.FromResult(participants))
                                  .Verifiable("There was no method call IParticipantRepository::GetByHash");

            _participantCache.Setup(x => x.Put(DataCacheObject, hash, participants))
                             .Verifiable("There was no method call IParticipantCache::Put");

            var result = await _participantService.Get(hash, code, null, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsFalse(result.Collision);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestInCacheWithDocument()
        {
            const string hash = "hash";
            const string document = "document";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
                {
                    Document = document,
                    RegionId = regionId,
                };

            var p2 = new ParticipantCacheModel
            {
                Document = "mwahaha",
                RegionId = regionId,
            };

            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns(participants)
                             .Verifiable("There was no method call IParticipantCache::Get");

            var result = await _participantService.Get(hash, null, document, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsFalse(result.Collision);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestInCacheWithDocumentCollision()
        {
            const string hash = "hash";
            const string document = "document";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
            {
                Document = document,
                RegionId = regionId,
            };

            var p2 = new ParticipantCacheModel
            {
                Document = document,
                RegionId = regionId,
            };

            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                .Returns(DataCacheObject)
                .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                .Returns(participants)
                .Verifiable("There was no method call IParticipantCache::Get");

            var result = await _participantService.Get(hash, null, document, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsTrue(result.Collision);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestNotInCacheWithDocument()
        {
            const string hash = "hash";
            const string document = "document";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
                {
                    Document = document,
                    RegionId = regionId,
                };

            var p2 = new ParticipantCacheModel
                {
                    Document = "mwahaha",
                    RegionId = regionId,
                };
            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns((ParticipantCollectionCacheModel)null)
                             .Verifiable("There was no method call IParticipantCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _participantRepository.Setup(x => x.GetByHash(DbConnection, hash))
                                  .Returns(Task.FromResult(participants))
                                  .Verifiable("There was no method call IParticipantRepository::GetByHash");

            _participantCache.Setup(x => x.Put(DataCacheObject, hash, participants))
                             .Verifiable("There was no method call IParticipantCache::Put");

            var result = await _participantService.Get(hash, null, document, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsFalse(result.Collision);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetParticipantsTestNotInCacheWithDocumentCollision()
        {
            const string hash = "hash";
            const string document = "document";
            const int regionId = 1;

            var p1 = new ParticipantCacheModel
            {
                Document = document,
                RegionId = regionId,
            };

            var p2 = new ParticipantCacheModel
            {
                Document = document,
                RegionId = regionId,
            };
            var participants = new ParticipantCollectionCacheModel { Participants = new[] { p1, p2 } };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _participantCache.Setup(x => x.Get(DataCacheObject, hash))
                             .Returns((ParticipantCollectionCacheModel)null)
                             .Verifiable("There was no method call IParticipantCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _participantRepository.Setup(x => x.GetByHash(DbConnection, hash))
                                  .Returns(Task.FromResult(participants))
                                  .Verifiable("There was no method call IParticipantRepository::GetByHash");

            _participantCache.Setup(x => x.Put(DataCacheObject, hash, participants))
                             .Verifiable("There was no method call IParticipantCache::Put");

            var result = await _participantService.Get(hash, null, document, regionId);
            Assert.IsNotNull(result);
            Assert.AreSame(p1, result.Participant);
            Assert.IsTrue(result.Collision);
            MockRepository.VerifyAll();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task GetParticipantsTestWithCodeAndDocument()
        {
            const string hash = "hash";
            const string code = "code";
            const string document = "document";
            const int regionId = 1;

            await _participantService.Get(hash, code, document, regionId);
        }
    }
}