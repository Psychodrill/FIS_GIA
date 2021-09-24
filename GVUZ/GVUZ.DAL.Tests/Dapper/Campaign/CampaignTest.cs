using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVUZ.DAL.Dapper.Repository.Interfaces.Campaign;
using GVUZ.DAL.Dapper.Repository.Model.Campaigns;
using System.Configuration;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.DAL.Tests.TestHelpers;

namespace GVUZ.DAL.Tests.Dapper.Campaign
{
    [TestClass]
    public class CampaignTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            GvuzRepository.Initialize(Config.GVUZConnectionString);

            GVUZDatabaseHelper gvuzDatabaseHelper = new GVUZDatabaseHelper();
            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.TryCreateStructure();
        }

        [TestMethod]
        public void GetCampaignList()
        {
            var result = new CampaignRepository().GetCampaignList(587);
            Assert.IsNotNull(result);
        }
    }
}
