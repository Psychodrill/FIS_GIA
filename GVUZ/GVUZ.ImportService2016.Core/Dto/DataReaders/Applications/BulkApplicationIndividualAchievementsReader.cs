using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{

    public class BulkApplicationIndividualAchievementsReader : BulkReaderBase<BulkApplicationIndividualAchievementsDto>
    {
    public BulkApplicationIndividualAchievementsReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach (var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(BulkApplicationIndividualAchievementsDto.GetItems(app, vocabularyStorage));
            }
            //_records = packageData.Applications.Select(t=> new  {t.UID, t.SelectedCompetitiveGroups}).Select(p=> new BulkApplicationSelectedCompetitiveGroupDto(p. ){} ).ToList();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("IAUID", dto => GetStringOrNull(dto.IAUID));
            AddGetter("IAName", dto => dto.IAName);
            AddGetter("IAMark", dto => dto.IAMark.HasValue ? dto.IAMark.Value : (object)DBNull.Value);
            AddGetter("EntrantDocumentUID", dto => dto.EntrantDocumentUID);
            AddGetter("EntrantDocumentID", dto =>  GetIntOrNull(dto.EntrantDocumentID));
            AddGetter("InstitutionArchievementID", dto => GetIntOrNull(dto.InstitutionArchievementID));
            AddGetter("isAdvantageRight", dto => dto.isAdvantageRight);
        }
    }

    public class BulkApplicationIndividualAchievementsDto : ImportBase
    {
        public BulkApplicationIndividualAchievementsDto() { }

        public BulkApplicationIndividualAchievementsDto(PackageDataApplication application, PackageDataApplicationIndividualAchievement archievement, VocabularyStorage vocStorage) 
        {
            ApplicationGuid = application.GUID;
            IAUID = archievement.IAUID;

            IAMark = archievement.IAMarkSpecified ? (decimal?)Math.Round(archievement.IAMark, 2) : null;
            
            EntrantDocumentUID = archievement.IADocumentUID;
            EntrantDocumentID = archievement.EntrantDocumentID;
            InstitutionArchievementID = archievement.InstitutionAchievementID;
            isAdvantageRight = archievement.isAdvantageRight;
        }

        public static List<BulkApplicationIndividualAchievementsDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            var res = new List<BulkApplicationIndividualAchievementsDto>();
            if (application.IndividualAchievements!=null)
                foreach (var archievement in application.IndividualAchievements)
                {
                    res.Add(new BulkApplicationIndividualAchievementsDto(application, archievement, vocStorage));
                }

            return res;
        }
        
        public Guid ApplicationGuid { get; set; }
        public string ApplicationUID { get; set; }
        public string IAUID { get; set; }
        public string IAName { get; set; }
        public decimal? IAMark { get; set; }
        public string EntrantDocumentUID { get; set; }
        public int? EntrantDocumentID { get; set; }
        public int? InstitutionArchievementID { get; set; }
        public bool? isAdvantageRight { get; set; }
    }
}
