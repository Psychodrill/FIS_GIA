using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class MilitaryCardDocumentViewModel : BaseDocumentViewModel
    {
        public MilitaryCardDocumentViewModel()
        {
            DocumentTypeID = 14;
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(10)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [StringLength(6)]
        //[LocalRequired]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

        [LocalRequired]
        [DisplayName("Кем выдан")]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }
    }
}