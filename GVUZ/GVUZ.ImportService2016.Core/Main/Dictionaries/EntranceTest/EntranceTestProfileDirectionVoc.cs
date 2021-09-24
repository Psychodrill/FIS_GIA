using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest
{

    public class EntranceTestProfileDirectionVoc : VocabularyBase<EntranceTestProfileDirectionVocDto>
    {
        public EntranceTestProfileDirectionVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EntranceTestProfileDirectionVocDto : VocabularyBaseDto
    {
        public int DirectionID { get; set; }
        public override int ID
        {
            get { return DirectionID; }
            set { DirectionID = value; }
        }
    }
}
