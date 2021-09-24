using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptedApplicationsRecordViewModel
    {
        public int ApplicationId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public bool CanIncludeInRecommended { get; set; }
        public int StatusID { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("Дата последней проверки")]
        public string LastCheckDate { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroupNames { get; set; }

        [DisplayName("ФИО")]
        public string EntrantFullName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get; set; }

        [DisplayName("Сдал документы")]
        public bool OriginalDocumentsReceived { get; set; }

        [DisplayName("Рейтинг")]
        public string Rating { get; set; }

        [DisplayName("Рекомендован к зачислению")]
        public bool IsInRecommendedLists { get; set; }

        [DisplayName("Рейтинг")]
        public decimal CalculatedRating { get; set; }
    }
}