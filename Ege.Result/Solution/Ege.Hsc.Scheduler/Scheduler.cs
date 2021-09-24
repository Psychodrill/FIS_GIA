namespace Ege.Hsc.Scheduler
{
    using System;
    using System.ServiceProcess;
    using Common.Logging;
    using JetBrains.Annotations;
    using global::Ninject;
    using global::Ege.Hsc.Scheduler.Ninject;
    using global::Ege.Hsc.Scheduler.Scheduling;

    public partial class Scheduler : ServiceBase
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<Scheduler>();
        [NotNull] private readonly ISchedulingManager _schedulingManager;

        public Scheduler()
        {
            Logger.Trace("Create scheduler service");
            InitializeComponent();
            try
            {
                var manager = NinjectRetriever.Kernel.Get<ISchedulingManager>();
                _schedulingManager = manager;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
            
        }

        public void Run()
        {
            Run(this);
        }

        public void RunConsole()
        {
            Console.WriteLine("Create scheduler service");
            Logger.Trace("Create scheduler service");
            _schedulingManager.StartScheduler();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Trace("Service started");
            _schedulingManager.StartScheduler();
        }

        protected override void OnStop()
        {
            _schedulingManager.StopScheduler();
            Logger.Trace("Service stopped");
        }
    }
}