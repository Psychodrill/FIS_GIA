using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkApplicationEntranceTestDocumentReader : BulkReaderBase<BulkApplicationEntranceTestDocumentDto>
    {
        public BulkApplicationEntranceTestDocumentReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach (var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(BulkApplicationEntranceTestDocumentDto.GetItems(app, vocabularyStorage));
            }
            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("UID", dto => dto.UID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("ResultValue", dto => dto.ResultValue.HasValue ? dto.ResultValue.Value : (object)DBNull.Value);
            AddGetter("EntranceTestTypeId", dto => GetIntOrNull(dto.EntranceTestTypeId));
            AddGetter("CompetitiveGroupID", dto => GetIntOrNull(dto.CompetitiveGroupId));
            AddGetter("EntranceTestItemID", dto => GetIntOrNull(dto.EntranceTestItemId));
            AddGetter("SubjectId", dto => GetIntOrNull(dto.SubjectId));

            AddGetter("SourceId", dto => GetIntOrNull(dto.SourceId));
            AddGetter("BenefitId", dto => GetIntOrNull(dto.BenefitId));
            AddGetter("InstitutionDocumentTypeId", dto => GetIntOrNull(dto.InstitutionDocumentTypeId));
            AddGetter("InstitutionDocumentDate", dto => GetDateOrNull(dto.InstitutionDocumentDate));
            AddGetter("InstitutionDocumentNumber", dto => GetStringOrNull(dto.InstitutionDocumentNumber));
            AddGetter("EgeDocumentId", dto => GetStringOrNull(dto.EgeDocumentId));
            AddGetter("BenefitEntrantDocumentId", dto => dto.BenefitEntrantDocumentId.HasValue ? dto.BenefitEntrantDocumentId.Value : (object)DBNull.Value);



            AddGetter("EgeResultValue", dto => GetIntOrNull(dto.EgeResultValue));
            AddGetter("ETEntrantDocumentId", dto => dto.ETEntrantDocumentId.HasValue ? dto.ETEntrantDocumentId.Value : (object)DBNull.Value);

            AddGetter("DistantPlace", dto => GetStringOrNull(dto.DistantPlace));
            AddGetter("DisabledDocumentUID", dto => GetStringOrNull(dto.DisabledDocumentUID));

        }
    }

    public class BulkApplicationEntranceTestDocumentDto : ImportBase
    {
        public BulkApplicationEntranceTestDocumentDto() { }

        public BulkApplicationEntranceTestDocumentDto(PackageDataApplication application, PackageDataApplicationEntranceTestResult etResult, VocabularyStorage vocStorage)
        {
            ApplicationGuid = application.GUID;
            UID = etResult.UID;
            SourceId = etResult.ResultSourceTypeID.To(0);

            string subjectName = "";
            if (etResult.EntranceTestSubject != null)
            {
                if (etResult.EntranceTestSubject.Item is uint)
                {
                    var subjectId = ((uint)etResult.EntranceTestSubject.Item).To(0);
                    SubjectId = subjectId;
                    //EntranceTestItemId = vocStorage.EntranceTestItemCVoc.GetIdBySubjectId(subjectId);
                }
                else
                {
                    subjectName = etResult.EntranceTestSubject.Item.ToString();
                    //EntranceTestItemId = vocStorage.EntranceTestItemCVoc.GetIdBySubjectName(subjectName);
                }
            }

            if (etResult.CompetitiveGroupDict != null)
                CompetitiveGroupId = etResult.CompetitiveGroupDict.ID;
            EntranceTestTypeId = etResult.EntranceTestTypeID.To(0);

            if (etResult.ResultSourceTypeID == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.OlympicDocument){
                ResultValue = 100; // Олимпиада => 100 баллов!
                BenefitId = 3;
                EgeResultValue = Convert.ToInt32(etResult.ResultValue);

                //ETEntrantDocumentId = GetEntranceTestDocumentId(etResult);
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TOlympicDocument)
                {
                    TOlympicDocument doc = etResult.ResultDocument.Item as TOlympicDocument;
                    ETEntrantDocumentId = doc.GUID;
                }
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TOlympicTotalDocument)
                {
                    TOlympicTotalDocument doc = etResult.ResultDocument.Item as TOlympicTotalDocument;
                    ETEntrantDocumentId = doc.GUID;
                }
                //FIS - 1768 (30.10.2017 akopylov) не хватает документов для льготы 100 баллов
                //TSportDocument
                //TUkraineOlympic
                //TInternationalOlympic
                //TCustomDocument (не для бак/спец)
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TSportDocument)
                {
                    TSportDocument doc = etResult.ResultDocument.Item as TSportDocument;
                    ETEntrantDocumentId = doc.GUID;
                }
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TUkraineOlympic)
                {
                    TUkraineOlympic doc = etResult.ResultDocument.Item as TUkraineOlympic;
                    ETEntrantDocumentId = doc.GUID;
                }
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TInternationalOlympic)
                {
                    TInternationalOlympic doc = etResult.ResultDocument.Item as TInternationalOlympic;
                    ETEntrantDocumentId = doc.GUID;
                }
                if (etResult.ResultDocument != null && etResult.ResultDocument.Item is TCustomDocument)
                {
                    TCustomDocument doc = etResult.ResultDocument.Item as TCustomDocument;
                    ETEntrantDocumentId = doc.GUID;
                }
            }
            else
                ResultValue = etResult.ResultValue;

            if (SourceId.HasValue && SourceId.Value == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.InstitutionEntranceTest &&
                                      etResult.ResultDocument != null && ((etResult.ResultDocument.Item as TInstitutionDocument) != null))
            {
                var InstitutionDocument = etResult.ResultDocument.Item as TInstitutionDocument;
                InstitutionDocumentTypeId = InstitutionDocument.DocumentTypeID.To(0);
                InstitutionDocumentDate = InstitutionDocument.DocumentDate;
                InstitutionDocumentNumber = InstitutionDocument.DocumentNumber;

                //if (InstitutionDocumentNumber.Length > 50)
                //    InstitutionDocumentNumber = InstitutionDocumentNumber.Substring(0, 50);

            }
            else if (SourceId.HasValue && etResult.ResultDocument != null &&
                (SourceId.Value == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.EgeDocument ||
                 SourceId.Value == (int)GVUZ.Model.Entrants.EntranceTestResultSourceEnum.GiaDocument))
            {
                if (etResult.ResultDocument.Item is string)
                    EgeDocumentId = etResult.ResultDocument.Item.ToString();
            }

            EntranceTestItemId = etResult.EntranceTestItemC != null ? etResult.EntranceTestItemC.EntranceTestItemID : 0;
            DistantPlace = etResult.IsDistant != null ? etResult.IsDistant.DistantPlace : null;
            DisabledDocumentUID = etResult.IsDisabled != null ? etResult.IsDisabled.DisabledDocumentUID : null;

        }

        public BulkApplicationEntranceTestDocumentDto(PackageDataApplication application, PackageDataApplicationApplicationCommonBenefit benefit, VocabularyStorage vocStorage)
        {
            ApplicationGuid = application.GUID;
            UID = benefit.UID;
            BenefitId = benefit.BenefitKindID.To(0);
            CompetitiveGroupId = benefit.CompetitiveGroupDict.ID; 
            BenefitEntrantDocumentId = GetEntranceTestBenefitDocumentId(benefit);
            ResultValue = null;
            SourceId = null;
            EntranceTestTypeId = null;
        }

        public Guid GetEntranceTestBenefitDocumentId(PackageDataApplicationApplicationCommonBenefit benefit)
        {
            ImportBase document = GetEntranceTestBenefitDocument(benefit);
            return document != null ? document.GUID : Guid.Empty;
        }
        public ImportBase GetEntranceTestBenefitDocument(PackageDataApplicationApplicationCommonBenefit benefit)
        {
            /* Основание для льготы должно быть обязательно для валидности xml */
            if (benefit.DocumentReason == null) return null;

            //[System.Xml.Serialization.XmlElementAttribute("CustomDocument", typeof(TCustomDocument))]
            //[System.Xml.Serialization.XmlElementAttribute("InternationalOlympicDocument", typeof(TInternationalOlympic))]
            //[System.Xml.Serialization.XmlElementAttribute("MedicalDocuments", typeof(PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments))]
            //[System.Xml.Serialization.XmlElementAttribute("OlympicDocument", typeof(TOlympicDocument))]
            //[System.Xml.Serialization.XmlElementAttribute("OlympicTotalDocument", typeof(TOlympicTotalDocument))]
            //[System.Xml.Serialization.XmlElementAttribute("OrphanDocument", typeof(TOrphanDocument))]
            //[System.Xml.Serialization.XmlElementAttribute("SportDocument", typeof(TSportDocument))]
            //[System.Xml.Serialization.XmlElementAttribute("UkraineOlympicDocument", typeof(TUkraineOlympic))]
            //[System.Xml.Serialization.XmlElementAttribute("VeteranDocument", typeof(TVeteranDocument))]
            var MedicalDocument = benefit.DocumentReason.Item as PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments;

            if (MedicalDocument != null  && MedicalDocument.BenefitDocument != null)
            {
                /* Справка об установлении инвалидности */
                var DisabilityDocument = MedicalDocument.BenefitDocument.Item as TDisabilityDocument;
                if (DisabilityDocument != null) return DisabilityDocument;

                /* Заключение психолого-медико-педагогической комиссии */
                var InnerMedicalDocument = MedicalDocument.BenefitDocument.Item as TMedicalDocument;
                if (InnerMedicalDocument != null) return InnerMedicalDocument;
            }

            var CustomDocument = benefit.DocumentReason.Item as TCustomDocument;
            if (CustomDocument != null) return CustomDocument;

            var OlympicDocument = benefit.DocumentReason.Item as TOlympicDocument;
            if (OlympicDocument != null) return OlympicDocument;

            var OlympicTotalDocument = benefit.DocumentReason.Item as TOlympicTotalDocument;
            if (OlympicTotalDocument != null) return OlympicTotalDocument;

            var InternationalOlympic = benefit.DocumentReason.Item as TInternationalOlympic;
            if (InternationalOlympic != null) return InternationalOlympic;

            var orphanDocument = benefit.DocumentReason.Item as TOrphanDocument;
            if (orphanDocument != null) return orphanDocument;

            var sportDocument = benefit.DocumentReason.Item as TSportDocument;
            if (sportDocument != null) return sportDocument;

            var ukraineOlympic = benefit.DocumentReason.Item as TUkraineOlympic;
            if (ukraineOlympic != null) return ukraineOlympic;

            var veteranDocument = benefit.DocumentReason.Item as TVeteranDocument;
            if (veteranDocument != null) return veteranDocument;

            var pauperDocument = benefit.DocumentReason.Item as TPauperDocument;
            if (pauperDocument != null) return pauperDocument;

            var parentsLostDocument = benefit.DocumentReason.Item as TParentsLostDocument;
            if (parentsLostDocument != null) return parentsLostDocument;

            var stateEmployeeDocument = benefit.DocumentReason.Item as TStateEmployeeDocument;
            if (stateEmployeeDocument != null) return stateEmployeeDocument;

            var radiationWorkDocument = benefit.DocumentReason.Item as TRadiationWorkDocument;
            if (radiationWorkDocument != null) return radiationWorkDocument;

            return null;
        }

        public static List<BulkApplicationEntranceTestDocumentDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            var res = new List<BulkApplicationEntranceTestDocumentDto>();
            // В ApplicationEntranceTestDocument попадают:
            // 1. Application\EntranceTestResults
            if (application.EntranceTestResults != null)
            {
                foreach (var etResult in application.EntranceTestResults)
                {
                    res.Add(new BulkApplicationEntranceTestDocumentDto(application, etResult, vocStorage));
                }
            }

            // 2. Application\ApplicationCommonBenefit(s)
            foreach (var benefit in application.getAllBenefits())
            {
                res.Add(new BulkApplicationEntranceTestDocumentDto(application, benefit, vocStorage));
            }
            return res;
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public string UID { get; set; }

        public decimal? ResultValue { get; set; }
        public int? EntranceTestTypeId { get; set; }
        public int? CompetitiveGroupId { get; set; }
        public int? EntranceTestItemId { get; set; }
        public int? SourceId { get; set; }
        public int? SubjectId { get; set; }
        public int? BenefitId { get; set; }
        public int? InstitutionDocumentTypeId { get; set; }
        public DateTime? InstitutionDocumentDate { get; set; }
        public string InstitutionDocumentNumber { get; set; }
        /// <summary>
        /// UID ЕГЭ/ГИА документа
        /// </summary>
        public string EgeDocumentId { get; set; }
        public Guid? BenefitEntrantDocumentId { get; set; }

        public int? EgeResultValue { get; set; }

        public Guid? ETEntrantDocumentId { get; set; }
        public string DistantPlace { get; set; }
        public string DisabledDocumentUID { get; set; }
    }
}
