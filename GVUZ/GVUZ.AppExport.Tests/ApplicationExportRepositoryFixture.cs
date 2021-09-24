using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.AppExport.Services;
using NUnit.Framework;

namespace GVUZ.AppExport.Tests
{
    [TestFixture]
    public class ApplicationExportRepositoryFixture
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
        public void CreateNewRequest()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var app = repository.AddNew(42, 2014);
            var id = repository.FetchNewId(100);
            Assert.AreEqual(1, id.Count());
            Assert.AreEqual(app.RequestId, id.Single());
        }

        [Test]
        [Ignore()]
        public void FindByInstitution()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var items = Enumerable.Range(1, 10).Select(i => repository.AddNew(i, 2014)).ToArray();
            int numSaved = repository.FetchNewId(items.Length).Count();
            Assert.AreEqual(items.Length, numSaved);
            var inst = repository.FindByInstitution(5).Single();
            Assert.IsNotNull(inst);
            Assert.AreEqual(items[4].RequestId, inst.RequestId);
        }

        [Test]
        [Ignore()]
        public void CommitState()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var items = Enumerable.Range(1, 10).Select(i => repository.AddNew(42, 2014)).ToArray();

            Assert.IsTrue(repository.FindByInstitution(42).All(x => x.RequestStatus == ApplicationExportRequestStatus.New));
            repository.CommitState(items.Select(x => x.RequestId), ApplicationExportRequestStatus.Enqueued);
            Assert.IsTrue(repository.FindByInstitution(42).All(x => x.RequestStatus == ApplicationExportRequestStatus.Enqueued));
        }

        [Test]
        [Ignore()]
        public void FindByRequestId()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var items = Enumerable.Range(1, 10).Select(i => repository.AddNew(42, 2014)).ToArray();

            var itemId = items[0].RequestId;

            var loaded = repository.FindByRequestId(itemId);

            Assert.IsNotNull(loaded);
            Assert.AreNotSame(items[0], loaded);
            Assert.AreEqual(items[0].RequestId, loaded.RequestId);
            Assert.AreEqual(items[0].RequestStatus, loaded.RequestStatus);
            Assert.AreEqual(items[0].YearStart, loaded.YearStart);
            Assert.AreEqual(items[0].InstitutionId, loaded.InstitutionId);
            Assert.AreEqual(items[0].RequestDate.ToString("dd.MM.yyyy HH:mm:ss"), loaded.RequestDate.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        [Test]
        [Ignore()]
        public void ResetIncomplete()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var id = Enumerable.Range(1, 10).Select(i => repository.AddNew(42, i)).Select(x => x.RequestId).ToArray();
            repository.CommitState(id, ApplicationExportRequestStatus.Enqueued);
            var first = repository.FindByInstitution(42);
            Assert.IsTrue(first.All(x => x.RequestStatus == ApplicationExportRequestStatus.Enqueued));
            repository.ResetIncomplete();
            var second = repository.FindByInstitution(42);
            Assert.IsTrue(second.All(x => x.RequestStatus == ApplicationExportRequestStatus.New));
        }

        [Test]
        [Ignore()]
        public void HasPending()
        {
            var repository = new SqlApplicationExportRequestRepository(ConnectionString);
            var id = Enumerable.Range(1, 10).Select(i => repository.AddNew(42, i)).Select(x => x.RequestId).ToArray();

            var hasPending = repository.HasPending(42, 5);

            Assert.IsTrue(hasPending);

            repository.CommitState(id, ApplicationExportRequestStatus.Complete);

            hasPending = repository.HasPending(42, 5);

            Assert.IsFalse(hasPending);
        }

        [Test]
        [Ignore()]
        public void CreateExportDto()
        {
            const int institutionId = 587;
            int yearStart = DateTime.Now.Year;

            var loader = new ApplicationExportLoader(institutionId, yearStart);
            loader.ApplicationFetched += (sender, args) => Console.WriteLine("Found app #{0}", args.Data.AppId);
            loader.Load();


        }
    }
}