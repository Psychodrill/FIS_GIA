using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkCampaignReader : BulkReaderBase<PackageDataCampaignInfoCampaign>
    {
        public BulkCampaignReader(PackageData packageData)
        {
            _records = packageData.CampaignsToImport();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("UID", dto => dto.UID);
            AddGetter("Name", dto => dto.Name);

            AddGetter("YearStart", dto => dto.YearStart.To(0));
            AddGetter("YearEnd", dto => dto.YearEnd.To(0));
            AddGetter("EducationFormFlag", dto => dto.EducationFormFlag());
            AddGetter("StatusID", dto => dto.StatusID.To(0));
            AddGetter("CampaignTypeID", dto => dto.CampaignTypeID.To(0));
        }
    }
}