using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVUZ.DAL.Dapper.Repository.Interfaces.CompetitiveGroups;
using GVUZ.DAL.Dapper.Repository.Model.CompetitiveGroups;
using System.Configuration;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.DAL.Tests.TestHelpers;

namespace GVUZ.DAL.Tests.Dapper.CompetitiveGroup
{
    /// <summary>
    /// Summary description for CompetitiveGroupTest
    /// </summary>
    [TestClass]
    public class CompetitiveGroupTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            GvuzRepository.Initialize(Config.GVUZConnectionString);

            GVUZDatabaseHelper gvuzDatabaseHelper = new GVUZDatabaseHelper();
            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.TryCreateStructure();
        }

        ICompetitiveGroupRepository competitiveGroupRepository;
        public CompetitiveGroupTest()
        {
            this.competitiveGroupRepository = new CompetitiveGroupRepository();
        }
        
        public CompetitiveGroupTest(ICompetitiveGroupRepository competitiveGroupRepository)
        {
            this.competitiveGroupRepository = new CompetitiveGroupRepository();
        }
        //[TestMethod]
        //public void GetCompetitiveGroupList()
        //{
        //    var result = competitiveGroupRepository. GetCampaignList();
        //    Assert.IsNotNull(result);
        //}
    }
    

}
