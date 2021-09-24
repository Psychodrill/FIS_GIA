using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class GlobalMinEgeVoc : VocabularyBase<GlobalMinEgeVocDto>
    {
        public GlobalMinEgeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class GlobalMinEgeVocDto : VocabularyBaseDto
    {
        public int EgeYear { get; set; }
        public int MinEgeScore { get; set; }
        
    }
}
