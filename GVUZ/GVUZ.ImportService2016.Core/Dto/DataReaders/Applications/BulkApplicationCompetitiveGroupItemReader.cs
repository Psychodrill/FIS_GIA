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

    public class BulkApplicationCompetitiveGroupItemReader : BulkReaderBase<BulkApplicationCompetitiveGroupItemDto>
    {
        public BulkApplicationCompetitiveGroupItemReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach (var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(BulkApplicationCompetitiveGroupItemDto.GetItems(app, vocabularyStorage));
            }

            AddGetter("Id", dto => dto.GUID);
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("CompetitiveGroupID", dto => dto.CompetitiveGroupID);
            AddGetter("CompetitiveGroupItemID", dto => dto.CompetitiveGroupItemID);
            AddGetter("CompetitiveGroupTargetID", dto => GetIntOrNull(dto.CompetitiveGroupTargetID)); // dto.CompetitiveGroupTargetID != 0 ?  dto.CompetitiveGroupTargetID  : (object)DBNull.Value);
            AddGetter("EducationForm", dto => dto.EducationForm);
            AddGetter("EducationSource", dto => dto.EducationSource);

            AddGetter("Priority", dto => 0);
            AddGetter("IsAgreedDate", dto => GetDateOrNull(dto.IsAgreedDate));
            AddGetter("IsForSPOandVO", dto => dto.IsForSPOandVO);

            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("IsDisagreedDate", dto => GetDateOrNull(dto.IsDisagreedDate));
        }
    }

    public class BulkApplicationCompetitiveGroupItemDto : ImportBase
    {
        public BulkApplicationCompetitiveGroupItemDto() { }

        public BulkApplicationCompetitiveGroupItemDto(PackageDataApplication application, PackageDataApplicationFinSourceEduForm finSource, VocabularyStorage vocStorage)
        {
            ApplicationUID = application.UID;
            ApplicationGuid = application.GUID;
            var competitiveGroup = vocStorage.CompetitiveGroupVoc.GetItemByUid(finSource.CompetitiveGroupUID);
            CompetitiveGroupID = competitiveGroup.ID;
            CompetitiveGroupTargetID = (!string.IsNullOrEmpty(finSource.TargetOrganizationUID) && (competitiveGroup.EducationSourceId == GVUZ.Model.Institutions.AdmissionItemTypeConstants.TargetReception)) ? vocStorage.CompetitiveGroupTargetVoc.GetIdByUid(finSource.TargetOrganizationUID) : 0;
            EducationForm = competitiveGroup.EducationFormId;
            EducationSource = competitiveGroup.EducationSourceId;
            IsAgreedDate = finSource.IsAgreedDateSpecified ? (DateTime?)finSource.IsAgreedDate : null;
            IsDisagreedDate = finSource.IsDisagreedDateSpecified ? (DateTime?)finSource.IsDisagreedDate : null;
            IsForSPOandVO = finSource.IsForSPOandVO;
        }

        public static List<BulkApplicationCompetitiveGroupItemDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            var res = new List<BulkApplicationCompetitiveGroupItemDto>();
            foreach (var finSource in application.FinSourceAndEduForms.Where(t => !t.IsBroken))
            {
                var competitiveGroup = vocStorage.CompetitiveGroupVoc.GetItemByUid(finSource.CompetitiveGroupUID);
                if ((competitiveGroup == null) && string.IsNullOrEmpty(competitiveGroup.UID))
                {
                    // Все 3 новых поля пусты, значит передаётся старая структура - логика позаимствована из старого сервиса

                    foreach (var groupItem in application.SelectedCompetitiveGroupsFull)
                    {
                        res.Add(new BulkApplicationCompetitiveGroupItemDto()
                        {
                            ApplicationUID = application.UID,
                            ApplicationGuid = application.GUID,
                            CompetitiveGroupID = groupItem.ID,
                            //CompetitiveGroupItemID = groupItem.ID,
                            CompetitiveGroupTargetID = 0,
                            EducationForm = competitiveGroup.EducationFormId,
                            EducationSource = competitiveGroup.EducationSourceId,
                            IsAgreedDate = finSource.IsAgreedDateSpecified ? (DateTime?)finSource.IsAgreedDate : null,
                            IsDisagreedDate = finSource.IsDisagreedDateSpecified ? (DateTime?)finSource.IsDisagreedDate : null,
                            IsForSPOandVO = finSource.IsForSPOandVO
                        });
                    }
                }
                else
                {
                    res.Add(new BulkApplicationCompetitiveGroupItemDto(application, finSource, vocStorage));
                }
            }

            return res;
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public int CompetitiveGroupID { get; set; }
        public int CompetitiveGroupItemID { get; set; }
        public int CompetitiveGroupTargetID { get; set; }

        public int EducationForm { get; set; }
        public int EducationSource { get; set; }

        public string UID { get; set; }
        public DateTime? IsAgreedDate { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
        public bool IsForSPOandVO { get; set; }
    }
}
