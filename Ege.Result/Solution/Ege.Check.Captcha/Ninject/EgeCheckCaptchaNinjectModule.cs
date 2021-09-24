namespace Ege.Check.Captcha.Ninject
{
    using global::Ninject.Modules;

    public class EgeCheckCaptchaNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICaptchaGenerator>().To<CaptchaGenerator>().InSingletonScope();
            Bind<ICaptchaRetriever>().To<CaptchaRetriever>().InSingletonScope();
            Bind<ICaptchaTokenHelper>().To<CaptchaTokenHelper>().InSingletonScope();
            Bind<ICaptchaService>().To<InMemoryDbCacheCaptchaService>().InSingletonScope();
            Bind<ICaptchaCacheSettingsProvider>().To<CaptchaCacheSettingsProvider>().InSingletonScope();
        }
    }
}