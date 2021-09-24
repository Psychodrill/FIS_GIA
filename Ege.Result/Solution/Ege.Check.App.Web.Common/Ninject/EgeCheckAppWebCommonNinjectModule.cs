using Ege.Check.App.Web.Common.ReCapture;

namespace Ege.Check.App.Web.Common.Ninject
{
    using Ege.Check.App.Web.Common.Auth;
    using global::Ninject.Modules;

    public class EgeCheckAppWebCommonNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthCookieCreator>().To<AuthCookieCreator>().InSingletonScope();
            Bind<IRecaptchaService>().To<GoogleRecaptchaService>();
        }
    }
}
