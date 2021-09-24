using System.ComponentModel;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class EntrantDetailsApplicationViewModel
    {
        public int ApplicationId { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("Приемная кампания")]
        public string Campaign { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroup { get; set; }

        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get; set; }

        [DisplayName("Льгота")]
        public string Benefit { get; set; }

        public bool AllowEdit { get; set; }

        public bool IsCampaignFinished { get; set; }
    }
}