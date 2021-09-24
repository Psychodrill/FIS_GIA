using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.TargetOrganizations
{
    class BulkTargetOrganizationReader : BulkReaderBase<PackageDataTargetOrganization>
    {
        public BulkTargetOrganizationReader(PackageData packageData)
        {
            _records = packageData.TargetOrganizationsToImport();

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);
 
            AddGetter("Name", dto => DBNull.Value);
            AddGetter("UID", dto => dto.UID);
            AddGetter("ContractOrganizationName", dto => dto.ContractOrganizationName);
            AddGetter("HaveContract", dto => dto.HaveContract);
            AddGetter("ContractNumber", dto => dto.ContractNumber);
            AddGetter("ContractDate", dto => dto.ContractDate);
            AddGetter("ContractOrganizationOGRN", dto => dto.ContractOrganizationOGRN);
            AddGetter("ContractOrganizationKPP", dto => dto.ContractOrganizationKPP);
            AddGetter("EmployerOrganizationName", dto => dto.EmployerOrganizationName);
            AddGetter("EmployerOrganizationOGRN", dto => dto.EmployerOrganizationOGRN);
            AddGetter("EmployerOrganizationKPP", dto => dto.EmployerOrganizationKPP);
            AddGetter("LocationEmployerOrganizations", dto => dto.LocationEmployerOrganizations);

        }
    }
}
