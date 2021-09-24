using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkApplicationSelectedCompetitiveGroupItemReader : BulkReaderBase<BulkApplicationSelectedCompetitiveGroupItemDto>
    {
        public BulkApplicationSelectedCompetitiveGroupItemReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            foreach(var app in packageData.ApplicationsToImport()){
                _records.AddRange(BulkApplicationSelectedCompetitiveGroupItemDto.GetItems(app, vocabularyStorage));
            }
            //_records = packageData.Applications.Select(t=> new  {t.UID, t.SelectedCompetitiveGroups}).Select(p=> new BulkApplicationSelectedCompetitiveGroupDto(p. ){} ).ToList();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("CompetitiveGroupItemID", dto => dto.CompetitiveGroupItemID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);
        }
    }

    public class BulkApplicationSelectedCompetitiveGroupItemDto : ImportBase
    {
        public BulkApplicationSelectedCompetitiveGroupItemDto() { }

        public BulkApplicationSelectedCompetitiveGroupItemDto(PackageDataApplication application, CompetitiveGroupItemDict competitiveGroupItem, VocabularyStorage vocStorage) 
        {
            ApplicationUID = application.UID;
            ApplicationGuid = application.GUID;
            CompetitiveGroupItemID = competitiveGroupItem.ID;
            UID = competitiveGroupItem.UID;
        }

        public static List<BulkApplicationSelectedCompetitiveGroupItemDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage)
        {
            List<BulkApplicationSelectedCompetitiveGroupItemDto> res = new List<BulkApplicationSelectedCompetitiveGroupItemDto>();
            foreach (var cgi in application.SelectedCompetitiveGroupItemsFull)
            {
                res.Add(new BulkApplicationSelectedCompetitiveGroupItemDto(application, cgi, vocStorage));
            }

            return res;
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public int CompetitiveGroupItemID { get; set; }
        public string UID { get; set; }
    }
}
