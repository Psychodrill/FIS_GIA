using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkDeleteReader : BulkReaderBase<VocabularyBaseDto>
    {
        public BulkDeleteReader(int importPackageID) //List<VocabularyBaseDto> dtos)
        {
            _records.ForEach(t=> t.IsDeleted = true);

            AddGetter("ID", dto => dto.ID);
            AddGetter("Type", dto => dto.GetType().Name);
            AddGetter("ImportPackageID", dto => importPackageID);
        }

        public override void Add(VocabularyBaseDto addinionalRecord)
        {
            base.Add(addinionalRecord);
            addinionalRecord.IsDeleted = true;
        }

        public override void AddRange(List<VocabularyBaseDto> addinionalRecords)
        {
            base.AddRange(addinionalRecords);
            addinionalRecords.ForEach(t => t.IsDeleted = true);
        }
    }
}
