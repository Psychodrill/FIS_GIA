using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class ApplicationStatusTypeVoc : VocabularyBase<ApplicationStatusTypeVocDto>
    {
        public ApplicationStatusTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class ApplicationStatusTypeVocDto : VocabularyBaseDto
    {
        public int StatusID { get; set; }
        public bool UseForAppLimitValidation { get; set; }
        public bool IsActiveApp { get; set; }
    }
}
