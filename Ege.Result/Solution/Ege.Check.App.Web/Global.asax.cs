namespace Ege.Check.App.Web
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Dependencies;
    using System.Web.Routing;
    using Ege.Check.App.Web.Common;
    using Ege.Check.App.Web.Ninject;
    using Ege.Check.Captcha;
    using Ege.Check.Common;
    using Ege.Check.Logic.Services.Participant;
    using global::Ninject;

    public class CheckEgeWebApplication : CheckEgeCommonWebApplication
    {
        protected override void Start()
        {
            var memoryCacheService = NinjectDependencyResolver.Kernel.Get<IMemoryCacheService>();
            if (memoryCacheService == null)
            {
                throw new InvalidOperationException("Dependency resolver returned null instead of IMemoryCacheService");
            }
            if (Task.Factory == null)
            {
                throw new InvalidOperationException("Task.Factory is null");
            }
            Task.Factory.StartNew(
                () => PeriodicTask.Run(async () => await memoryCacheService.RefreshMemoryCache(), TimeSpan.FromMinutes(1)));

            var captchaCacheGenerator = NinjectDependencyResolver.Kernel.Get<ICaptchaService>();
            if (captchaCacheGenerator == null)
            {
                throw new InvalidOperationException("Dependency resolver returned null instead of ICaptchaService");
            }
            if (captchaCacheGenerator.Generatable)
            {
                Task.Factory.StartNew(
                    () => PeriodicTask.Run(captchaCacheGenerator.TryGenerateBatch, TimeSpan.FromMinutes(1)));
            }
            base.Start();
        }

        protected override void RegisterMvc(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            RouteConfig.RegisterRoutes(routes);
            base.RegisterMvc(routes);
        }

        protected override IDependencyResolver CreateResolver()
        {
            return new NinjectDependencyResolver();
        }

        protected override System.Web.Mvc.IDependencyResolver CreateMvcResolver()
        {
            return new NinjectDependencyResolver();
        }

        protected override bool CustomAuthentication
        {
            get { return true; }
        }
    }
}