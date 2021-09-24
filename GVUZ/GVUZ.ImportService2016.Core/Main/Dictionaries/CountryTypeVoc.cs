using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CountryTypeVoc : VocabularyBase<CountryTypeVocDto>
    {
        public CountryTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CountryTypeVocDto : VocabularyBaseDto
    {
        public int CountryID { get; set; }
        public override int ID
        {
            get { return CountryID; }
            set { CountryID = value; }
        }
    }
}
