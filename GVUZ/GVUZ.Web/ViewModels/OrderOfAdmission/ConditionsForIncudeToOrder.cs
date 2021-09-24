using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ConditionForIncudeToOrder
    {
        public List<Condition> Conditions;

        public ErrorTypes ErrorId { get; set; }
        public string ErrorMessage
        {
            get
            {
                if (ErrorId == ErrorTypes.PackageError_Other)
                {
                    return "Выбранные заявления не удовлетворяют условиям включения в приказ";
                }
                if (ErrorId == ErrorTypes.PackageError_DistinctBenefits)
                {
                    return "Пакетное включение в приказ доступно для заявлений, содержащих равное количество одинаковых льгот";
                }
                if (ErrorId == ErrorTypes.PackageError_DistinctCountries)
                {
                    return "Пакетное включение в приказ доступно для заявлений, поданных абитуриентами - гражданами одной страны";
                }
                if (ErrorId == ErrorTypes.NoApplicationItems)
                {
                    return "Для включения в приказ выбранные заявления должны содержать хотя бы одно условие приема";
                }
                if (ErrorId == ErrorTypes.NoAgreement)
                {
                    return "Для включения в приказ выбранные заявления должны содержать согласие по одному из условий приема";
                }
                return "";
            }
        }

        public class Condition
        {
            public int ApplicationId { get; set; }
            public int ApplicationCompetitiveGroupItemId { get; set; }
            public int? CompetitiveGroupID { get; set; }
            public int? EducationFormID { get; set; }
            public int? EducationSourceID { get; set; }
            public int? CampaignID { get; set; }
            public int? EducationLevelID { get; set; }
            public bool IsAgreed { get; set; }
            public string BenefitIDs { get; set; }
        }

        public enum ErrorTypes
        {
            /// <summary>
            /// Ошибок нет
            /// </summary>
            None,
            /// <summary>
            /// Выбранные заявления не удовлетворяют условиям включения в приказ
            /// </summary>
            PackageError_Other,
            PackageError_DistinctBenefits,
            PackageError_DistinctCountries,
            /// <summary>
            /// Для включения в приказ выбранные заявления должны содержать хотя бы одно условие приема
            /// </summary>
            NoApplicationItems,
            /// <summary>
            /// Для включения в приказ выбранные заявления должны содержать согласие по одному из условий приема
            /// </summary>
            NoAgreement,
        }
    }
}