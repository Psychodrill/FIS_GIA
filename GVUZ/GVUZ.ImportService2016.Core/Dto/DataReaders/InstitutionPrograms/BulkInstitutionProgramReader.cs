using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.InstitutionPrograms
{
    class BulkInstitutionProgramReader : BulkReaderBase<PackageDataInstitutionProgram>
    {
        public BulkInstitutionProgramReader(PackageData packageData)
        {
            _records = packageData.InstitutionProgramToImport();

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);
           
            AddGetter("UID", dto => dto.UID);
            AddGetter("Name", dto => dto.Name);
            AddGetter("Code", dto => dto.Code);
        }
    }
}
