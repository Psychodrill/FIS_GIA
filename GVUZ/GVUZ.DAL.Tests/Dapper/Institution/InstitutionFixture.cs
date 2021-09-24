using GVUZ.DAL.Dapper;
using GVUZ.DAL.Dapper.Repository.Model.Institution;
using GVUZ.DAL.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.DAL.Tests.TestHelpers;
using GVUZ.DAL.Tests.TestHelpers.L2SGVUZ;

using L2SInstitution = GVUZ.DAL.Tests.TestHelpers.L2SGVUZ.Institution;

namespace GVUZ.DAL.Tests.Dapper.Institution
{
    [TestClass]
    public class InstitutionFixture
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
        public void GetInstitutionInfoDto()
        {
            int institutionId;
            CreateInstitution(out institutionId);

            var r = new InstitutionRepository();
            var dto = r.GetInstitutionInfoDto(institutionId);
            Assert.IsNotNull(dto);
        }

        [TestMethod]
        public void InsertAttachment()
        {
            byte[] content = new byte[65535];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(content);
            }

            using (var ms = new MemoryStream(content))
            {
                var dto = new AttachmentCreateDto
                {
                    Content = ms,
                    ContentLength = content.LongLength,
                    ContentType = "application/pdf",
                    FileName = "file.pdf"
                };

                using (var cn = new SqlConnection(Config.GVUZConnectionString))
                {
                    cn.Open();

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        int id = DapperHelper.InsertAttachment(tx, dto);

                        Assert.IsTrue(id > 0);

                        tx.Commit();

                    }

                }
                    

            }
                
        }

        private void CreateInstitution(out int institutionId)
        {
            using (GVUZDataContext gvuzDB = new GVUZDataContext(Config.GVUZConnectionString))
            {
                L2SInstitution institution = new L2SInstitution();
                institution.InstitutionTypeID = Constants.GVUZHigherLevelInstitutionTypeId;
                institution.Address = "Address";
                institution.AdmissionStructurePublishDate = DateTime.Now;
                institution.BriefName = "BriefName";
                institution.City = "City";
                institution.CreatedDate = DateTime.Now;
                institution.DateUpdated = DateTime.Now;

                gvuzDB.Institutions.InsertOnSubmit(institution);
                gvuzDB.SubmitChanges();

                institutionId= institution.InstitutionID;
            }
        }
    }
}
