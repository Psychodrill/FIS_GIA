using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVUZ.DAL.Dapper.Repository.Interfaces.TargetOrganization;
using GVUZ.DAL.Dapper.Repository.Model.TargetOrganization;
using System.Configuration;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.DAL.Tests.TestHelpers;

namespace GVUZ.DAL.Tests.Dapper.TargetOrganization
{
    [TestClass]
    public class TargetOrganizationTests
    {
        [TestInitialize]
        public void InitializeTest()
        {
            GvuzRepository.Initialize(Config.GVUZConnectionString);
        
            GVUZDatabaseHelper gvuzDatabaseHelper = new GVUZDatabaseHelper();
            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.TryCreateStructure();
        }

        ICompetitiveGroupTargetRepository competitiveGroupTargetRepository;
        public TargetOrganizationTests()
        {
            this.competitiveGroupTargetRepository = new CompetitiveGroupTargetRepository();
        }
        public TargetOrganizationTests(ICompetitiveGroupTargetRepository competitiveGroupTargetRepository)
        {
            this.competitiveGroupTargetRepository = competitiveGroupTargetRepository;
        }
        [TestMethod]
        public void GetGetCompetitiveGroupTarget()
        {
            var result = competitiveGroupTargetRepository.GetCompetitiveGroupTarget(587, null, false);
            Assert.IsNotNull(result); 
        }
    }
}
