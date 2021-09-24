using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.Applications
{
    public class BulkEntrantCitizenshipReader : BulkReaderBase<EntrantCitizenshipDto>
    {
        public BulkEntrantCitizenshipReader(PackageData packageData)
        {
            foreach (var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(EntrantCitizenshipDto.GetItems(app));
            }

            AddGetter("Id", dto => dto.ID);
            AddGetter("EntrantUID", dto => dto.EntrantUID);
            AddGetter("CountryID", dto => dto.CountryID);
            //AddGetter("InstitutionID", dto => packageData.InstitutionId);
            //AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
        }
    }

    public class EntrantCitizenshipDto : ImportBase
    {
        public EntrantCitizenshipDto() {}

        public EntrantCitizenshipDto(PackageDataApplication app, PackageDataApplicationEntrantCitizenship citizenship)
        {
            EntrantUID = app.EntrantUID;
            CountryID = citizenship.CountryID;
        }

        public static List<EntrantCitizenshipDto> GetItems(PackageDataApplication package)
        {
            List<EntrantCitizenshipDto> res = new List<EntrantCitizenshipDto>();
            if (package.EntrantCitizenships != null)
                foreach (var citizenship in package.EntrantCitizenships)
                {
                    res.Add(new EntrantCitizenshipDto(package, citizenship));
                }

            return res;
        }

        public string EntrantUID { get; set; }
        public int? CountryID { get; set; }
        //public int InstitutionID { get; set; }
        //public int ImportPackageId { get; set; }
    }
}
