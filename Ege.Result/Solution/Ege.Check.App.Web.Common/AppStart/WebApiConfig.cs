namespace Ege.Check.App.Web.Common.AppStart
{
    using System;
    using System.Web.Http;
    using Ege.Check.App.Web.Common.Filters;
    using Ege.Check.App.Web.Common.Formatters;
    using Ege.Check.App.Web.ModelBinders;
    using JetBrains.Annotations;

    public static class WebApiConfig
    {
        public static void Register([NotNull] HttpConfiguration config, bool customAuthentication)
        {
            config.MapHttpAttributeRoutes();
            if (customAuthentication)
            {
                config.SuppressHostPrincipal();
            }
            if (config.Filters == null)
            {
                throw new InvalidOperationException("config.Filters is null");
            }
            config.Filters.Add(new CheckEgeExceptionFilterAttribute());
            config.BindParameter(typeof (DateTime), new DateTimeModelBinder());
            config.BindParameter(typeof (DateTime?), new DateTimeModelBinder());
            if (config.Formatters == null)
            {
                throw new InvalidOperationException("config.Formatters is null");
            }
            //config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(new ServiceStackTextJsonMediaFormatter());
            config.EnsureInitialized();
        }
    }
}