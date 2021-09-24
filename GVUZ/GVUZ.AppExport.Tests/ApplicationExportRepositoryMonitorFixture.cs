using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GVUZ.AppExport.Services;
using NUnit.Framework;
using log4net.Config;

namespace GVUZ.AppExport.Tests
{
    [TestFixture]
    public class ApplicationExportRepositoryMonitorFixture
    {
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["TestExport"].ConnectionString; }
        }

        [SetUp]
        public void SetUp()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "truncate table [dbo].[ApplicationExportRequest]";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        [Ignore()]
        public void WatchNew()
        {
            var cts = new CancellationTokenSource();
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var monitor = new ApplicationExportRequestMonitor(cts.Token, repository, TimeSpan.FromSeconds(1), 32);
            monitor.RequestCreated += (sender, args) =>
                {
                    Console.WriteLine("New requests: {0}", string.Join(", ", args.RequestId.Select(x => x.ToString())));
                    repository.CommitState(args.RequestId, ApplicationExportRequestStatus.Enqueued);
                };
            Task.Factory.StartNew(monitor.Run);
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            repository.AddNew(42, 2014);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            cts.Cancel();

            Assert.IsTrue(repository.FindByInstitution(42).All(x => x.RequestStatus == ApplicationExportRequestStatus.Enqueued));

        }

        [Test]
        [Ignore()]
        public void RunProcessor()
        {
            var cts = new CancellationTokenSource();
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var monitor = new ApplicationExportRequestMonitor(cts.Token, repository, TimeSpan.FromSeconds(5), 100);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DirectoryInfo storage = new DirectoryInfo(Path.Combine(appData, Guid.NewGuid().ToString("N").ToUpper()));
            XmlConfigurator.Configure();
            var processor = new ExportProcessor(cts.Token, 16, repository, monitor, storage);

            Task.Factory.StartNew(processor.Run);

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Enumerable.Range(1, 48).ToList().ForEach(x => repository.AddNew(42, 2014));

            Thread.Sleep(TimeSpan.FromMinutes(2));
            cts.Cancel(false);
        }

        [Test]
        [Ignore()]
        public void RunLoader()
        {
            var loader = new ApplicationExportLoader(587, 2014);
            loader.ApplicationFetched += (sender, args) => Console.WriteLine("Fetched app {0}", args.Data.AppId);
            loader.Load();

        }
    }
}