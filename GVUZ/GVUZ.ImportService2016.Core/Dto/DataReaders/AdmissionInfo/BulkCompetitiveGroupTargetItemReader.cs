using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkCompetitiveGroupTargetItemReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupTargetOrganizationCompetitiveGroupTargetItem>
    {
        public BulkCompetitiveGroupTargetItemReader(PackageData packageData)
        {
            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.TargetOrganizations != null)
                    foreach (var cgt in cg.TargetOrganizations)
                    {
                        if (cgt.CompetitiveGroupTargetItem != null || cg.EducationSourceID == 16)
                        //foreach (var cgti in cgt.Items)
                        {
                            var cgti = cgt.CompetitiveGroupTargetItem == null ? new PackageDataAdmissionInfoCompetitiveGroupTargetOrganizationCompetitiveGroupTargetItem() : cgt.CompetitiveGroupTargetItem;
                            cgti.TargetID = cgt.ID;
                            cgti.CompetitiveGroupID = cg.ID;
                            cgti.CompetitiveGroupGUID = cg.GUID;
                            _records.Add(cgti);
                        }
                    }
            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

            AddGetter("NumberTargetO", dto => dto.ItemElementName == ItemChoiceType1.NumberTargetO ? dto.Item : 0); 
            AddGetter("NumberTargetOZ", dto => dto.ItemElementName == ItemChoiceType1.NumberTargetOZ ? dto.Item : 0);
            AddGetter("NumberTargetZ", dto => dto.ItemElementName == ItemChoiceType1.NumberTargetZ ? dto.Item : 0);

            AddGetter("TargetID", dto => dto.TargetID);
            AddGetter("CompetitiveGroupID", dto => dto.CompetitiveGroupID);
            AddGetter("CompetitiveGroupGUID", dto => dto.CompetitiveGroupGUID);
        }
    }
}