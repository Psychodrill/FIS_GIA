using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.Partial
{
    public partial class ImportPackage //: GVUZ.ServiceModel.Import.ImportPackage
    {
        public VocabularyStorage VocabularyStorage { get; set; }


        public ImportPackage() { }

        public string CheckResultInfo { get; set; }
        public int? CheckStatusID { get; set; }
        public string Comment { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string ImportedAppIDs { get; set; }
        public DateTime LastDateChanged { get; set; }
        public string PackageData { get; set; }
        public int PackageID { get; set; }
        public string ProcessResultInfo { get; set; }
        public int StatusID { get; set; }
        public int TypeID { get; set; }
        public string UserLogin { get; set; }
        public int InstitutionID { get; set; }
    }
}
