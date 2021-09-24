using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class NewApplicationsRecordViewModel
    {
        public static readonly NewApplicationsRecordViewModel MetadataInstance = new NewApplicationsRecordViewModel();

        public int ApplicationId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public bool IsEditable { get; set; }
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

        [DisplayName("Рекомендован к зачислению")]
        public bool IsInRecommendedLists { get; set; }

        public bool DisableActions
        {
            get { return IsCampaignFinished || IsEditable; }
        }

        public bool EnableActions
        {
            get { return !DisableActions; }
        }
    }
}