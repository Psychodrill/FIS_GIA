using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class AllowedDirectionsVoc : VocabularyBase<AllowedDirectionsVocDto>
    {
        public AllowedDirectionsVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class AllowedDirectionsVocDto : VocabularyBaseDto
    {
        public int DirectionID { get; set; } 
        public int AdmissionItemTypeID { get; set; }
        public int ParentDirectionID { get; set; }
    }
}
