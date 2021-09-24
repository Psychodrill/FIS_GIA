namespace Ege.Hsc.Logic.Tests.Requests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Ege.Check.Common;
    using Ege.Check.Common.Hash;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Blanks;
    using Ege.Hsc.Logic.Csv;
    using Ege.Hsc.Logic.Models;
    using Ege.Hsc.Logic.Requests;
    using Ege.Logic.BaseTests;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RequestServiceTests : BaseLogicTest
    {
        [NotNull]
        private Mock<IFioHasher> _fioHasher;
        [NotNull]
        private Mock<IBlankRequestRepository> _repository;
        [NotNull]
        private Mock<IBlankZipper> _zipper;
        [NotNull]
        private Mock<IRequestCsvParser> _csvParser;
        [NotNull]
        private Mock<IEqualityComparer<ParticipantBlankRequest>> _requestEqualityComparer;
        [NotNull]
        private Mock<IMapper<RequestData, RequestStatus>> _requestDataMapper;
        [NotNull]
        private Mock<IMapper<ParticipantBlankRequest, RequestedParticipant>> _requestMapper;
        [NotNull]
        private Mock<IMapper<ParticipantBlankRequest, string>> _nameExtractor;
        [NotNull]
        private RequestService _requestService;

        [TestInitialize]
        public override void Init()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            base.Init();
            _fioHasher = MockRepository.Create<IFioHasher>();
            _repository = MockRepository.Create<IBlankRequestRepository>();
            _zipper = MockRepository.Create<IBlankZipper>();
            _csvParser = MockRepository.Create<IRequestCsvParser>();
            _requestEqualityComparer = MockRepository.Create<IEqualityComparer<ParticipantBlankRequest>>();
            _requestDataMapper = MockRepository.Create<IMapper<RequestData, RequestStatus>>();
            _requestMapper = MockRepository.Create<IMapper<ParticipantBlankRequest, RequestedParticipant>>();
            _nameExtractor = MockRepository.Create<IMapper<ParticipantBlankRequest, string>>();
            
            _requestService = new RequestService(
                _fioHasher.Object,
                _repository.Object,
                ConnectionFactory.Object,
                _zipper.Object,
                _csvParser.Object,
                _requestEqualityComparer.Object,
                _requestDataMapper.Object,
                _requestMapper.Object,
                _nameExtractor.Object,
                null,
                null);

            // ReSharper restore AssignNullToNotNullAttribute
        }


        [TestMethod]
        public async Task GetRequestsTest()
        {
            const int userId = 7;

            var requestPage = new RequestDataPage
                {
                    Page = new Collection<RequestData>(),
                    Count = 1
                };

            ConnectionFactory.Setup(x => x.CreateHscAsync())
                .Returns(() => Task.FromResult(DbConnection))
                .Verifiable("There was no method call IDbConnectionFactory::CreateHscAsync");

            _repository.Setup(x => x.GetRequestsData(DbConnection, It.Is<UserReference>(u => u.UserId == userId), 0, 10))
                .Returns(() => Task.FromResult(requestPage))
                .Verifiable("There was no method call IBlankRequestRepository::GetRequestsData");

            var result = await _requestService.GetRequests(new UserReference {UserId = userId}, 0, 10);
            Assert.IsNotNull(result);
            Assert.AreEqual(requestPage.Count, result.Count);
            Assert.IsNotNull(result.Page);
            Assert.AreEqual(requestPage.Page.Count, result.Page.Count);
        }
    }
}