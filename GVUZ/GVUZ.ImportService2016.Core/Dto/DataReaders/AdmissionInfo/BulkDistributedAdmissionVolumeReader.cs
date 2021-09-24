using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkDistributedAdmissionVolumeReader : BulkReaderBase<PackageDataAdmissionInfoItem1>
    {
        public BulkDistributedAdmissionVolumeReader(PackageData packageData)
        {
            _records = packageData.DistributedAdmissionVolumesToImport();

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("AdmissionVolumeID", dto => dto.AdmissionVolumeID);
            AddGetter("AdmissionVolumeGUID", dto => dto.AdmissionVolumeGUID);

            AddGetter("IdLevelBudget", dto => dto.LevelBudget.To(0));

            AddGetter("NumberBudgetO", dto => dto.NumberBudgetO.To(0));
            AddGetter("NumberBudgetOZ", dto => dto.NumberBudgetOZ.To(0));
            AddGetter("NumberBudgetZ", dto => dto.NumberBudgetZ.To(0));

            AddGetter("NumberQuotaO", dto => dto.NumberQuotaO.To(0));
            AddGetter("NumberQuotaOZ", dto => dto.NumberQuotaOZ.To(0));
            AddGetter("NumberQuotaZ", dto => dto.NumberQuotaZ.To(0));

            AddGetter("NumberTargetO", dto => dto.NumberTargetO.To(0));
            AddGetter("NumberTargetOZ", dto => dto.NumberTargetOZ.To(0));
            AddGetter("NumberTargetZ", dto => dto.NumberTargetZ.To(0));

            AddGetter("IsPlan", dto => dto.IsPlan);
        }
    }
}
