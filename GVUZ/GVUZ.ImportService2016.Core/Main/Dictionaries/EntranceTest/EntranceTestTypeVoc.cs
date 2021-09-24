using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest
{

    public class EntranceTestTypeVoc : VocabularyBase<EntranceTestTypeVocDto>
    {
        public EntranceTestTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EntranceTestTypeVocDto : VocabularyBaseDto
    {
        public int EntranceTestTypeID { get; set; }
        public override int ID
        {
            get { return EntranceTestTypeID; }
            set { EntranceTestTypeID = value; }
        }
    }
}
