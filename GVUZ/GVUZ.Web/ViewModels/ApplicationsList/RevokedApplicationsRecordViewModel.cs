using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RevokedApplicationsRecordViewModel
    {
        public int ApplicationId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public int StatusID { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Дата отзыва заявления")]
        public string LastDenyDate { get; set; }

        [DisplayName("ФИО")]
        public string EntrantFullName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get; set; }

        [DisplayName("Рекомендован к зачислению")]
        public bool IsInRecommendedLists { get; set; }
    }
}