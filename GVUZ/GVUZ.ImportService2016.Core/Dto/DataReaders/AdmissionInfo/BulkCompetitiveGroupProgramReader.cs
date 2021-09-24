using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkCompetitiveGroupProgramReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupEduProgram>
    {
        public BulkCompetitiveGroupProgramReader(PackageData packageData)
        {
            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.EduPrograms != null)
                foreach (var program in cg.EduPrograms)
                {
                    program.ParentID = cg.GUID;
                    _records.Add(program);
                }
            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.ParentID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

            AddGetter("InstitutionProgramID", dto => dto.InstitutionProgramID);
            AddGetter("UID", dto => GetStringOrNull(dto.UID));
            
            //AddGetter("Name", dto => dto.Name);
            //AddGetter("Code", dto => GetStringOrNull(dto.Code));

        }
    }
}
