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
    public class FindOlympicDiplomantRVIPersonTests
    {
        private FBSDatabaseHelper _fbsDatabaseHelper;
        private ESRPDatabaseHelper _esrpDatabaseHelper;
        private GVUZDatabaseHelper _gvuzDatabaseHelper;

        private Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> _testFindOlympicDiplomantRVIPersons;
        private Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> _testFindOlympicDiplomantRVIPersonsMultiple;

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

            _testFindOlympicDiplomantRVIPersons = (GVUZDatabaseHelper.PersonData personData) =>
            {
                return _gvuzDatabaseHelper.ExecFindOlympicDiplomantRVIPersons(personData);
            };

            _testFindOlympicDiplomantRVIPersonsMultiple = (GVUZDatabaseHelper.PersonData personData) =>
            {
                return _gvuzDatabaseHelper.ExecFindOlympicDiplomantRVIPersonsMultiple(new List<GVUZDatabaseHelper.PersonData>() { personData });
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
        public void FindOlympicDiplomantRVIPersonsCanRunOnEmptyDB()
        {
            CanRunOnEmptyDB(_testFindOlympicDiplomantRVIPersons);
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsMultipleCanRunOnEmptyDB()
        {
            CanRunOnEmptyDB(_testFindOlympicDiplomantRVIPersonsMultiple);
        }

        private void CanRunOnEmptyDB(Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> test)
        {
            GVUZDatabaseHelper.PersonData personData = new GVUZDatabaseHelper.PersonData()
            {
                LastName = String.Empty,
                FirstName = String.Empty,
                Patronymic = String.Empty,
                DocumentNumber = String.Empty,
                DocumentSeries = String.Empty
            };
            test(personData);
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsProcessNotFound()
        {
            ProcessNotFound(_testFindOlympicDiplomantRVIPersons);
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsMultipleProcessNotFound()
        {
            ProcessNotFound(_testFindOlympicDiplomantRVIPersonsMultiple);
        }

        private void ProcessNotFound(Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> test)
        {
            GVUZDataHelper.ClearRVIPersons();

            GVUZDatabaseHelper.PersonData personData = new GVUZDatabaseHelper.PersonData()
            {
                LastName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                DocumentNumber = "111111",
                DocumentSeries = "1111"
            };
            GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult result = test(personData);

            Assert.IsTrue(GVUZDataHelper.GetRVIIsEmpty());
            Assert.AreEqual(Constants.GVUZOlympicDiplomantStatusNotFound, result.StatusId);

        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsProcessFound()
        {
            ProcessFound(_testFindOlympicDiplomantRVIPersons);
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsMultipleProcessFound()
        {
            ProcessFound(_testFindOlympicDiplomantRVIPersonsMultiple);
        }

        private void ProcessFound(Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> test)
        {
            GVUZDatabaseHelper.PersonData personData = new GVUZDatabaseHelper.PersonData()
            {
                LastName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                DocumentNumber = "111111",
                DocumentSeries = "1111"
            };

            int personId = 1;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId, personData.LastName, personData.FirstName, personData.Patronymic, personData.DocumentNumber, personData.DocumentSeries);

            GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult result = test(personData);
            Assert.AreEqual(Constants.GVUZOlympicDiplomantStatusFound, result.StatusId);
            Assert.AreEqual(1, result.PersonIds.Count);
            Assert.IsTrue(result.PersonIds.Contains(personId));
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsProcessDuplicates()
        {
            ProcessDuplicates(_testFindOlympicDiplomantRVIPersons);
        }

        [TestMethod]
        public void FindOlympicDiplomantRVIPersonsMultipleProcessDuplicates()
        {
            ProcessDuplicates(_testFindOlympicDiplomantRVIPersonsMultiple);
        }

        private void ProcessDuplicates(Func<GVUZDatabaseHelper.PersonData, GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult> test)
        {
            GVUZDatabaseHelper.PersonData personData = new GVUZDatabaseHelper.PersonData()
            {
                LastName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                DocumentNumber = "111111",
                DocumentSeries = "1111"
            };

            int personId1 = 1;
            int personId2 = 2;
            GVUZDataHelper.CreateRVIPersonWithDocument(personId1, personData.LastName, personData.FirstName, personData.Patronymic, personData.DocumentNumber, personData.DocumentSeries);
            GVUZDataHelper.CreateRVIPersonWithDocument(personId2, personData.LastName, personData.FirstName, personData.Patronymic, personData.DocumentNumber, personData.DocumentSeries);

            GVUZDatabaseHelper.FindOlympicDiplomantRVIPersonsResult result = test(personData);
            Assert.AreEqual(Constants.GVUZOlympicDiplomantStatusDuplicatesFound, result.StatusId);
            Assert.AreEqual(2, result.PersonIds.Count);
            Assert.IsTrue(result.PersonIds.Contains(personId1));
            Assert.IsTrue(result.PersonIds.Contains(personId2));
        }
    }
}
