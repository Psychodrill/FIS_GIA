using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using FogSoft.Helpers;
using FogSoft.WSRP;
using FogSoft.WSRP.Factories;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.FileStorage;
using GVUZ.Model.Helpers;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.Web.Filters;
using GVUZ.Web.Models.Account;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using ISession = FogSoft.Helpers.ISession;
using GVUZ.Model.Cache;
using GVUZ.Web.Models;

namespace GVUZ.Web
{
	// For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : HttpApplication
	{
		// ReSharper disable InconsistentNaming
        public static Dictionary<string, DateTime> ConnectedtUsers { get; set; }
		public static void RegisterRoutes(RouteCollection routes)
		{
            routes.IgnoreRoute("Resources/images/{*pathInfo}");
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			//routes.IgnoreRoute("{*favicon.ico}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
			routes.IgnoreRoute("import/importservice.svc");
			routes.IgnoreRoute("import/importservice.svc/{*pathInfo}");

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Account", action = "DefaultPage", id = UrlParameter.Optional }, // Parameter defaults
					new[] { "GVUZ.Web.Controllers" });
		}

		private UnityContainer _unityContainer;

		protected void Application_Start()
        {
            ConnectedtUsers = new Dictionary<string, DateTime>();

            GVUZ.DAL.Dapper.Repository.Model.GvuzRepository.Initialize(ConfigurationManager.ConnectionStrings["Main"].ConnectionString);

			_unityContainer = new UnityContainer();
			_unityContainer.RegisterInstance<IConfigurationService>(new ConfigurationService());
			_unityContainer.RegisterInstance<ISession>(new HttpSession());
            _unityContainer.RegisterInstance<ICache>(new Cache());

			_unityContainer.RegisterInstance<IPortletFactory>(new AttributedPortletFactory());
			//_unityContainer.RegisterInstance<IImportServiceSettings>(new WebServiceImportSettings());
			_unityContainer.RegisterInstance<IEgeInformationProvider>(new EgeInformationProvider());
			//_unityContainer.RegisterInstance<IEgeInformationProvider>(new EmulatorEgeInformationProvider());
            _unityContainer.RegisterInstance<IFileStorage>(new FileStorage(ConfigurationManager.AppSettings["OlympicFileStorage"]));
			ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_unityContainer));

			FogSoft.Helpers.Resources.ResetManagerMap();
			FogSoft.Helpers.Resources.Map<MembershipCreateStatus>(() => Messages.ResourceManager);
			FogSoft.Helpers.Resources.Map<InstitutionCreationError>(() => Messages.ResourceManager);
			
			LogHelper.InitLoging();

			Mapper.Reset();

			AutoMapperHelper.InitializeMapper(new[] { typeof(MvcApplication).Assembly });

			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			// encode application/json request
			// ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

			//ModelBinders.Binders.Add(typeof(DateTime), new ModelBinder());
			DefaultModelBinder.ResourceClassKey = "ErrorStrings";

			PackageManager.Initialize();
            BulkEntitesMapper.Initialize();
			//стартуем обработик, как будет отдельный сервис, отсюда нужно убрать
            //if (AppSettings.Get("RunImportProcessorInsideApp", true))
            //{
            //    BulkEntitesMapper.Initialize();
            //    ProcessingManager.Start();
            //}
            GlobalFilters.Filters.Add(new TransactionFilterAttribute());

            // Model Binding for decimal
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

        }

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception lastError = Server.GetLastError();
			LogHelper.LogCurrentDomainUnhandledException(sender, new UnhandledExceptionEventArgs(lastError, false));
    	}

		protected void Application_End(object sender, EventArgs e)
		{
			LogHelper.LogHttpRuntimeShutdownReason();
		}

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //HttpRuntime.Cache.

            if ((HttpRuntime.Cache != null) && (HttpRuntime.Cache.Get("CurrentUser") == null))
            {
                Context.User = UserHelper.ReplaceIdentity(Context.User);
                if (Context.User != null)
                    HttpRuntime.Cache.Insert("CurrentUser", Context.User.Identity.Name);
            } 
        }

		protected void Application_AuthorizeRequest(object sender, EventArgs e)
		{

		}
	}
}