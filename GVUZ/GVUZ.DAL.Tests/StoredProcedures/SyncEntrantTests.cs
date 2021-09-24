using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.DAL.Tests.TestHelpers;
using GVUZ.DAL.Tests.TestHelpers.L2SGVUZ;
using GVUZ.DAL.Tests.TestHelpers.L2SFBS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GVUZEntrant = GVUZ.DAL.Tests.TestHelpers.L2SGVUZ.Entrant;

namespace GVUZ.DAL.Tests.StoredProcedures
{
    [TestClass]
    public class SyncEntrantTests
    {
        private FBSDatabaseHelper _fbsDatabaseHelper;
        private ESRPDatabaseHelper _esrpDatabaseHelper;
        private GVUZDatabaseHelper _gvuzDatabaseHelper;

        private Action<int> _testSyncEntrantOneEntrant;
        private Action<int> _testSyncEntrantMultipleOneEntrant;
        private Action<int, int> _testSyncEntrantTwoEntrants;
        private Action<int, int> _testSyncEntrantMultipleTwoEntrants;

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

            _testSyncEntrantOneEntrant = (entrantId) =>
            {
                _gvuzDatabaseHelper.ExecSyncEntrant(entrantId);
            };
            _testSyncEntrantMultipleOneEntrant = (entrantId) =>
            {
                _gvuzDatabaseHelper.ExecSyncEntrantMultiple(new List<int>() { entrantId });
            };
            _testSyncEntrantTwoEntrants = (entrantId1, entrantId2) =>
            {
                _gvuzDatabaseHelper.ExecSyncEntrant(entrantId1);
                _gvuzDatabaseHelper.ExecSyncEntrant(entrantId2);
            };
            _testSyncEntrantMultipleTwoEntrants = (entrantId1, entrantId2) =>
            {
                _gvuzDatabaseHelper.ExecSyncEntrantMultiple(new List<int>() { entrantId1, entrantId2 });
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
        public void SyncEntrantCanRunOnEmptyDB()
        {
            _testSyncEntrantOneEntrant(0);
            _testSyncEntrantMultipleOneEntrant(0);
        }

        [TestMethod]
        public void SyncEntrantMultipleCanRunWithoutIds()
        {
            _gvuzDatabaseHelper.ExecSyncEntrantMultiple(new List<int>());
        }

        [TestMethod]
        public void SyncEntrantCreatesRVIData1()
        {
            CreatesRVIData1(_testSyncEntrantOneEntrant);
        }

        [TestMethod]
        public void SyncEntrantMultipleCreatesRVIData1()
        {
            CreatesRVIData1(_testSyncEntrantMultipleOneEntrant);
        }

        private void CreatesRVIData1(Action<int> test)
        {
            int entrantId;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId);

            test(entrantId);

            CheckEntrantRVIPerson(entrantId);
        }

        [TestMethod]
        public void SyncEntrantCreatesRVIData2()
        {
            CreatesRVIData2(_testSyncEntrantTwoEntrants);
        }

        [TestMethod]
        public void SyncEntrantMultipleCreatesRVIData2()
        {
            CreatesRVIData2(_testSyncEntrantMultipleTwoEntrants);
        }

        private void CreatesRVIData2(Action<int, int> test)
        {
            int entrantId1;
            int entrantId2;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId1);
            GVUZDataHelper.CreateEntrantWithDocument("Петров", "Петр", "Петрович", "222222", "2222", out entrantId2);

            test(entrantId1, entrantId2);

            CheckEntrantRVIPerson(entrantId1);
            CheckEntrantRVIPerson(entrantId2);
            CheckEntrantLinkedToDistinctRVIPersons(entrantId1, entrantId2);
        }

        [TestMethod]
        public void SyncEntrantMergesGVUZDuplicates()
        {
            MergesGVUZDuplicates(_testSyncEntrantTwoEntrants);
        }

        [TestMethod]
        public void SyncEntrantMultipleMergesGVUZDuplicates()
        {
            MergesGVUZDuplicates(_testSyncEntrantMultipleTwoEntrants); ;
        }

