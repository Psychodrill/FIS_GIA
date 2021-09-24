namespace Ege.Check.App.Services
{
    using System;
    using Ege.Check.App.Services.Ninject;
    using global::Common.Logging;
    using global::Ninject;
    using global::Ninject.Web.Common;
    using JetBrains.Annotations;

    public class Global : NinjectHttpApplication
    {
        [NotNull]
        protected readonly static ILog Logger = LogManager.GetLogger<Global>();

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logger.InfoFormat("Application ended");
        }

        protected override IKernel CreateKernel()
        {
            return KernelContainer.Kernel;
        }
    }
}