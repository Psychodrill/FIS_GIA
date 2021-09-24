using System.ServiceProcess;

namespace GVUZ.AppExport.Service
{
    public partial class ServiceApplication : ServiceBase
    {
        private readonly ServiceLauncher _launcher;

        public ServiceApplication()
        {
            InitializeComponent();
            _launcher = new ServiceLauncher();
        }

        protected override void OnStart(string[] args)
        {
            _launcher.Start();
        }

        protected override void OnStop()
        {
            //RequestAdditionalTime(300000);
            _launcher.Stop();
        }
    }
}
