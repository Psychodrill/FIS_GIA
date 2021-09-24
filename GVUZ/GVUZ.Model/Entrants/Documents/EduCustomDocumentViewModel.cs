using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class EduCustomDocumentViewModel : BaseDocumentViewModel
    {
        public EduCustomDocumentViewModel()
        {
            DocumentTypeID = 19;
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [StringLength(100)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [StringLength(20)]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

        [DisplayName("Кем выдан")]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        [DisplayName("Наименование документа")]
        [LocalRequired]
        [StringLength(500)]
        public string DocumentTypeNameText { get; set; }


        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentCustom WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentCustom(EntrantDocumentID, DocumentTypeNameText, AdditionalInfo)
	VALUES({0}, {1}, {2})",
                                          EntrantDocumentID,
                                          DocumentTypeNameText,
                                          null);
        }
    }
}