using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.InstitutionArchievements
{
    class BulkInstitutionAchievementsReader : BulkReaderBase<PackageDataInstitutionAchievement>
    {
        public BulkInstitutionAchievementsReader(PackageData packageData)
        {
            _records = packageData.InstitutionAchievementsToImport();

            AddGetter("Id", dto => dto.ID);  
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);

            AddGetter("UID", dto => dto.InstitutionAchievementUID);
            AddGetter("Name", dto => dto.Name);
            AddGetter("IdCategory", dto => dto.IdCategory.To(0));
            AddGetter("MaxValue", dto => dto.MaxValue);
            AddGetter("CampaignID", dto => dto.CampaignID);

            
        }
    }
}
