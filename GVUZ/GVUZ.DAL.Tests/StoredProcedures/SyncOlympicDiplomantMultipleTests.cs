using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Tests.TestHelpers;
using GVUZ.DAL.Tests.TestHelpers.L2SGVUZ;
using GVUZ.DAL.Tests.TestHelpers.L2SFBS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GVUZOlympicDiplomant = GVUZ.DAL.Tests.TestHelpers.L2SGVUZ.OlympicDiplomant;

namespace GVUZ.DAL.Tests.StoredProcedures
{
    [TestClass]
    public class SyncOlympicDiplomantMultipleTests
    {
        private FBSDatabaseHelper _fbsDatabaseHelper;
        private ESRPDatabaseHelper _esrpDatabaseHelper;
        private GVUZDatabaseHelper _gvuzDatabaseHelper;

        private Action<long> _testSyncOlympicDiplomantMultipleOneOlympicDiplomant;
        private Action<long, long> _testSyncOlympicDiplomantMultipleTwoOlympicDiplomants;

        [TestInitialize]
        public void InitializeTest()
        {
            TestLocker.Lock();

            _fbsDatabaseHelper = new FBSDatabaseHelper();
            _esrpDatabaseHelper = new ESRPDatabaseHelper();
            _gvuzDatabaseHelper = new GVUZDatabaseHelper();

            _fbsDatabaseHelper.CreatEmptyDatabase();
            _fbsDatabaseHelper.TryCreateStructure();

            _esrpDatabaseHelper.CreatEmptyDatabase();
            _esrpDatabaseHelper.TryCreateStructure();

            _gvuzDatabaseHelper.CreatEmptyDatabase();
            _gvuzDatabaseHelper.TryCreateStructure();


            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant = (olympicDiplomantId) =>
            {
                _gvuzDatabaseHelper.ExecSyncOlympicDiplomantMultiple(new List<long>() { olympicDiplomantId });
            };
            _testSyncOlympicDiplomantMultipleTwoOlympicDiplomants = (olympicDiplomantId1, olympicDiplomantId2) =>
            {
                _gvuzDatabaseHelper.ExecSyncOlympicDiplomantMultiple(new List<long>() { olympicDiplomantId1, olympicDiplomantId2 });
            };
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _gvuzDatabaseHelper.DropDatabase();
            _esrpDatabaseHelper.DropDatabase();
            _fbsDatabaseHelper.DropDatabase();

            TestLocker.Unlock();
        }

        [TestMethod]
        public void SyncOlympicDiplomantMultipleCanRunOnEmptyDB()
        {
            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(0);
        }

        [TestMethod]
        public void SyncOlympicDiplomantMultipleCanRunWithoutIds()
        {
            _gvuzDatabaseHelper.ExecSyncOlympicDiplomantMultiple(new List<long>());
        }

        [TestMethod]
        public void SyncOlympicDiplomantMultipleProcessNotFound1()
        {
            GVUZDataHelper.ClearRVIPersons();

            long olympicDiplomantId;
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out olympicDiplomantId);

            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(olympicDiplomantId);

            Assert.IsTrue(GVUZDataHelper.GetRVIIsEmpty());
            CheckOlympicDiplomantStatus(olympicDiplomantId, Constants.GVUZOlympicDiplomantStatusNotFound);
        } 

        [TestMethod]
        public void SyncOlympicDiplomantMultipleProcessNotFound2()
        {
            GVUZDataHelper.ClearRVIPersons();

            long olympicDiplomantId1;
            long olympicDiplomantId2;
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out olympicDiplomantId1);
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Петров", "Петр", "Петрович", "222222", "2222", out olympicDiplomantId2);

            _testSyncOlympicDiplomantMultipleTwoOlympicDiplomants(olympicDiplomantId1, olympicDiplomantId2);

