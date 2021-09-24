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
    public class BulkApplicationSelectedCompetitiveGroupReader : BulkReaderBase<BulkApplicationSelectedCompetitiveGroupDto>
    {
        public BulkApplicationSelectedCompetitiveGroupReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach(var app in packageData.ApplicationsToImport()){
                _records.AddRange(BulkApplicationSelectedCompetitiveGroupDto.GetItems(app, vocabularyStorage));
            }
            //_records = packageData.Applications.Select(t=> new  {t.UID, t.SelectedCompetitiveGroups}).Select(p=> new BulkApplicationSelectedCompetitiveGroupDto(p. ){} ).ToList();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("UID", dto => dto.UID);
            AddGetter("CompetitiveGroupID", dto => dto.CompetitiveGroupID);
            AddGetter("CalculatedRating", dto => dto.CalculatedRating);
            AddGetter("CalculatedBenefitId", dto => dto.CalculatedBenefitID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);
        }
    }

    public class BulkApplicationSelectedCompetitiveGroupDto : ImportBase
    {
        public BulkApplicationSelectedCompetitiveGroupDto() { }

        public BulkApplicationSelectedCompetitiveGroupDto(PackageDataApplication application, CompetitiveGroupDict competitiveGroup, VocabularyStorage vocStorage)
        {
            ApplicationUID = application.UID;
            ApplicationGuid = application.GUID;
            CompetitiveGroupID = competitiveGroup.ID;
            UID = competitiveGroup.UID;

            CalculatedRating = null;
            if (application.EntranceTestResults != null)
                CalculatedRating = application.EntranceTestResults.Where(t => t.CompetitiveGroupID == competitiveGroup.UID).Sum(x => x.ResultValue);

            CalculatedBenefitID = null;
            // Если сущ. ApplicationEntranceTestDocument  с ApplicationID, CompetitiveGroupID и EntranceTestItemID == null и BenefitID != null, то CalculatedBenefitID = BenefitID
            // т.е. есть документ в Benefit
            var benefit = application.getAllBenefits().Where(t => t.CompetitiveGroupID == competitiveGroup.UID /*&& t.BenefitKindID!=null */
                            && !(t.DocumentReason != null && t.DocumentReason.Item != null && (t.DocumentReason.Item is TOlympicTotalDocument || t.DocumentReason.Item is TOlympicDocument))).FirstOrDefault();
            if (benefit != null)
                CalculatedBenefitID = benefit.BenefitKindID.To(0);
            else
            {
                // Или если сущ. ApplicationEntranceTestDocument  с ApplicationID, CompetitiveGroupID и EntranceTestItemID != null и BenefitID != null и !=2, то CalculatedBenefitID = BenefitID
                var benefitAlso = application.getAllBenefits().Where(t => t.CompetitiveGroupID == competitiveGroup.UID
                    && (/*t.BenefitKindID != null &&*/ t.BenefitKindID != 2 && t.BenefitKindID != 0)
                    && (t.DocumentReason != null && t.DocumentReason.Item != null
                        && (t.DocumentReason.Item.GetType() == typeof(TOlympicTotalDocument) || t.DocumentReason.Item is TOlympicDocument))).FirstOrDefault();
                if (benefitAlso != null)
                    CalculatedBenefitID = benefitAlso.BenefitKindID.To(0);
            }
        }

        public static List<BulkApplicationSelectedCompetitiveGroupDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            List<BulkApplicationSelectedCompetitiveGroupDto> res = new List<BulkApplicationSelectedCompetitiveGroupDto>();
            foreach (var cg in application.SelectedCompetitiveGroupsFull)
            {
                res.Add(new BulkApplicationSelectedCompetitiveGroupDto(application, cg, vocStorage));
            }

            return res;
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public int CompetitiveGroupID { get; set; }
        public string UID { get; set; }

        /// <summary>
        /// Если сущ. ApplicationEntranceTestDocument  с ApplicationID, CompetitiveGroupID и EntranceTestItemID == null и BenefitID != null, то CalculatedBenefitID = BenefitID
        /// Или если сущ. ApplicationEntranceTestDocument  с ApplicationID, CompetitiveGroupID и EntranceTestItemID != null и BenefitID != null и !=2, то CalculatedBenefitID = BenefitID
        /// </summary>
        public int? CalculatedBenefitID { get; set; }

        /// <summary>
        /// Вычисляется как сумма ApplicationEntranceTestDocument.ResultValue у всех с EntranceTestItemC.CompetitiveGroupID = CompetitiveGroupID
        /// </summary>
        public decimal? CalculatedRating { get; set; }
    }
}
