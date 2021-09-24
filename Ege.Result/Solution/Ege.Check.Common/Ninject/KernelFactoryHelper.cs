namespace Ege.Check.Common.Ninject
{
    using global::Ninject;
    using global::Ninject.Modules;

    public static class KernelFactoryHelper
    {
        public static IKernel CreateKernel(params INinjectModule[] modules)
        {
            var kernel = new StandardKernel(modules);
            return kernel;
        }
    }
}