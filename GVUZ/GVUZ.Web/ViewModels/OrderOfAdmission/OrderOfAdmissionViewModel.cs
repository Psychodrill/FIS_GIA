using System;
using System.ComponentModel;
using System.Globalization;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionViewModel
    {
        private ApplicationOrderListViewModel _applicationList;

        public int OrderId { get; set; }

        public int OrderTypeId { get; set; }

        public string OrderTypeNamePrepositional
        {
            get
            {
                if (OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                    return "о зачислении";
                else if (OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                    return "об отказе от зачисления";
                return null;
            }
        }

        [DisplayName("Статус приказа")]
        public int OrderStatus { get; set; }

        [DisplayName("Дата создания")]
        public string DateCreated { get; set; }

        [DisplayName("Дата изменения")]
        public string DateEdited {
            get
            {
                return string.Concat(dateEdited.ToString(),timeZone);
            }
            set
            {

                this.dateEdited = Convert.ToDateTime(value.Substring(0,18));
                timeZone = value.Substring(19);
            }
        }

        [DisplayName("Дата публикации")]
        public string DatePublished { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevelName { get; set; }

        [DisplayName("Форма обучения")]
        public string EducationFormName { get; set; }

        [DisplayName("Источник финансирования")]
        public string EducationSourceName { get; set; }

        [DisplayName("Приемная кампания")]
        public string CampaignName { get; set; }

        [DisplayName("Этап")]
        public short? Stage { get; set; }

        [DisplayName("Льготный приказ")]
        public bool? IsForBeneficiary { get; set; }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool? IsForeigner { get; set; }

        [DisplayName("Этап")]
        public string StageName
        {
            get { return Stage.HasValue ? Stage.Value.ToString(CultureInfo.InvariantCulture) : null; }
        }

        [DisplayName("Льготный приказ")]
        public string IsForBeneficiaryName
        {
            get { return IsForBeneficiary.HasValue ? IsForBeneficiary.Value ? "да" : "нет" : null; }
        }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public string IsForeignerName
        {
            get { return IsForeigner.HasValue ? IsForeigner.Value ? "да" : "нет" : null; }
        }

        [DisplayName("Статус приказа")]
        public string OrderStatusName { get; set; }

        public ApplicationOrderListViewModel ApplicationList
        {
            get { return _applicationList ?? (_applicationList = new ApplicationOrderListViewModel()); }
            set { _applicationList = value; }
        }

        [DisplayName("Наименование приказа")]
        public string OrderName { get; set; }

        [DisplayName("Регистрационный номер")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата приказа")]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Дата приказа")]
        public string OrderDateText
        {
            get { return OrderDate.HasValue ? OrderDate.Value.ToString("dd.MM.yyyy") : string.Empty; }
        }

        [DisplayName("Идентификатор в БД ОО (UID)")]
        public string UID { get; set; }

        public bool IsCampaignFinished { get; set; }

        [DisplayName("Идентификатор последней проверки (UID)")]
        public string CheckOrderUID { get; set; }

        [DisplayName("Статус автоматизированной проверки")]
        public string CheckOrderStatus
        {
            get
            {
                if(checkOrderStatus == true) 
                {
                    return "Проверка завершена успешно (совпадений абитуриентов не обнаружено)";
                };
                if (checkOrderStatus == null)
                {
                    if (dateEdited > checkOrderDate)
                    {
                        return "Нет данных (проверка не проводилась)";
                    }
                    return "Проверка завершена успешно (совпадений абитуриентов не обнаружено)";
                }
                else
                {
                    return "Проверка завершена с ошибкой (обнаружены совпадения абитуриентов)";
                }
            }
            set
            {
                if (value == "False")
                {
                    this.checkOrderStatus = false;
                }
                if (value == null)
                {
                    this.checkOrderStatus = null;
                }
                if (value == "true")
                {
                    this.checkOrderStatus = true;
                }

            }
        } 

        [DisplayName("Дата последней проверки")]
        public string CheckOrderDate 
        { 
            get 
            {
                return string.Concat(checkOrderDate.ToString(),timeZone);
            }
            set
            {

                this.checkOrderDate = Convert.ToDateTime(value.Substring(0,18));
                this.timeZone = value.Substring(19);
            }
        }

        private bool? checkOrderStatus;
        private DateTime? checkOrderDate;
        private DateTime? dateEdited;
        private string timeZone;
    }
}