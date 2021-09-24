using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class DisabilityTypeVoc : VocabularyBase<DisabilityTypeVocDto>
    {
        public DisabilityTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DisabilityTypeVocDto : VocabularyBaseDto
    {
        public int DisabilityID { get; set; }

        public override int ID
        {
            get { return DisabilityID; }
            set { DisabilityID = value; }
        }
    }
}
