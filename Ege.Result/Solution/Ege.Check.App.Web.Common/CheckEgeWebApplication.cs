namespace Ege.Check.App.Web.Common
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Ege.Check.App.Web.Common.AppStart;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using global::Common.Logging;
    using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

    public abstract class CheckEgeCommonWebApplication : HttpApplication
    {
        [NotNull] private static readonly CultureInfo RuCulture;
        [NotNull] protected readonly ILog Logger;

        static CheckEgeCommonWebApplication()
        {
            RuCulture = CultureInfo.CreateSpecificCulture("ru-RU");
        }

        protected CheckEgeCommonWebApplication()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        protected void Application_Start()
        {
            Logger.Trace("Application started");
            if (GlobalConfiguration.Configuration == null)
            {
                throw new InvalidOperationException("GlobalConfiguration.Configuration is null");
            }
            SerializeSettings(GlobalConfiguration.Configuration);
            RegisterWebApi(GlobalConfiguration.Configuration);
            RegisterMvc(RouteTable.Routes);
            DependencyResolver.SetResolver(CreateMvcResolver());
            GlobalConfiguration.Configuration.DependencyResolver = CreateResolver();
            Start();
        }

        protected virtual void Start()
        {
        }

        protected void Application_End()
        {
            Logger.Trace("Application ended");
        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = RuCulture;
            Thread.CurrentThread.CurrentUICulture = RuCulture;
        }

        protected virtual void SerializeSettings([NotNull] HttpConfiguration config)
        {
            var jsonSetting = new JsonSerializerSettings();
            if (jsonSetting.Converters == null)
            {
                throw new InvalidOperationException("jsonSetting.Converters is null");
            }
            if (config.Formatters == null)
            {
                throw new InvalidOperationException("config.Formatters is null");
            }
            if (config.Formatters.JsonFormatter == null)
            {
                throw new InvalidOperationException("config.Formatters.JsonFormatter is null");
            }
            jsonSetting.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;
        }

        protected abstract IDependencyResolver CreateResolver();
        protected abstract System.Web.Mvc.IDependencyResolver CreateMvcResolver();

        protected virtual void RegisterWebApi([NotNull] HttpConfiguration config)
        {
            WebApiConfig.Register(config, CustomAuthentication);
        }

        protected virtual void RegisterMvc(RouteCollection routes)
        {
            RouteConfig.RegisterRoutes(routes);
        }

        protected abstract bool CustomAuthentication { get; }
    }
}