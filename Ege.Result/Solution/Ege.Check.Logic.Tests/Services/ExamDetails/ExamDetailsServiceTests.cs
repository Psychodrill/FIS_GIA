namespace Ege.Check.Logic.Tests.Services.ExamDetails
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Blanks;
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Dal.MemoryCache.ExamInfo;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Repositories.Answers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Check.Logic.Services.Participant.ExamDetails;
    using Ege.Logic.BaseTests;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ExamDetailsServiceTests : BaseLogicTest
    {
        [NotNull]
        private Mock<IAnswerCollectionCache> _answerCollectionCache;
        [NotNull]
        private Mock<IAnswerRepository> _answerRepository;
        [NotNull]
        private ExamDetailsService _examDetailService;
        [NotNull]
        private Mock<IExamInfoMemoryCache> _examInfoMemoryCache;
        [NotNull]
        private Mock<IRegionSettingsMemoryCache> _regionSettingMemoryCache;
        [NotNull]
        private Mock<ISubjectExamMemoryCache> _subjectExamMemoryCache;
        [NotNull]
        private Mock<IBlankModelCreator> _blankModelCreator;

        private Mock<ICancelledParticipantExamMemoryCache> _cancelledExamsMemoryCache;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            _answerRepository = MockRepository.Create<IAnswerRepository>();
            _answerCollectionCache = MockRepository.Create<IAnswerCollectionCache>();
            _examInfoMemoryCache = MockRepository.Create<IExamInfoMemoryCache>();
            _regionSettingMemoryCache = MockRepository.Create<IRegionSettingsMemoryCache>();
            _subjectExamMemoryCache = MockRepository.Create<ISubjectExamMemoryCache>();
            _blankModelCreator = MockRepository.Create<IBlankModelCreator>();
            _cancelledExamsMemoryCache = MockRepository.Create<ICancelledParticipantExamMemoryCache>();

            _examDetailService = new ExamDetailsService(
                _answerRepository.Object,
                _answerCollectionCache.Object,
                _examInfoMemoryCache.Object,
                ConnectionFactory.Object,
                _regionSettingMemoryCache.Object,
                CacheFactory.Object,
                _subjectExamMemoryCache.Object,
                _blankModelCreator.Object,
                _cancelledExamsMemoryCache.Object);
        }

        [TestMethod]
        public async Task GetExamDetailsTestWithShowResultFalse()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var regionSettings = new RegionSettingsCacheModel
                {
                    Info = new RegionInfoCacheModel
                        {
                            Info = "Информация",
                            HotlinePhone = "43242344",
                        },
                    Servers = new BlanksServerCacheModel
                        {
                            Common = "http\\common",
                            Composition = "http\\composition",
                        },
                    Settings = new Dictionary<int, RegionExamSettingCacheModel>
                        {
                            {
                                7, new RegionExamSettingCacheModel
                                    {
                                        GekDocument = new GekDocumentCacheModel
                                            {
                                                GekDate = new DateTime(2015, 03, 13),
                                                GekNumber = "000634",
                                                Url = "http\\afsdfsdfsdf",
                                            },
                                        ShowBlanks = false,
                                        ShowResult = false,
                                    }
                            }
                        }
                };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _regionSettingMemoryCache.Setup(x => x.Get(kvp.Key.RegionId))
                                     .Returns(regionSettings)
                                     .Verifiable("There was no method call IRegionSettingsMemoryCache::Get");

            var result = await _examDetailService.GetExamDetails(kvp);
            Assert.IsNull(result);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetExamDetailsTestInCache()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var regionSettings = new RegionSettingsCacheModel
                {
                    Info = new RegionInfoCacheModel
                        {
                            Info = "Информация",
                            HotlinePhone = "43242344",
                        },
                    Servers = new BlanksServerCacheModel
                        {
                            Common = "http\\common",
                            Composition = "http\\composition",
                        },
                    Settings = new Dictionary<int, RegionExamSettingCacheModel>
                        {
                            {
                                7, new RegionExamSettingCacheModel
                                    {
                                        GekDocument = new GekDocumentCacheModel
                                            {
                                                GekDate = new DateTime(2015, 03, 13),
                                                GekNumber = "000634",
                                                Url = "http\\afsdfsdfsdf",
                                            },
                                        ShowBlanks = true,
                                        ShowResult = true,
                                    }
                            }
                        }
                };

            var answerCollection = new AnswerCollectionCacheModel
                {
                    Answers = new Collection<AnswerCacheModel>
                        {
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    Answer = "бла бла",
                                    Mark = 3,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    Answer = "бла бла бла",
                                    Mark = 1,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    Answer = "бла бла бла бла",
                                    Mark = 2,
                                }
                        }
                };

            var examInfo = new ExamInfoCacheModel();

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _regionSettingMemoryCache.Setup(x => x.Get(kvp.Key.RegionId))
                                     .Returns(regionSettings)
                                     .Verifiable("There was no method call IRegionSettingsMemoryCache::Get");

            _answerCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns(answerCollection)
                                  .Verifiable("There was no method call IAnswerCollectionCache::Get");

            _subjectExamMemoryCache.Setup(x => x.GetExam(kvp.Value))
                                   .Returns(new ExamMemoryCacheModel
                                       {
                                           SubjectCode = 20,
                                       })
                                   .Verifiable("There was no method call ISubjectExamMemoryCache::GetExam");

            _cancelledExamsMemoryCache.Setup(x => x.IsHidden(kvp.Key.Code, kvp.Key.RegionId, kvp.Value))
                .Returns(false)
                .Verifiable("There was no method call ICancelledParticipantExamMemoryCache::IsHidden");

            _examInfoMemoryCache.Setup(x => x.Get(20))
                                .Returns(examInfo)
                                .Verifiable("There was no method call IExamInfoMemoryCache::Get");

            var result = await _examDetailService.GetExamDetails(kvp);
            Assert.IsNotNull(result);
            Assert.AreSame(regionSettings.Servers, result.Servers);
            Assert.AreSame(answerCollection, result.Answers);
            Assert.AreSame(examInfo, result.ExamInfo);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetExamDetailsTestWithShowResultNotInCache()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var regionSettings = new RegionSettingsCacheModel
                {
                    Info = new RegionInfoCacheModel
                        {
                            Info = "Информация",
                            HotlinePhone = "43242344",
                        },
                    Servers = new BlanksServerCacheModel
                        {
                            Common = "http\\common",
                            Composition = "http\\composition",
                        },
                    Settings = new Dictionary<int, RegionExamSettingCacheModel>
                        {
                            {
                                7, new RegionExamSettingCacheModel
                                    {
                                        GekDocument = new GekDocumentCacheModel
                                            {
                                                GekDate = new DateTime(2015, 03, 13),
                                                GekNumber = "000634",
                                                Url = "http\\afsdfsdfsdf",
                                            },
                                        ShowBlanks = true,
                                        ShowResult = true,
                                    }
                            }
                        }
                };

            var answerCollection = new AnswerCollectionCacheModel
                {
                    Answers = new Collection<AnswerCacheModel>
                        {
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    Answer = "бла бла",
                                    Mark = 3,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    Answer = "бла бла бла",
                                    Mark = 1,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    Answer = "бла бла бла бла",
                                    Mark = 2,
                                }
                        }
                };

            var examInfo = new ExamInfoCacheModel();

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _regionSettingMemoryCache.Setup(x => x.Get(kvp.Key.RegionId))
                                     .Returns(regionSettings)
                                     .Verifiable("There was no method call IRegionSettingsMemoryCache::Get");

            _answerCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns((AnswerCollectionCacheModel) null)
                                  .Verifiable("There was no method call IAnswerCollectionCache::Get");

            ConnectionFactory.Setup(x => x.CreateAsync())
                             .Returns(() => Task.FromResult(DbConnection))
                             .Verifiable("There was no method call IDbConnectionFactory::CreateAsync");

            _answerRepository.Setup(x => x.GetExamAnswers(DbConnection, kvp))
                             .Returns(Task.FromResult(answerCollection))
                             .Verifiable("There was no method call IAnswerRepository::GetExamAnswers");

            _answerCollectionCache.Setup(x => x.Put(DataCacheObject, kvp, answerCollection))
                                  .Verifiable("There was no method call IAnswerCollectionCache::Put");

            _examInfoMemoryCache.Setup(x => x.Get(20))
                                .Returns(examInfo)
                                .Verifiable("There was no method call IExamInfoMemoryCache::Get");

            _subjectExamMemoryCache.Setup(x => x.GetExam(7))
                                   .Returns(new ExamMemoryCacheModel {SubjectCode = 20})
                                   .Verifiable("There was no method call SubjectExamMemoryCache::GetExam");

            _cancelledExamsMemoryCache.Setup(x => x.IsHidden(kvp.Key.Code, kvp.Key.RegionId, kvp.Value))
                                      .Returns(false)
                                      .Verifiable(
                                          "There was no method call ICancelledParticipantExamMemoryCache::IsHidden");

            var result = await _examDetailService.GetExamDetails(kvp);
            Assert.IsNotNull(result);
            Assert.AreSame(regionSettings.Servers, result.Servers);
            Assert.AreSame(answerCollection, result.Answers);
            Assert.AreSame(examInfo, result.ExamInfo);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetExamDetailsTestInCacheIsHiddenFlag()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
                {
                    Code = "000000000001",
                    Document = string.Empty,
                    Hash = string.Empty,
                    RegionId = 61
                }, 7);

            var regionSettings = new RegionSettingsCacheModel
                {
                    Info = new RegionInfoCacheModel
                        {
                            Info = "Информация",
                            HotlinePhone = "43242344",
                        },
                    Servers = new BlanksServerCacheModel
                        {
                            Common = "http\\common",
                            Composition = "http\\composition",
                        },
                    Settings = new Dictionary<int, RegionExamSettingCacheModel>
                        {
                            {
                                7, new RegionExamSettingCacheModel
                                    {
                                        GekDocument = new GekDocumentCacheModel
                                            {
                                                GekDate = new DateTime(2015, 03, 13),
                                                GekNumber = "000634",
                                                Url = "http\\afsdfsdfsdf",
                                            },
                                        ShowBlanks = true,
                                        ShowResult = true,
                                    }
                            }
                        }
                };

            var answerCollection = new AnswerCollectionCacheModel
                {
                    Answers = new Collection<AnswerCacheModel>
                        {
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    Answer = "бла бла",
                                    Mark = 3,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    Answer = "бла бла бла",
                                    Mark = 1,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    Answer = "бла бла бла бла",
                                    Mark = 2,
                                }
                        },
                    IsHidden = true,
                };

            var examInfo = new ExamInfoCacheModel();

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _regionSettingMemoryCache.Setup(x => x.Get(kvp.Key.RegionId))
                                     .Returns(regionSettings)
                                     .Verifiable("There was no method call IRegionSettingsMemoryCache::Get");

            _answerCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns(answerCollection)
                                  .Verifiable("There was no method call IAnswerCollectionCache::Get");

            _examInfoMemoryCache.Setup(x => x.Get(20))
                                .Returns(examInfo)
                                .Verifiable("There was no method call IExamInfoMemoryCache::Get");

            _cancelledExamsMemoryCache.Setup(x => x.IsHidden(kvp.Key.Code, kvp.Key.RegionId, kvp.Value))
                .Returns(true)
                .Verifiable("There was no method call ICancelledParticipantExamMemoryCache::IsHidden");


            _subjectExamMemoryCache.Setup(x => x.GetExam(7))
                                   .Returns(new ExamMemoryCacheModel { SubjectCode = 20 })
                                   .Verifiable("There was no method call SubjectExamMemoryCache::GetExam");

            var result = await _examDetailService.GetExamDetails(kvp);
            Assert.IsNull(result);
            MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetExamDetailsTestInCacheWithBlanks()
        {
            var kvp = new KeyValuePair<ParticipantCacheModel, int>(new ParticipantCacheModel
            {
                Code = "000000000001",
                Document = string.Empty,
                Hash = string.Empty,
                RegionId = 61,
            }, 7);

            var regionSettings = new RegionSettingsCacheModel
            {
                Info = new RegionInfoCacheModel
                {
                    Info = "Информация",
                    HotlinePhone = "43242344",
                },
                Servers = new BlanksServerCacheModel
                {
                    Common = "http\\common",
                    Composition = "http\\composition",
                },
                Settings = new Dictionary<int, RegionExamSettingCacheModel>
                        {
                            {
                                7, new RegionExamSettingCacheModel
                                    {
                                        GekDocument = new GekDocumentCacheModel
                                            {
                                                GekDate = new DateTime(2015, 03, 13),
                                                GekNumber = "000634",
                                                Url = "http\\afsdfsdfsdf",
                                            },
                                        ShowBlanks = true,
                                        ShowResult = true,
                                    }
                            }
                        }
            };

            var answerCollection = new AnswerCollectionCacheModel
            {
                Answers = new Collection<AnswerCacheModel>
                        {
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 1,
                                    Answer = "бла бла",
                                    Mark = 3,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 2,
                                    Answer = "бла бла бла",
                                    Mark = 1,
                                },
                            new AnswerCacheModel
                                {
                                    Type = TaskType.A,
                                    Number = 3,
                                    Answer = "бла бла бла бла",
                                    Mark = 2,
                                }
                        },
                IsHidden = false,
                Blanks = new Collection<BlankCacheModel>
                    {
                        new BlankCacheModel
                            {
                                Barcode = "fdsfsdf",
                                BlankType = 3,
                                PageCount = 5
                            }
                    }
            };

            var examInfo = new ExamInfoCacheModel();

            var blanksModel = new List<BlankClientModel>
                {
                    new BlankClientModel(),
                    new BlankClientModel(),
                };

            CacheFactory.Setup(x => x.GetCache())
                        .Returns(DataCacheObject)
                        .Verifiable("There was no method call CacheFactory::GetCache");

            _regionSettingMemoryCache.Setup(x => x.Get(kvp.Key.RegionId))
                                     .Returns(regionSettings)
                                     .Verifiable("There was no method call IRegionSettingsMemoryCache::Get");

            _answerCollectionCache.Setup(x => x.Get(DataCacheObject, kvp))
                                  .Returns(answerCollection)
                                  .Verifiable("There was no method call IAnswerCollectionCache::Get");

            _examInfoMemoryCache.Setup(x => x.Get(20))
                                .Returns(examInfo)
                                .Verifiable("There was no method call IExamInfoMemoryCache::Get");

            _subjectExamMemoryCache.Setup(x => x.GetExam(7))
                                   .Returns(new ExamMemoryCacheModel { SubjectCode = 20 })
                                   .Verifiable("There was no method call SubjectExamMemoryCache::GetExam");

            _blankModelCreator.Setup(x => x.Create(answerCollection.Blanks, It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<NameCoderBlank>()))
                       .Returns(blanksModel)
                       .Verifiable("There was no method call BlankModelCreator::Create");

            _cancelledExamsMemoryCache.Setup(x => x.IsHidden(kvp.Key.Code, kvp.Key.RegionId, kvp.Value))
                .Returns(false)
                .Verifiable("There was no method call ICancelledParticipantExamMemoryCache::IsHidden");

            var result = await _examDetailService.GetExamDetails(kvp);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Answers);
            Assert.IsTrue(blanksModel.SequenceEqual(result.Answers.BlanksWithUrls));
            MockRepository.VerifyAll();
        }
    }
}