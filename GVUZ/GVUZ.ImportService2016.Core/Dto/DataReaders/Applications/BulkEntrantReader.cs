using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkEntrantReader : BulkReaderBase<PackageDataApplicationEntrant>
    {
        public BulkEntrantReader(PackageData packageData)
        {
            _records  = packageData.ApplicationsToImport().Select(t => t.Entrant).GroupBy(t => t.UID).Select(g => g.First()).ToList();

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("UID", dto => dto.UID);
            AddGetter("LastName", dto => dto.LastName);
            AddGetter("FirstName", dto => dto.FirstName);
            AddGetter("MiddleName", dto => GetStringOrNull(dto.MiddleName));
            AddGetter("GenderID", dto => dto.GenderID);
            AddGetter("CustomInformation", dto => GetStringOrNull(dto.CustomInformation));

            AddGetter("Email", dto => GetStringOrNull(dto.Email));

            AddGetter("RegionID", dto => GetIntOrNull(dto.RegionID));
            AddGetter("TownTypeID", dto => GetIntOrNull(dto.TownTypeID));
            AddGetter("Address", dto => GetStringOrNull(dto.Address));

            AddGetter("IsFromKrymEntrantDocumentUID", dto => dto.IsFromKrym != null ? dto.IsFromKrym.DocumentUID : (object)DBNull.Value);

            AddGetter("SNILS", dto => GetStringOrNull(dto.SNILS));


        }
    }
}
