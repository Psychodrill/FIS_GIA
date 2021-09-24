using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest
{

    public class EntranceTestResultSourceVoc : VocabularyBase<EntranceTestResultSourceVocDto>
    {
        public EntranceTestResultSourceVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EntranceTestResultSourceVocDto : VocabularyBaseDto
    {
        public int SourceID { get; set; }
        public string Description { get; set; }
    }
}
