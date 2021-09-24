namespace Ege.Check.App.Web.Tests.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;
    using Ege.Check.App.Services.Ninject;
    using Ege.Check.App.Web.Api.Participant;
    using Ege.Check.App.Web.Blanks.Controllers;
    using Ege.Check.App.Web.Blanks.Ninject;
    using Ege.Check.App.Web.Ninject;
    using Ege.Check.Logic.Services;
    using Ege.Check.Logic.Services.Staff.Users;
    using Ege.Hsc.Scheduler.Jobs;
    using Ege.Hsc.Scheduler.Ninject;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::Ninject;
    using Quartz;

    [TestClass]
    public class WebAppKernelFactoryTests
    {
        [TestMethod]
        public void CreateKernelTest()
        {
            KernelTestHelper.CheckControllers(WebAppKernelFactory.CreateKernel(), typeof (RegionController),
                                              new[] {typeof (IUserService)});
        }
    }

    [TestClass]
    public class ServicesAppKernelTests
    {
        [TestMethod]
        public void KernelTest()
        {
            var kernel = KernelContainer.Kernel;
            kernel.Get<EgeDataService>();
        }
    }

    [TestClass]
    public class BlanksAppKernelTests
    {
        [TestMethod]
        public void KernelTest()
        {
            KernelTestHelper.CheckControllers(BlanksWebAppKernelFactory.CreateKernel(), typeof (AuthController),
                                              new Type[0]);
        }
    }

    [TestClass]
    public class SchedulerKernelTests
    {
        [TestMethod]
        public void KernelTest()
        {
            KernelTestHelper.CheckJobs(NinjectKernelFactory.CreateKernel(), typeof(DownloadBlanksJob), new Type[0]);
        }
    }
    
    public static class KernelTestHelper
    {
        public static void FailIfThereAreExceptions([NotNull] IList<string> exceptionMessages)
        {
            if (exceptionMessages.Any())
            {
                Assert.Fail(string.Join("\n***************************************************************************\n\n",
                                Enumerable.Repeat("", 1).Concat(exceptionMessages)));
            }
        }

        public static void CheckControllers(IKernel kernel, Type assemblyType, Type[] andTypes)
        {
            CheckSubtypes(kernel, assemblyType, andTypes, new [] {typeof(ApiController), typeof(Controller)});
        }

        public static void CheckJobs(IKernel kernel, Type assemblyType, Type[] andTypes)
        {
            CheckSubtypes(kernel, assemblyType, andTypes, new [] {typeof(IJob)});
        }

        public static void CheckSubtypes(IKernel kernel, Type assemblyType, Type[] andTypes, Type[] checkedBaseTypes)
        {
            var exceptions = new List<string>();

            var assembly = Assembly.GetAssembly(assemblyType);
            var controllerTypes =
                assembly.GetTypes()
                        .Where(type => checkedBaseTypes.Any(ct => ct.IsAssignableFrom(type) && !type.IsAbstract));
            foreach (var controllerType in controllerTypes.Concat(andTypes))
            {
                try
                {
                    kernel.Get(controllerType);
                }
                catch (ActivationException ae)
                {
                    exceptions.Add(ae.Message);
                }
            }
            FailIfThereAreExceptions(exceptions);
        }
    }
}