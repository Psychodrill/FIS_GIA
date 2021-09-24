using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class EntrantRecordViewModel
    {
        public int EntrantId { get; set; }
        public int ApplicationId { get; set; }

        [Display(Name = "ФИО абитуриента")]
        public string EntrantName { get; set; }

        [Display(Name = "Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        [Display(Name = "№ заявления")]
        public string ApplicationNumber { get; set; }

        [Display(Name = "Дата регистрации")]
        public string RegistrationDate { get; set; }

        [Display(Name = "Приемная кампания")]
        public string CampaignName { get; set; }

        [Display(Name = "Конкурс")]
        public string CompetitiveGroupNames { get; set; }

        [Display(Name = "Статус")]
        public string StatusName { get; set; }

        public bool AllowEdit { get; set; }

        public bool IsCampaignFinished { get; set; }
    }
}