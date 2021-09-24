using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GVUZ.DAL.Tests.GenericTests
{
    [TestClass]
    public class DatabaseHelperTests
    {
        [TestInitialize]
        public void InitializeTest()
        {
            TestLocker.Lock();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            TestLocker.Unlock();
        }

        [TestMethod]
        public void CanGetDatabaseName()
        {
            string dbName = "DB_NAME";
            string connectionString1 = String.Format("Data Source=xx.xx.xxx.xx;Initial Catalog={0};User=xxx;Password=xxx", dbName);
            string connectionString2 = String.Format("Data Source=xx.xx.xxx.xx;Database={0};User=xxx;Password=xxx", dbName);
            string connectionString3 = String.Format("Initial Catalog={0};Data Source=xx.xx.xxx.xx;User=xxx;Password=xxx", dbName);
            string connectionString4 = String.Format("Data Source=xx.xx.xxx.xx;User=xxx;Password=xxx;Database={0};", dbName);

            Func<string, string> testable
                = (connectionString) => { return DatabaseHelperBase.GetDatabaseName(connectionString); };

            Assert.AreEqual(dbName, testable(connectionString1));
            Assert.AreEqual(dbName, testable(connectionString2));
            Assert.AreEqual(dbName, testable(connectionString3));
            Assert.AreEqual(dbName, testable(connectionString4));
        }

        [TestMethod]
        public void CanCreateEmptyFBSDatabase()
        {
            FBSDatabaseHelper fbsDatabaseHelper = new FBSDatabaseHelper();

            fbsDatabaseHelper.CreatEmptyDatabase();
            fbsDatabaseHelper.DropDatabase();
        }

        [TestMethod]
        public void CanCreateFBSDatabaseWithStructure()
        {
            FBSDatabaseHelper fbsDatabaseHelper = new FBSDatabaseHelper();

            fbsDatabaseHelper.CreatEmptyDatabase();
            fbsDatabaseHelper.CreateStructure();

            IEnumerable<string> migrations = fbsDatabaseHelper.GetOrderedMigrationsFromDB();
            foreach (string migration in fbsDatabaseHelper.AppliedMigrations)
            {
                if (!migrations.Contains(migration))
                    Assert.Fail();
            }

            fbsDatabaseHelper.DropDatabase();
        }

        [TestMethod]
        public void CanCreateEmptyESRPDatabase()
        {
            ESRPDatabaseHelper esrpDatabaseHelper = new ESRPDatabaseHelper();

            esrpDatabaseHelper.CreatEmptyDatabase();
            esrpDatabaseHelper.DropDatabase();
        }

        [TestMethod]
        public void CanCreateESRPDatabaseWithStructure()
        {
            ESRPDatabaseHelper esrpDatabaseHelper = new ESRPDatabaseHelper();

            esrpDatabaseHelper.CreatEmptyDatabase();
            esrpDatabaseHelper.CreateStructure();

            IEnumerable<string> migrations = esrpDatabaseHelper.GetOrderedMigrationsFromDB();
            foreach (string migration in esrpDatabaseHelper.AppliedMigrations)
            {
                if (!migrations.Contains(migration))
                    Assert.Fail();
            }

            esrpDatabaseHelper.DropDatabase();
        }

        [TestMethod]
        public void CanCreateEmptyGVUZDatabase()
        {
            GVUZDatabaseHelper gvuzDatabaseHelper = new GVUZDatabaseHelper();

            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.DropDatabase();
        }

        [TestMethod]
        public void CanCreateGVUZDatabaseWithStructure()
        {
            ESRPDatabaseHelper esrpDatabaseHelper = new ESRPDatabaseHelper();
            GVUZDatabaseHelper gvuzDatabaseHelper = new GVUZDatabaseHelper();

            esrpDatabaseHelper.CreatEmptyDatabase();
            esrpDatabaseHelper.CreateStructure();

            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.CreateStructure();

            IEnumerable<string> migrations = gvuzDatabaseHelper.GetOrderedMigrationsFromDB();
            foreach (string migration in gvuzDatabaseHelper.AppliedMigrations)
            {
                if (!migrations.Contains(migration))
                    Assert.Fail();
            }

            gvuzDatabaseHelper.DropDatabase();
            esrpDatabaseHelper.DropDatabase();
        }
    }
}