        private void MergesGVUZDuplicates(Action<int, int> test)
        {
            int entrantId1;
            int entrantId2;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId1);
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId2);

            test(entrantId1, entrantId2);

            CheckEntrantRVIPerson(entrantId1);
            CheckEntrantRVIPerson(entrantId2);
            CheckEntrantLinkedToSameRVIPersons(entrantId1, entrantId2);
        }

        [TestMethod]
        public void SyncEntrantRunsTwice()
        {
            RunsTwice(_testSyncEntrantOneEntrant);
        }

        [TestMethod]
        public void SyncEntrantMultipleRunsTwice()
        {
            RunsTwice(_testSyncEntrantMultipleOneEntrant);
        }

        private void RunsTwice(Action<int> test)
        {
            int entrantId;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId);

            test(entrantId);
            test(entrantId);

            CheckEntrantRVIPerson(entrantId);
        }

        [TestMethod]
        public void SyncEntrantLinksToExistingRVIData()
        {
            LinksToExistingRVIData(_testSyncEntrantOneEntrant);
        }

        [TestMethod]
        public void SyncEntrantMultipleLinksToExistingRVIData()
        {
            LinksToExistingRVIData(_testSyncEntrantMultipleOneEntrant);
        }

        private void LinksToExistingRVIData(Action<int> test)
        {
            int personId = 1;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId, "Иванов", "Иван", "Иванович", "111111", "1111");

            int entrantId;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId);

            test(entrantId);

            CheckEntrantRVIPerson(entrantId, personId);
        }

        [TestMethod]
        public void SyncEntrantMergesRVIDuplicates()
        {
            MergesRVIDuplicates(_testSyncEntrantOneEntrant);
        }

        [TestMethod]
        public void SyncEntrantMultipleMergesRVIDuplicates()
        {
            MergesRVIDuplicates(_testSyncEntrantMultipleOneEntrant);
        }

        private void MergesRVIDuplicates(Action<int> test)
        {
            int personId1 = 1;
            int personId2 = 2;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId1, "Иванов", "Иван", "Иванович", "111111", "1111");
            GVUZDataHelper.CreateRVIPersonWithDocument(personId2, "Иванов", "Иван", "Иванович", "111111", "1111");

            int entrantId;
            GVUZDataHelper.CreateEntrantWithDocument("Иванов", "Иван", "Иванович", "111111", "1111", out entrantId);

            test(entrantId);

            CheckEntrantRVIPerson(entrantId);
            CheckRVIDuplicatesMerged(personId1, personId2);
        }

        private void CheckEntrantRVIPerson(int entrantId, int? experctedRviPersonId = null)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZEntrant entrant = gvuzDB.Entrants.SingleOrDefault(x => x.EntrantID == entrantId);
                Assert.IsNotNull(entrant);
                Assert.IsTrue(entrant.PersonId.HasValue);
                Assert.IsTrue(entrant.PersonLinkDate.HasValue);

                EntrantDocument identityDocument = gvuzDB.EntrantDocuments.SingleOrDefault(x => x.EntrantDocumentID == entrant.IdentityDocumentID);
                Assert.IsNotNull(identityDocument);

                RVIPerson rviPerson = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == entrant.PersonId.Value);
                Assert.IsNotNull(rviPerson);

                if (experctedRviPersonId.HasValue)
                {
                    Assert.AreEqual(experctedRviPersonId.Value, rviPerson.PersonId);
                }

                Assert.AreEqual(StringsHelper.NormalizeNamePart(entrant.FirstName), rviPerson.NormName);
                Assert.AreEqual(StringsHelper.NormalizeNamePart(entrant.LastName), rviPerson.NormSurname);
                Assert.AreEqual(StringsHelper.NormalizeNamePart(entrant.MiddleName), rviPerson.NormSecondName);
                Assert.AreEqual(entrant.Email, rviPerson.Email);
                Assert.AreEqual(entrant.MobilePhone, rviPerson.MobilePhone);
                Assert.AreEqual(entrant.SNILS, rviPerson.SNILS);

                bool expectedSex = (entrant.GenderID == Constants.GVUZFemaleSexId) ? Constants.RVIFemaleSex : Constants.RVIMaleSex;
                Assert.AreEqual(expectedSex, rviPerson.Sex.Value);

                RVIPersonIdentDoc rviDocument = gvuzDB.RVIPersonIdentDocs.SingleOrDefault(x => x.PersonId == rviPerson.PersonId);
                Assert.IsNotNull(rviDocument);

                Assert.AreEqual(identityDocument.DocumentNumber, rviDocument.DocumentNumber);
                Assert.AreEqual(identityDocument.DocumentSeries, rviDocument.DocumentSeries);
                Assert.AreEqual(identityDocument.DocumentDate, rviDocument.DocumentDate);
                Assert.AreEqual(identityDocument.DocumentOrganization, rviDocument.DocumentOrganization);
            }
        }

        private void CheckEntrantLinkedToDistinctRVIPersons(int entrantId1, int entrantId2)
        {
            Assert.AreNotEqual(entrantId1, entrantId2);
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZEntrant entrant1 = gvuzDB.Entrants.SingleOrDefault(x => x.EntrantID == entrantId1);
                GVUZEntrant entrant2 = gvuzDB.Entrants.SingleOrDefault(x => x.EntrantID == entrantId2);
                Assert.IsNotNull(entrant1);
                Assert.IsNotNull(entrant2);
                Assert.IsTrue(entrant1.PersonId.HasValue);
                Assert.IsTrue(entrant2.PersonId.HasValue);
                Assert.AreNotEqual(entrant1.PersonId.Value, entrant2.PersonId.Value);

                RVIPerson rviPerson1 = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == entrant1.PersonId.Value);
                RVIPerson rviPerson2 = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == entrant2.PersonId.Value);
                Assert.IsNotNull(rviPerson1);
                Assert.IsNotNull(rviPerson2);

                RVIPersonIdentDoc rviDocument1 = gvuzDB.RVIPersonIdentDocs.SingleOrDefault(x => x.PersonId == rviPerson1.PersonId);
                RVIPersonIdentDoc rviDocument2 = gvuzDB.RVIPersonIdentDocs.SingleOrDefault(x => x.PersonId == rviPerson2.PersonId);
                Assert.IsNotNull(rviDocument1);
                Assert.IsNotNull(rviDocument2);

                Assert.AreNotEqual(rviDocument1.PersonIdentDocID, rviDocument2.PersonIdentDocID);
            }
        }

        private void CheckEntrantLinkedToSameRVIPersons(int entrantId1, int entrantId2)
        {
            Assert.AreNotEqual(entrantId1, entrantId2);
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                GVUZEntrant entrant1 = gvuzDB.Entrants.SingleOrDefault(x => x.EntrantID == entrantId1);
                GVUZEntrant entrant2 = gvuzDB.Entrants.SingleOrDefault(x => x.EntrantID == entrantId2);
                Assert.IsNotNull(entrant1);
                Assert.IsNotNull(entrant2);
                Assert.IsTrue(entrant1.PersonId.HasValue);
                Assert.IsTrue(entrant2.PersonId.HasValue);
                Assert.AreEqual(entrant1.PersonId.Value, entrant2.PersonId.Value);

                RVIPerson rviPerson = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == entrant1.PersonId.Value);
                Assert.IsNotNull(rviPerson);
            }
        }

        private void CheckRVIDuplicatesMerged(int personId1, int personId2)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                RVIPerson rviPerson1 = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == personId1);
                RVIPerson rviPerson2 = gvuzDB.RVIPersons.SingleOrDefault(x => x.PersonId == personId2);

                Assert.IsFalse((rviPerson1 == null) && (rviPerson2 == null));
                Assert.IsFalse((rviPerson1 != null) && (rviPerson2 != null));
            }
        }
    }
}
