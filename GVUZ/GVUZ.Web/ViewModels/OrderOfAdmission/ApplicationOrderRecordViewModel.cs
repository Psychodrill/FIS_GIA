using System;
using System.ComponentModel;
using GVUZ.Data.Model;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationOrderRecordViewModel
    {
        public int OrderId { get; set; }
        public int OrderTypeId { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicationItemId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public int OrderStatus { get; set; }

        public bool AllowExcludeAction
        {
            get { return OrderStatus != OrderOfAdmissionStatus.Published; }
        }

        public bool AllowRefuseAdmissionAction
        {
            get
            {
                return (OrderTypeId == OrderOfAdmissionType.OrderOfAdmission
                    && OrderStatus == OrderOfAdmissionStatus.Published
                    && IsDisagreed 
                    && IsDisagreedDate.HasValue);
            }
        }

        [DisplayName("Идентификатор")]
        public string ApplicationOrderId { get; set; }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("ФИО")]
        public string EntrantName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevelName { get; set; }

        [DisplayName("Форма обучения")]
        public string EducationFormName { get; set; }

        [DisplayName("Источник финансирования")]
        public string EducationSourceName { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroupName { get; set; }

        [DisplayName("Направление подготовки / специальность")]
        public string DirectionName { get; set; }

        [DisplayName("Льгота")]
        public string Benefit { get; set; }

        [DisplayName("Организация целевого приема")]
        public string CompetitiveGroupTargetName { get; set; }

        [DisplayName("Количество баллов")]
        public decimal? Rating { get; set; } // decimal 10,4

        [DisplayName("Количество баллов")]
        public string RatingText
        {
            get { return Rating.HasValue ? Rating.Value.ToString("0.####") : string.Empty; }
        }

        [DisplayName("Уровень бюджета")]
        public string LevelBudgetName { get; set; }

        [DisplayName("Согласие")]
        public bool IsAgreed { get; set; }
        public string IsAgreedStr { get { return IsAgreed ? "Да" : "Нет"; } }

        [DisplayName("Дата согласия")]
        public DateTime? IsAgreedDate { get; set; }
        public string IsAgreedDateStr
        {
            get
            {
                if (IsAgreedDate.HasValue)
                    return IsAgreedDate.Value.ToString("dd.MM.yyyy");
                return null;
            }
        }

        [DisplayName("Отказ от согласия")]
        public bool IsDisagreed { get; set; }
        public string IsDisagreedStr { get { return IsDisagreed ? "Да" : "Нет"; } }

        [DisplayName("Дата отказа")]
        public DateTime? IsDisagreedDate { get; set; }
        public string IsDisagreedDateStr
        {
            get
            {
                if (IsDisagreedDate.HasValue)
                    return IsDisagreedDate.Value.ToString("dd.MM.yyyy");
                return null;
            }
        }

        [DisplayName("Приказ о зачислении")]
        public string OrderOfAdmissionInfo { get; set; }
    }
}