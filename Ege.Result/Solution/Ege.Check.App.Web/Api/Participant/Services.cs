using Common.Logging;
using Ege.Check.App.Web.Common.ReCapture;

namespace Ege.Check.App.Web.Api.Participant
{
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Check.App.Web.Ninject;
    using Ege.Check.Captcha;
    using Ege.Check.Logic.Services.Participant.Appeals;
    using Ege.Check.Logic.Services.Participant.ExamDetails;
    using Ege.Check.Logic.Services.Participant.ExamLists;
    using Ege.Check.Logic.Services.Participant.Participants;
    using Ege.Check.Logic.Services.Participant.Regions;
    using global::Ninject;
    using JetBrains.Annotations;

    public static class Services
    {
        [NotNull] public static IRegionService RegionService = NinjectDependencyResolver.Kernel.Get<IRegionService>();

        [NotNull] public static ICaptchaRetriever CaptchaRetriever = NinjectDependencyResolver.Kernel.Get<ICaptchaRetriever>();

        [NotNull] public static IAuthCookieCreator AuthCookieCreator =
            NinjectDependencyResolver.Kernel.Get<IAuthCookieCreator>();

        [NotNull] public static ICaptchaTokenHelper CaptchaTokenHelper =
            NinjectDependencyResolver.Kernel.Get<ICaptchaTokenHelper>();

        [NotNull] public static IParticipantService ParticipantService =
            NinjectDependencyResolver.Kernel.Get<IParticipantService>();

        [NotNull] public static IExamListService ExamListService =
            NinjectDependencyResolver.Kernel.Get<IExamListService>();

        [NotNull] public static IExamDetailsService ExamDetailsService =
            NinjectDependencyResolver.Kernel.Get<IExamDetailsService>();

        [NotNull] public static IAppealService AppealService = NinjectDependencyResolver.Kernel.Get<IAppealService>();

        [NotNull] public static IRecaptchaService RecaptchaService = NinjectDependencyResolver.Kernel.Get<IRecaptchaService>();

    }
}
