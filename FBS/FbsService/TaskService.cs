using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Reflection;
using System.IO;
using System.ServiceModel;

namespace FbsService
{
    public partial class TaskService : ServiceBase
    {
        private TimeSpan ThreadStoppingTimeout = TimeSpan.Parse("00:00:03");
        private TimeSpan ThreadStoppingCheckTimeout = TimeSpan.Parse("00:00:00.050");
        private const string LoggerName = "Service";

        private static bool mServiceStopping = false;
        
        internal static int ConfigThreadCount = 10;
        internal static TimeSpan ConfigThreadIdleTimeout = new TimeSpan(0, 0, 1);
        internal static string BulkFileDirectory;
        
        private static log4net.ILog mLogger = null;
        internal static string ArchiveFileDirectory;

        public static log4net.ILog Logger
        {
            get
            {
                return mLogger;
            }
        }

        public TaskService()
        {
            InitializeComponent();

            // Прочитать конфигурацию
            log4net.Config.XmlConfigurator.Configure();
            LoadConfiguration();

            // Определяем путь к файлу лога
            string logFilePath = Path.ChangeExtension(Environment.GetCommandLineArgs()[0], ".log");
            mLogger = LogManager.GetLogger(LoggerName);
        }

        public static void LoadConfiguration()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ThreadIdleTimeout"]))
                ConfigThreadIdleTimeout = TimeSpan.Parse(ConfigurationManager.AppSettings["ThreadIdleTimeout"]);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ThreadCount"]))
                ConfigThreadCount = Int32.Parse(ConfigurationManager.AppSettings["ThreadCount"]);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BulkFileDirectory"]))
                BulkFileDirectory = ConfigurationManager.AppSettings["BulkFileDirectory"];
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ArchiveFileDirectory"]))
                ArchiveFileDirectory = ConfigurationManager.AppSettings["ArchiveFileDirectory"];
        }

        public static string NormalizePath(string path)
        {
            if (path == null) 
                throw new ArgumentNullException();
            if (path == string.Empty) 
                throw new ArgumentException();

            if (Path.IsPathRooted(path)) 
                return path;
            return Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), path);
        }

        public static bool ServiceStopping
        {
            get
            {
                return mServiceStopping;
            }
        }

        protected override void OnStart(string[] args)
        {
            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            mLogger.Info(string.Format("{0} {1}", assemblyName.Name, assemblyName.Version.ToString()));

            mLogger.Info("Service started");
            TaskManager.Instance().BeginExecuteTasks();
        }

        protected override void OnStop()
        {
            mServiceStopping = true;
            DateTime stoppingEnd = DateTime.Now.Add(ThreadStoppingTimeout);
            while (true)
            {
                if (!TaskManager.Instance().HasExecutingTask())
                    break;
                if (stoppingEnd < DateTime.Now)
                    break;
                Thread.Sleep(ThreadStoppingCheckTimeout);
            }

            TaskManager.Instance().EndExecuteTasks();
            mLogger.Info("Service stopped");
        }
    }
}