            Assert.IsTrue(GVUZDataHelper.GetRVIIsEmpty());
            CheckOlympicDiplomantStatus(olympicDiplomantId1, Constants.GVUZOlympicDiplomantStatusNotFound);
            CheckOlympicDiplomantStatus(olympicDiplomantId2, Constants.GVUZOlympicDiplomantStatusNotFound);
        }

        [TestMethod]
        public void SyncOlympicDiplomantMultipleRunsTwice()
        {
            GVUZDataHelper.ClearRVIPersons();

            long olympicDiplomantId;
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out olympicDiplomantId);

            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(olympicDiplomantId);
            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(olympicDiplomantId);

            Assert.IsTrue(GVUZDataHelper.GetRVIIsEmpty());
            CheckOlympicDiplomantStatus(olympicDiplomantId, Constants.GVUZOlympicDiplomantStatusNotFound);
        } 

        [TestMethod]
        public void SyncOlympicDiplomantMultipleLinksToExistingRVIData()
        {
            int personId = 1;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId, "Иванов", "Иван", "Иванович", "111111", "1111");

            long olympicDiplomantId;
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out olympicDiplomantId);

            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(olympicDiplomantId);

            CheckOlympicDiplomantStatus(olympicDiplomantId, Constants.GVUZOlympicDiplomantStatusFound);
            CheckOlympicDiplomantRVIPerson(olympicDiplomantId, personId);
        } 

        [TestMethod]
        public void SyncOlympicDiplomantMultipleProcessRVIDuplicates()
        {
            int personId1 = 1;
            int personId2 = 2;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId1, "Иванов", "Иван", "Иванович", "111111", "1111");
            GVUZDataHelper.CreateRVIPersonWithDocument(personId2, "Иванов", "Иван", "Иванович", "111111", "1111");

            long olympicDiplomantId;
            GVUZDataHelper.CreateOlympicDiplomantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out olympicDiplomantId);

            _testSyncOlympicDiplomantMultipleOneOlympicDiplomant(olympicDiplomantId);

            CheckOlympicDiplomantStatus(olympicDiplomantId, Constants.GVUZOlympicDiplomantStatusDuplicatesFound);
        }

        private void CheckOlympicDiplomantStatus(long olympicDiplomantId, int expectedStatusId)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZOlympicDiplomant olympicDiplomant = gvuzDB.OlympicDiplomants.SingleOrDefault(x => x.OlympicDiplomantID == olympicDiplomantId);
                Assert.IsNotNull(olympicDiplomant);
                Assert.IsTrue(olympicDiplomant.StatusID.HasValue);
                Assert.AreEqual(expectedStatusId, olympicDiplomant.StatusID);

                if ((expectedStatusId == Constants.GVUZOlympicDiplomantStatusDuplicatesFound) || (expectedStatusId == Constants.GVUZOlympicDiplomantStatusNotFound))
                {
                    Assert.IsFalse(olympicDiplomant.PersonId.HasValue);
                    Assert.IsFalse(olympicDiplomant.PersonLinkDate.HasValue);
                }
                else
                {
                    Assert.IsTrue(olympicDiplomant.PersonId.HasValue);
                    Assert.IsTrue(olympicDiplomant.PersonLinkDate.HasValue);
                }
            }
        }

        private void CheckOlympicDiplomantRVIPerson(long olympicDiplomantId, int experctedRviPersonId)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZOlympicDiplomant olympicDiplomant = gvuzDB.OlympicDiplomants.SingleOrDefault(x => x.OlympicDiplomantID == olympicDiplomantId);
                Assert.IsNotNull(olympicDiplomant);
                Assert.IsTrue(olympicDiplomant.PersonId.HasValue);
                Assert.IsTrue(olympicDiplomant.PersonLinkDate.HasValue);

                OlympicDiplomantDocument identityDocument = gvuzDB.OlympicDiplomantDocuments.SingleOrDefault(x => x.OlympicDiplomantDocumentID == olympicDiplomant.OlympicDiplomantIdentityDocumentID);
                Assert.IsNotNull(identityDocument);

                RVIPerson rviPerson = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == olympicDiplomant.PersonId.Value);
                Assert.IsNotNull(rviPerson);

                Assert.AreEqual(experctedRviPersonId, rviPerson.PersonId);

                Assert.AreEqual(StringsHelper.NormalizeNamePart(identityDocument.FirstName), rviPerson.NormName);
                Assert.AreEqual(StringsHelper.NormalizeNamePart(identityDocument.LastName), rviPerson.NormSurname);
                Assert.AreEqual(StringsHelper.NormalizeNamePart(identityDocument.MiddleName), rviPerson.NormSecondName);

                RVIPersonIdentDoc rviDocument = gvuzDB.RVIPersonIdentDocs.SingleOrDefault(x => x.PersonId == rviPerson.PersonId);
                Assert.IsNotNull(rviDocument);

                Assert.AreEqual(identityDocument.DocumentNumber, rviDocument.DocumentNumber);
                Assert.AreEqual(identityDocument.DocumentSeries, rviDocument.DocumentSeries);
                Assert.AreEqual(identityDocument.DateIssue, rviDocument.DocumentDate);
                Assert.AreEqual(identityDocument.OrganizationIssue, rviDocument.DocumentOrganization);
            }
        }
    }
}
