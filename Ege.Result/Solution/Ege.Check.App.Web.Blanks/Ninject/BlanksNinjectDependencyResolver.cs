namespace Ege.Check.App.Web.Blanks.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using global::Ninject;

    public class BlanksNinjectDependencyResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        public static readonly IKernel Kernel;

        static BlanksNinjectDependencyResolver()
        {
            Kernel = BlanksWebAppKernelFactory.CreateKernel();
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(Kernel);
        }

        public void Dispose()
        {
        }
    }
}