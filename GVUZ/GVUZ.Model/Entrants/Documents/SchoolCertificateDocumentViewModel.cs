using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class SchoolCertificateDocumentViewModel : BaseDocumentViewModel
    {
        [LocalRequired]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(15)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [StringLength(6)]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

        [DisplayName("Средний балл")]
        public float? GPA { get; set; }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentEdu WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentEdu(EntrantDocumentID, GPA)
	VALUES({0}, {1})",
                                          EntrantDocumentID,
                                          GPA);
        }
    }
}