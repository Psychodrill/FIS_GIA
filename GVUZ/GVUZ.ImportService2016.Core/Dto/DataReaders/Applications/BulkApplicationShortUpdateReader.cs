using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Extensions;
using System;
using System.Collections.Generic;
using GVUZ.Model.Institutions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkApplicationShortUpdateReader : BulkReaderBase<PackageDataApplicationShortUpdateDto>
    {
        public BulkApplicationShortUpdateReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach (var app in packageData.ApplicationsToShortImport())
            {
                _records.AddRange(PackageDataApplicationShortUpdateDto.GetItems(app, vocabularyStorage));
            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("ApplicationNumber", dto => GetStringOrNull(dto.ApplicationNumber));
            AddGetter("RegistrationDate", dto => GetDateOrNull(dto.RegistrationDate));
            AddGetter("CustomInformation", dto => GetStringOrNull(dto.CustomInformation));

            AddGetter("CompetitiveGroupID", dto => GetIntOrNull(dto.CompetitiveGroupID));
            AddGetter("IsAgreedDate", dto => GetDateOrNull(dto.IsAgreedDate));

            AddGetter("EntrantDocumentID", dto => GetIntOrNull(dto.EntrantDocumentID));
            AddGetter("EntrantDocumentUID", dto => GetStringOrNull(dto.EntrantDocumentUID));
            AddGetter("OriginalReceivedDate", dto => GetDateOrNull(dto.OriginalReceivedDate));

            AddGetter("StatusID", dto => dto.StatusID);

            AddGetter("IsDisagreedDate", dto => GetDateOrNull(dto.IsDisagreedDate));
        }
    }

    public class PackageDataApplicationShortUpdateDto : ImportBase
    {
        public PackageDataApplicationShortUpdateDto() { }


        public static List<PackageDataApplicationShortUpdateDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            var res = new List<PackageDataApplicationShortUpdateDto>();
            res.Add(new PackageDataApplicationShortUpdateDto
            {
                ID = application.ID,
                ApplicationNumber = application.ApplicationNumber,
                RegistrationDate = application.RegistrationDate,
                CustomInformation = application.Entrant.CustomInformation,
                StatusID = application.StatusID.To(0)
                // остальное null
            });

            if (application.FinSourceAndEduForms != null)
            {
                foreach (var fs in application.FinSourceAndEduForms)
                {
                    res.Add(new PackageDataApplicationShortUpdateDto()
                    {
                        ID = application.ID,
                        CompetitiveGroupID = fs.CompetitiveGroupID,
                        IsAgreedDate = fs.IsAgreedDateSpecified ? (DateTime?)fs.IsAgreedDate : null,
                        IsDisagreedDate = fs.IsDisagreedDateSpecified ? (DateTime?)fs.IsDisagreedDate : null
                    });
                }
            }

            if (application.ApplicationDocuments != null)
            {
                foreach(var doc in application.ApplicationDocuments.AllDocuments())
                {
                    res.Add(new PackageDataApplicationShortUpdateDto()
                    {
                        ID = application.ID,
                        EntrantDocumentID = doc.ID,
                        EntrantDocumentUID = doc.UID,
                        OriginalReceivedDate = doc.OriginalReceivedDateSpecified ? (DateTime?)doc.OriginalReceivedDate : null
                    });
                }
            }

            return res;
        }

        //public int ID { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int StatusID { get; set; }

        //public int EntrantID { get; set; }
        public string CustomInformation { get; set; }

        public int? EntrantDocumentID { get; set; }
        public string EntrantDocumentUID { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }

        public int? CompetitiveGroupID { get; set; }
        public DateTime? IsAgreedDate { get; set; }

        public DateTime? IsDisagreedDate { get; set; }

    }
}