using System.ComponentModel;
using GVUZ.Data.Model;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionRecordViewModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsCampaignFinished { get; set; }
        
        [DisplayName("Наименование приказа")]
        public string OrderName { get; set; }

        [DisplayName("Номер приказа")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата приказа")]
        public string OrderDate { get; set; }

        [DisplayName("Статус приказа")]
        public string OrderStatusName { get; set; }

        [DisplayName("Приемная кампания")]
        public string CampaignName { get; set; }

        [DisplayName("Этап приема")]
        public string Stage { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevel { get; set; }
         
        [DisplayName("Форма обучения")]
        public string EducationForm { get; set; }

        [DisplayName("Источник финансирования")]
        public string EducationSource { get; set; }

        [DisplayName("Количество абитуриентов")]
        public int NumberOfApplicants { get; set; }

        [DisplayName("Льготный приказ")]
        public bool IsForBeneficiary { get; set; }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool IsForeigner { get; set; }

        public bool DisablePublishAction
        {
            get { return IsCampaignFinished || OrderStatusId != OrderOfAdmissionStatus.NotPublished; }
        }

        public bool DisableEditAction
        {
            get { return IsCampaignFinished; }
        }

        public bool DisableDeleteAction
        {
            get { return IsCampaignFinished || OrderStatusId != OrderOfAdmissionStatus.NoApplications; }
        }

    }
}