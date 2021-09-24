using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class PsychoDocumentViewModel : BaseDocumentViewModel
    {
        public PsychoDocumentViewModel()
        {
            DocumentTypeID = 12;
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(20)]
        [DisplayName("Номер")]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [LocalRequired]
        [DisplayName("Выдана")]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }
    }
}