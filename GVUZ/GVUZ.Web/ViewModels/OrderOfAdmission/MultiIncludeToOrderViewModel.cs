using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{

    public class MultiIncludeToOrderViewModel
    {
        public Order Ord;
        public ConditionInclude Condition;
        public List<Application> Apps;
        public Application App;

        public List<BudgetItem> BudgetLevels = new List<BudgetItem>();
        public List<Benefit> Benefits = new List<Benefit>();

        public class Application
        {
            public int ApplicationID { get; set; }
            public int InstitutionID { get; set; }

            public int AppCGItemId { get; set; }

            [DisplayName("Номер заявления")]
            public string Number { get; set; }

            [DisplayName("ФИО абитуриента")]
            public string FIO { get; set; }

            [DisplayName("Док-т, удостоверяющий личность")]
            public string Document { get; set; }

            [DisplayName("Дата регистрации")]
            public DateTime? RegistrationDate { get; set; }
            public string RegistrationDateText
            {
                get { return RegistrationDate.HasValue ? RegistrationDate.Value.ToString("dd.MM.yyyy") : ""; }
            }

            [DisplayName("Количество баллов")]
            public decimal? Points { get; set; }

            public string PointsStr
            {
                get
                {
                    if (!Points.HasValue)
                        return String.Empty;
                    else
                        return Points.Value.ToString("0.##");
                }
            }
        }

        public class Order
        {
            public int OrderID { get; set; }
            public int InstitutionID { get; set; }
            public short? OrderStatusId { get; set; }
            public bool? IsCampaignFinished { get; set; }

            [DisplayName("Наименование приказа")]
            public string OrderName { get; set; }

            [DisplayName("Номер приказа")]
            public string OrderNumber { get; set; }

            [DisplayName("Дата приказа")]
            public DateTime? OrderDate { get; set; }

            public string OrderDateText
            {
                get
                {
                    return OrderDate.HasValue ? OrderDate.Value.ToString("dd.MM.yy") : "";
                }
            }

            [DisplayName("Реквизиты приказа")]
            public string OrderDetails { get; set; }

            [DisplayName("Льготный приказ")]
            public bool? IsForBeneficiary { get; set; }
            public string IsForBeneficiaryText
            {
                get
                {
                    if (IsForBeneficiary == null) { return ""; }
                    if (IsForBeneficiary == true) { return "Да"; } else { return "Нет"; }
                }
            }

            [DisplayName("Прием по направлениям Минобрнауки")]
            public bool? IsForeigner { get; set; }
            public string IsForeignerText
            {
                get
                {
                    if (IsForeigner == null) { return ""; }
                    if (IsForeigner == true) { return "Да"; } else { return "Нет"; }
                }
            }

            [DisplayName("Уровень образования")]
            public string EducationLevel { get; set; }

            [DisplayName("Форма обучения")]
            public string EducationForm { get; set; }

            [DisplayName("Источник финансирования")]
            public string EducationSource { get; set; }

            [DisplayName("Этап приема")]
            public string Stage { get; set; }


            public void IncludeToOrderViewModel()
            {
            }

        }

        public class ConditionInclude
        {
            public int AppCGItemId { get; set; }

            public int CompetitiveGroupID { get; set; }

            [DisplayName("Конкурс")]
            public string CompetitiveGroupName { get; set; }

            [DisplayName("Направление подготовки/специальность")]
            public string DirectionName { get; set; }

            [DisplayName("Уровень образования")]
            public string EduLevelName { get; set; }

            [DisplayName("Форма обучения")]
            public string EducationFormName { get; set; }

            [DisplayName("Источник финансирования")]
            public string EducationSourceName { get; set; }
            public int? EducationSourceID { get; set; }

            [DisplayName("Объем приема")]
            public int CGTotal { get; set; }
            public int CGCount { get; set; }

            [DisplayName("Фед")]
            public int CGBFedTotal { get; set; }
            public int CGBFedCount { get; set; }

            [DisplayName("Рег")]
            public int CGBRegTotal { get; set; }
            public int CGBRegCount { get; set; }

            [DisplayName("Мун")]
            public int CGBMunTotal { get; set; }
            public int CGBMunCount { get; set; }

            [DisplayName("Уровень бюджета")]
            public int IdLevelBudget { get; set; }
        }

        public class BudgetItem
        {
            public int? ID { get; set; }
            public string Name { get; set; }
        }

        public class Benefit
        {
            public int? BenefitID { get; set; }
            public string BenefitName { get; set; }
        }

    }
}