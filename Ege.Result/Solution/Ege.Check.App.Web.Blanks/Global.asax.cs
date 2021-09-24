namespace Ege.Check.App.Web.Blanks
{
    using System.Web.Http.Dependencies;
    using Ege.Check.App.Web.Blanks.Ninject;
    using Ege.Check.App.Web.Common;

    public class CheckEgeBlanksApplication : CheckEgeCommonWebApplication
    {
        protected override IDependencyResolver CreateResolver()
        {
            return new BlanksNinjectDependencyResolver();
        }

        protected override System.Web.Mvc.IDependencyResolver CreateMvcResolver()
        {
            return new BlanksNinjectDependencyResolver();
        }

        protected override bool CustomAuthentication
        {
            get { return false; }
        }
    }
}