using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.InstitutionProgram
{
    public class InstitutionProgramModel
    {
        public int InstitutionProgramID { get; set; }
        public int InstitutionID { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        //public bool? CanRemove { get; set; }
        //public bool? IsEdit { get; set; }
    }
}
