namespace Ege.Check.Logic.Ninject
{
    using Ege.Check.Logic.BlankServers;
    using Ege.Check.Logic.Helpers;
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Ege.Check.Logic.LoadServices.Processing;
    using Ege.Check.Logic.Services.Participant;
    using Ege.Check.Logic.Services.Participant.Appeals;
    using Ege.Check.Logic.Services.Participant.ExamDetails;
    using Ege.Check.Logic.Services.Participant.ExamLists;
    using Ege.Check.Logic.Services.Participant.Participants;
    using Ege.Check.Logic.Services.Participant.Regions;
    using Ege.Check.Logic.Services.Staff.Documents;
    using Ege.Check.Logic.Services.Staff.ExamCancellation;
    using Ege.Check.Logic.Services.Staff.Exams;
    using Ege.Check.Logic.Services.Staff.Rcoi;
    using Ege.Check.Logic.Services.Staff.Settings;
    using Ege.Check.Logic.Services.Staff.TaskSettings;
    using Ege.Check.Logic.Services.Staff.Users;
    using Ege.Dal.Common.Helpers;
    using global::Ninject.Modules;

    public class EgeCheckLogicNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegionService>().To<RegionService>().InSingletonScope();
            Bind<IParticipantService>().To<ParticipantService>().InSingletonScope();
            Bind<IExamListService>().To<ExamListService>().InSingletonScope();
            Bind<IExamDetailsService>().To<ExamDetailsService>().InSingletonScope();
            Bind<IUserService>().To<UserService>().InSingletonScope();
            Bind<IExamSettingService>().To<ExamSettingService>().InSingletonScope();
            Bind<IExamCancellationService>().To<ExamCancellationService>().InSingletonScope();
            Bind<IRcoiInfoService>().To<RcoiInfoService>().InSingletonScope();
            Bind<ISubjectExamService>().To<SubjectExamService>().InSingletonScope();
            Bind<IDocumentUrlService>().To<DocumentUrlService>().InSingletonScope();
            Bind<IAppealService>().To<AppealService>().InSingletonScope();
            Bind<IAnswerCriteriaService>().To<AnswerCriteriaService>().InSingletonScope();
            Bind<IMemoryCacheService>().To<MemoryCacheService>().InSingletonScope();

            Bind<IPasswordHasher>().To<PasswordHasher>().InSingletonScope();
            Bind<IUrlCorrector>().To<UrlCorrector>().InSingletonScope();

            Bind<IBatchSizeSettingsReader>().To<ConfigBatchSizeSettingsReader>().InSingletonScope();
            Bind<IDecompressor>().To<Decompressor>().InSingletonScope();
            Bind<IDeserializer>().To<Deserializer>().InSingletonScope();
            Bind<IProcedureNameGetter>().To<AttributeProcedureNameGetter>().InSingletonScope();
            Bind<IExpressionHelper>().To<ExpressionHelper>().InSingletonScope();
            Bind(typeof (IDatatableCollector<>)).To(typeof (DatatableCollector<>)).InSingletonScope();

            Bind<IPageCountDataParser>().To<PageCountDataParser>().InSingletonScope();
            Bind<IPageCountFileParser>().To<PageCountFileParser>().InSingletonScope();
            Bind<IPageCountRetriever>().To<PageCountRetriever>().InSingletonScope();
            Bind<IServerPageCountService>().To<ServerPageCountService>().InSingletonScope();

            Bind<ICacheUpdaterService>().To<CacheUpdaterService>().InSingletonScope();
            Bind<IExamListFilterHelper>().To<ExamListFilterHelper>().InSingletonScope();
        }
    }
}