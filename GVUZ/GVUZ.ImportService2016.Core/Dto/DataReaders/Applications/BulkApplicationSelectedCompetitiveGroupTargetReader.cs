using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.Model.Institutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkApplicationSelectedCompetitiveGroupTargetReader : BulkReaderBase<BulkApplicationSelectedCompetitiveGroupTargetDto>
    {
        public BulkApplicationSelectedCompetitiveGroupTargetReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach (var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(BulkApplicationSelectedCompetitiveGroupTargetDto.GetItems(app, vocabularyStorage));
            }
            //_records = packageData.Applications.Select(t=> new  {t.UID, t.SelectedCompetitiveGroups}).Select(p=> new BulkApplicationSelectedCompetitiveGroupDto(p. ){} ).ToList();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("CompetitiveGroupTargetID", dto => dto.CompetitiveGroupTargetID);
            AddGetter("IsForO", dto => dto.IsForO);
            AddGetter("IsForOZ", dto => dto.IsForOZ);
            AddGetter("IsForZ", dto => dto.IsForZ);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);
        }
    }

    public class BulkApplicationSelectedCompetitiveGroupTargetDto : ImportBase
    {
        public BulkApplicationSelectedCompetitiveGroupTargetDto() { }

        public BulkApplicationSelectedCompetitiveGroupTargetDto(PackageDataApplication application, PackageDataApplicationFinSourceEduForm finSource, VocabularyStorage vocStorage) 
        {
            ApplicationGuid = application.GUID;
            CompetitiveGroupTargetID = vocStorage.CompetitiveGroupTargetVoc.GetIdByUid(finSource.TargetOrganizationUID);
            IsForO = finSource.EducationFormID == EDFormsConst.O;
            IsForOZ = finSource.EducationFormID == EDFormsConst.OZ;
            IsForZ = finSource.EducationFormID == EDFormsConst.Z;
        }

        public static List<BulkApplicationSelectedCompetitiveGroupTargetDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            List<BulkApplicationSelectedCompetitiveGroupTargetDto> res = new List<BulkApplicationSelectedCompetitiveGroupTargetDto>();
            if (application.FinSourceAndEduForms != null)
                foreach (var finSource in application.FinSourceAndEduForms)
                {
                    if (finSource.FinanceSourceID == EDSourceConst.Target && !string.IsNullOrEmpty(finSource.TargetOrganizationUID))
                        res.Add(new BulkApplicationSelectedCompetitiveGroupTargetDto(application, finSource, vocStorage));
                }

            return res;
        }
        
        public Guid ApplicationGuid { get; set; }
        public int CompetitiveGroupTargetID { get; set; }
        public bool IsForO { get; set; }
        public bool IsForOZ { get; set; }
        public bool IsForZ { get; set; }
    }
}
