namespace Ege.Check.Dal.Store.Ninject
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Store.Bulk;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Dal.Store.Mappers.Staff;
    using Ege.Check.Dal.Store.Repositories.Answers;
    using Ege.Check.Dal.Store.Repositories.Appeals;
    using Ege.Check.Dal.Store.Repositories.Blanks;
    using Ege.Check.Dal.Store.Repositories.Documents;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Dal.Store.Repositories.PagesCount;
    using Ege.Check.Dal.Store.Repositories.Participants;
    using Ege.Check.Dal.Store.Repositories.Regions;
    using Ege.Check.Dal.Store.Repositories.Users;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;
    using global::Ninject.Modules;

    public class EgeCheckDalStoreNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataReaderCollectionMapper<AvailableRegion>>()
                .To<AvailableRegionCollectionMapper>()
                .InSingletonScope();
            Bind<IDataReaderMapper<ParticipantCollectionCacheModel>>()
                .To<ParticipantCollectionMapper>()
                .InSingletonScope();
            Bind<IDataReaderMapper<ExamCollectionCacheModel>>().To<ExamCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<AnswerCollectionCacheModel>>().To<AnswerCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<ExamInfoCacheModel>>().To<AnswerCriteriaCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<IDictionary<int, ExamInfoCacheModel>>>()
                .To<AnswerCriteriaCollectionMapper>()
                .InSingletonScope();
            Bind<IDataReaderMapper<IDictionary<int, RegionSettingsCacheModel>>>()
                .To<RegionSettingsMapper>()
                .InSingletonScope();
            Bind<IDataReaderMapper<UserModel>>().To<UserMapper>().InSingletonScope();
            Bind<IDataReaderMapper<ExamSettings>>().To<ExamSettingsMapper>().InSingletonScope();
            Bind<IDataReaderMapper<GekDocument>>().To<GekDocumentMapper>().InSingletonScope();
            Bind<IDataReaderMapper<RcoiInfo>>().To<RcoiInfoMapper>().InSingletonScope();
            Bind<IDataReaderMapper<DocumentUrlsCollection>>().To<DocumentUrlsCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<CancelledExamsPage>>().To<CancelledExamMapper>().InSingletonScope();
            Bind<IDataReaderMapper<AppealCollectionCacheModel>>().To<AppealCollectionMapper>().InSingletonScope();
            Bind<IDataReaderMapper<UserDtoPage>>().To<UserDtoMapper>().InSingletonScope();
            Bind<IDataReaderMapper<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>>>()
                .To<ExamListMapper>()
                .InSingletonScope();

            Bind<IDataTableMapper<KeyValuePair<int, IEnumerable<ExamSetting>>>>()
                .To<ExamSettingsDataTableMapper>()
                .InSingletonScope();
            Bind<IDataTableMapper<IEnumerable<DocumentUrl>>>().To<DocumentUrlsDataTableMapper>().InSingletonScope();
            Bind<IDataTableMapper<ExamInfoCacheModel>>().To<TaskSettingsDataTableMapper>().InSingletonScope();

            Bind<IDataReaderSyncMapper<UserModel>>().To<UserMapper>().InSingletonScope();

            Bind<IRegionRepository>().To<RegionRepository>().InSingletonScope();
            Bind<IParticipantRepository>().To<ParticipantRepository>().InSingletonScope();
            Bind<IParticipantExamRepository>().To<ParticipantExamRepository>().InSingletonScope();
            Bind<IAnswerRepository>().To<AnswerRepository>().InSingletonScope();
            Bind<IAnswerCriteriaRepository>().To<AnswerCriteriaRepository>().InSingletonScope();
            Bind<IRegionSettingsRepository>().To<RegionSettingsRepository>().InSingletonScope();
            Bind<IUserRepository>().To<UserRepository>().InSingletonScope();
            Bind<IRcoiInfoRepository>().To<RcoiInfoRepository>().InSingletonScope();
            Bind<IExamCancellationRepository>().To<ExamCancellationRepository>().InSingletonScope();
            Bind<ISubjectExamRepository>().To<SubjectExamRepository>().InSingletonScope();
            Bind<IDocumentUrlRepository>().To<DocumentUrlRepository>().InSingletonScope();
            Bind<IAppealRepository>().To<AppealRepository>().InSingletonScope();

            Bind<ITableSqlGenerator>().To<TableSqlGenerator>().InSingletonScope();
            Bind<IEgeTempTableOperator>().To<EgeTempTableOperator>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<CancelledParticipantExam>>().To<CancelledParticipantExamCollectionMapper>().InSingletonScope();

            Bind<IDataTableMapper<IEnumerable<PageCountData>>>().To<PageCountDataMapper>().InSingletonScope();
            Bind<IBlankInfoRepository>().To<BlankInfoRepository>().InSingletonScope();
            Bind<IDataReaderCollectionMapper<UpdatedBlankInfo>>().To<UpdatedBlankCollectionMapper>().InSingletonScope();

            Bind<IPagesCountRepository>().To<PagesCountRepository>().InSingletonScope();
        }
    }
}