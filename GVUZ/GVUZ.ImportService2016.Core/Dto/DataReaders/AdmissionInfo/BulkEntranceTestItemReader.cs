using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkEntranceTestItemReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupEntranceTestItem>
    {
        public BulkEntranceTestItemReader(PackageData packageData)
        {

            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.EntranceTestItems != null)
                    foreach (var e in cg.EntranceTestItems)
                    {
                        e.ParentID = cg.GUID;
                        _records.Add(e);

                    }
            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.ParentID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

            AddGetter("EntranceTestTypeID", dto => dto.EntranceTestTypeID.To(0));
            AddGetter("Form", dto => string.Empty); // dto.Form);
	        AddGetter("MinScore", dto => dto.MinScore);
            AddGetter("SubjectID", dto => GetIntOrNull(dto.SubjectID));
            AddGetter("SubjectName", dto => GetStringOrNull(dto.SubjectName));
	        AddGetter("UID", dto => dto.UID);
	        AddGetter("EntranceTestPriority", dto => dto.EntranceTestPriority);

            AddGetter("ReplacedEntranceTestItemUID", dto => dto.IsForSPOandVO != null ? (object)dto.IsForSPOandVO.ReplacedEntranceTestItemUID : DBNull.Value);
            AddGetter("IsFirst", dto => dto.IsFirst);
            AddGetter("IsSecond", dto => dto.IsSecond);
        }
    }
}