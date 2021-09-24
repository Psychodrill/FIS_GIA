using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkCompetitiveGroupTargetReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupTargetOrganization>
    {
        public BulkCompetitiveGroupTargetReader(PackageData packageData)
        {
            Dictionary<string, Guid> items = new Dictionary<string, Guid>();
            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.TargetOrganizations != null)
                    foreach (var cgt in cg.TargetOrganizations)
                    {
                        if (!items.ContainsKey(cgt.UID))
                        {
                            // Если запись с таким UID еще на добавляли - добавляем и запоним ее Guid
                            items.Add(cgt.UID, cgt.GUID);
                            cgt.ParentID = cg.GUID;
                            _records.Add(cgt);
                        }
                        else
                        {
                            // Значит запись с таким UID уже добавили, поставим Guid той записи, чтобы на нее прицепить дочерних CGTI
                            cgt.GUID = items[cgt.UID];
                        }
                    }

            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.ParentID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

            AddGetter("Name", dto => string.Empty); // dto.TargetOrganizationName);

            AddGetter("UID", dto => dto.UID);


        }

    }
}
