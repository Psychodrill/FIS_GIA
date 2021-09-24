using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class SearchApplicationsRecordViewModel
    {
        public int ApplicationId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public int EntrantId { get; set; }
        public int StatusID { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroupNames { get; set; }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("ФИО")]
        public string EntrantFullName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        public bool AllowEdit
        {
            get { return !(IsCampaignFinished || IsInOrder); }
        }

        public bool IsInOrder { get; set; }

        public int? OrderId { get; set; }
    }
}