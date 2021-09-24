using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptApplicationRecordViewModel
    {
        public static readonly AcceptApplicationRecordViewModel MetadataInstance = new AcceptApplicationRecordViewModel();

        public int ApplicationId { get; set; }

        [DisplayName("ФИО абитуриента")]
        public string EntrantName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Причина решения")]
        public string Reason { get; set; }

        //added 21.11.2017
        [DisplayName("Способ возврата документов")]
        public int? ReturnDocumentsTypeId { get; set; }

        [DisplayName("Дата возврата документов")]
        public DateTime? ReturnDocumentsDate { get; set; }

    }
}