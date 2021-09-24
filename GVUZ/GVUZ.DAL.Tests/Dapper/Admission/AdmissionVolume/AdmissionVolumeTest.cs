using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.Repository.Model.Admission;
using GVUZ.DAL.Dapper.Repository.Model;
using System.Configuration;
using GVUZ.DAL.Tests.TestHelpers;

namespace GVUZ.DAL.Tests.Dapper.Admission.AdmissionVolume
{
    [TestClass]
    public class AdmissionVolumeTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            GvuzRepository.Initialize(Config.GVUZConnectionString);
            
            GVUZDatabaseHelper  gvuzDatabaseHelper = new GVUZDatabaseHelper();
            gvuzDatabaseHelper.CreatEmptyDatabase();
            gvuzDatabaseHelper.TryCreateStructure();
        }
         
        [TestMethod]
        public void FillAdmissionVolumeViewModel()
        {
            var model = new DAL.Dapper.ViewModel.Admission.AdmissionVolumeViewModel();
            model.InstitutionID = 587;
            model.SelectedCampaignID = 46;


            var result = new AdmissionVolumeRepository().FillAdmissionVolumeViewModel(model, false);
            Assert.IsNotNull(result);
        }
    }
}
