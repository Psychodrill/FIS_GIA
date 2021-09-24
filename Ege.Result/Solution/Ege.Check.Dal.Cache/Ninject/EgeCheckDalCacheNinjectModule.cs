namespace Ege.Check.Dal.Cache.Ninject
{
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.Cache.AppealCollection;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Captcha;
    using Ege.Check.Dal.Cache.ExamCollection;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices;
    using Ege.Check.Dal.Cache.LoadServices.DtoCache;
    using Ege.Check.Dal.Cache.LoadServices.Helpers;
    using Ege.Check.Dal.Cache.Participants;
    using Ege.Check.Dal.Cache.StaffUsers;
    using Ege.Check.Logic.Services.Dtos.Models;
    using global::Ninject.Modules;

    public class EgeCheckDalCacheNinjectModule<T> : NinjectModule
        where T: ICacheFactory
    {
        public override void Load()
        {
            Bind<ICacheFactory>().To<T>().InSingletonScope();

            Bind<ICacheSerializer>().To<NewtonsoftJsonCacheSerializer>().InSingletonScope();

            Bind<ICacheSettingsProvider>().To<CacheSettingsProvider>().InSingletonScope();

            Bind<IParticipantCache>().To<ParticipantCache>().InSingletonScope();
            Bind<IExamCollectionCache>().To<ExamCollectionCache>().InSingletonScope();
            Bind<IAnswerCollectionCache>().To<AnswerCollectionCache>().InSingletonScope();
            Bind<IAppealCollectionCache>().To<AppealCollectionCache>().InSingletonScope();

            Bind<ICaptchaCache>().To<CaptchaCache>().InSingletonScope();

            Bind<IStaffUserCache>().To<StaffUserCache>().InSingletonScope();

            Bind<ICacheFailureHelper>().To<CacheFailureHelper>().InSingletonScope();

            Bind<IParticipantLookupCreator>().To<ParticipantLookupCreator>().InSingletonScope();

            Bind(typeof (ICacheWriter<>)).To<NopCacheWriter>().InSingletonScope();
            Bind<ICacheWriter<ParticipantDto>>().To<ParticipantCacheWriter>().InSingletonScope();
            Bind<ICacheWriter<ParticipantExamDto>>().To<ParticipantExamCacheWriter>().InSingletonScope();
            Bind<ICacheWriter<AnswerDto>>().To<AnswerCacheWriter>().InSingletonScope();
            Bind<ICacheWriter<AppealDto>>().To<AppealCacheWriter>().InSingletonScope();
            Bind<ICacheWriter<BlankInfoDto>>().To<BlankInfoCacheWriter>().InSingletonScope();

            Bind<ICacheLockAcquirer>().To<CacheLockAcquirer>().InSingletonScope();
            Bind(typeof(IDtoCache<>)).To(typeof(DtoCache<>)).InSingletonScope();
            Bind<IBulkProcessor>().To<BulkProcessor>().InSingletonScope();

            Bind<IBlankInfoCacheUpdater>().To<BlankInfoCacheWriter>().InSingletonScope();
        }
    }
}
