namespace Ege.Check.Dal.MemoryCache.Ninject
{
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Dal.MemoryCache.ExamInfo;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using global::Ninject.Modules;

    public class EgeCheckDalMemoryCacheNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRegionMemoryCache>().To<RegionMemoryCache>().InSingletonScope();
            Bind<IExamInfoMemoryCache>().To<ExamInfoMemoryCache>().InSingletonScope();
            Bind<IRegionSettingsMemoryCache>().To<RegionSettingsMemoryCache>().InSingletonScope();
            Bind<ISubjectExamMemoryCache>().To<SubjectExamMemoryCache>().InSingletonScope();
            Bind<ICancelledParticipantExamMemoryCache>().To<CancelledParticipantExamMemoryCache>().InSingletonScope();

        }
    }
}