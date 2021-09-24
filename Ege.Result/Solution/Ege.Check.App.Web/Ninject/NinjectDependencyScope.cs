namespace Ege.Check.App.Web.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using global::Ninject;
    using global::Ninject.Syntax;

    public class NinjectDependencyScope : IDependencyScope
    {
        private readonly IResolutionRoot _resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return _resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
        }
    }
}