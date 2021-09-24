using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest
{
    public class EntranceTestCreativeDirectionVoc : VocabularyBase<EntranceTestCreativeDirectionDto>
    {
        public EntranceTestCreativeDirectionVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EntranceTestCreativeDirectionDto : VocabularyBaseDto
    {
        public int DirectionID { get; set; }
        public int ParentID { get; set; }
        public override int ID
        {
            get { return DirectionID; }
            set { DirectionID = value; }
        }
    }
}
