using System;
using System.ComponentModel;
using System.Xml.Serialization;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    [Description("Общая льгота заявления")]
    public class ApplicationCommonBenefitDto : BaseDto
    {
        public string ApplicationCommonBenefitID;
        public string BenefitKindID;
        public string CompetitiveGroupID;
        public ApplicationCommonBenefitDocumentsDto DocumentReason;
        public string DocumentTypeID;

        /// <summary>
        ///     для внутренних целей
        /// </summary>
        [XmlIgnore] public int? EntrantDocumentID;

        //для экспорта
    }

    public static class ApplicationCommonBenefitDtoExtensions
    {
        /// <summary>
        ///     Поиск основания для льготы
        /// </summary>
        /// <param name="benefit"></param>
        /// <returns></returns>
        public static BaseDocumentDto GetEntranceTestBenefitDocument(this ApplicationCommonBenefitDto benefit)
        {
            /* Основание для льготы должно быть обязательно для валидности xml */
            if (benefit.DocumentReason == null) return null;
            if (benefit.DocumentReason.MedicalDocuments != null &&
                benefit.DocumentReason.MedicalDocuments.BenefitDocument != null)
            {
                BenefitDocumentsDto document = benefit.DocumentReason.MedicalDocuments.BenefitDocument;
                /* Справка об установлении инвалидности */
                if (document.DisabilityDocument != null) return document.DisabilityDocument;
                /* Заключение психолого-медико-педагогической комиссии */
                if (document.MedicalDocument != null) return document.MedicalDocument;
            }
            if (benefit.DocumentReason.CustomDocument != null)
                return benefit.DocumentReason.CustomDocument;

            if (benefit.DocumentReason.OlympicDocument != null)
                return benefit.DocumentReason.OlympicDocument;

            if (benefit.DocumentReason.OlympicTotalDocument != null)
                return benefit.DocumentReason.OlympicTotalDocument;

            return null;
        }

        public static Guid GetEntranceTestBenefitDocumentId(this ApplicationCommonBenefitDto benefit)
        {
            BaseDocumentDto document = benefit.GetEntranceTestBenefitDocument();
            return document != null ? document.Id : Guid.Empty;
        }
    }
}