using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkAdmissionVolumeReader : BulkReaderBase<PackageDataAdmissionInfoItem>
    {
        public BulkAdmissionVolumeReader(PackageData packageData)
        {
            _records = packageData.AdmissionVolumesToImport();

            AddGetter("ID", dto => dto.ID);  
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("AdmissionItemTypeID", dto => dto.EducationLevelID);
            AddGetter("DirectionID", dto => GetIntOrNull(dto.DirectionID));
           
            AddGetter("NumberBudgetO", dto => dto.NumberBudgetO.To(0));
            AddGetter("NumberBudgetOZ", dto => dto.NumberBudgetOZ.To(0));
            AddGetter("NumberBudgetZ", dto => dto.NumberBudgetZ.To(0));
            AddGetter("NumberPaidO", dto => dto.NumberPaidO.To(0));
            AddGetter("NumberPaidOZ", dto => dto.NumberPaidOZ.To(0));
            AddGetter("NumberPaidZ", dto => dto.NumberPaidZ.To(0));

            AddGetter("UID", dto => dto.UID);
            AddGetter("Course", dto => 0); // dto.Course.To(0));
            AddGetter("CampaignID", dto => dto.CampaignID);

            AddGetter("NumberTargetO", dto => dto.NumberTargetO.To(0));
            AddGetter("NumberTargetOZ", dto => dto.NumberTargetOZ.To(0));
            AddGetter("NumberTargetZ", dto => dto.NumberTargetZ.To(0));
            AddGetter("NumberQuotaO", dto => dto.NumberQuotaO.To(0));
            AddGetter("NumberQuotaOZ", dto => dto.NumberQuotaOZ.To(0));
            AddGetter("NumberQuotaZ", dto => dto.NumberQuotaZ.To(0));

            AddGetter("IsPlan", dto => dto.IsPlan);
            AddGetter("ParentDirectionID", dto => GetIntOrNull(dto.ParentDirectionID));
        }
    }
}
