using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkCompetitiveGroupReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroup>
    {
        public BulkCompetitiveGroupReader(PackageData packageData)
        {
            _records = packageData.CompetitiveGroupsToImport();

            AddGetter("ID", dto => dto.ID);  
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("Name", dto => dto.Name);
            AddGetter("Course", dto => 1);// dto.Course.To(0));
            AddGetter("UID", dto => dto.UID);
            AddGetter("CampaignID", dto => dto.CampaignID);

            AddGetter("DirectionID", dto => GetIntOrNull(dto.DirectionID.To(0)));
            AddGetter("EducationFormID", dto => dto.EducationFormID);
            AddGetter("EducationLevelID", dto => dto.EducationLevelID);
            AddGetter("EducationSourceID", dto => dto.EducationSourceID);

            AddGetter("IsFromKrym", dto => dto.IsForKrym);
            AddGetter("IsAdditional", dto => dto.IsAdditional);

            AddGetter("LevelBudget", dto => dto.LevelBudgetSpecified ? dto.LevelBudget.To(0) : (object)DBNull.Value);
            AddGetter("ParentDirectionID", dto => GetIntOrNull(dto.ParentDirectionID.To(0)));
            AddGetter("StudyPeriod", dto => GetStringOrNull(dto.StudyPeriod));
            AddGetter("StudyBeginningDate", dto => GetStringOrNull(dto.StudyBeginningDate));
            AddGetter("StudyEndingDate", dto => GetStringOrNull(dto.StudyEndingDate));
        }
    }
}
