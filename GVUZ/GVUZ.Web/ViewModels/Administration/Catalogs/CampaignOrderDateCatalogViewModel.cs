using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
    public class CampaignOrderDateCatalogViewModel
    {
        public class CampaignOrderDateCatalogData
        {
            [DisplayName("Год начала ПК")]
            public int YearStart { get; set; }

            [DisplayName("Начало приёма документов")]
            public string StartDate { get; set; }

            [DisplayName("Окончание приёма документов")]
            public string EndDate { get; set; }

            [DisplayName("Издание приказа о зачислении - целевой приём")]
            public string TargetOrderDate { get; set; }

            [DisplayName("Издание приказа о зачислении - 1 этап")]
            public string Stage1OrderDate { get; set; }

            [DisplayName("Издание приказа о зачислении - 2 этап")]
            public string Stage2OrderDate { get; set; }

            [DisplayName("Издание приказа о зачислении - с оплатой обучения")]
            public string PaidOrderDate { get; set; }

            [DisplayName("Разрешено использование дат предыдущих лет")]
            public int PreviousUseDepth { get; set; }
        }

        public CampaignOrderDateCatalogData[] CampaignOrderDates { get; set; }

        public CampaignOrderDateCatalogData DataDescr { get { return null; } }
    }
}