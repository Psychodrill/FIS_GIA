using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class CompetitiveGroupTargetItemViewModel
    {
        public int CompetitiveGroupTargetItemID { get; set; }
        public int CompetitiveGroupTargetID { get; set; }
        public string ContractOrganizationName { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }


        public string  DisplayName
        {
            get { return (Name ?? ContractOrganizationName) + (!string.IsNullOrWhiteSpace(UID) ? string.Format(" (UID: {0})", UID) : string.Empty);  }
            set { }
        }

        public int NumberTargetO { get; set; }
	    public int NumberTargetOZ { get; set; }
	    public int NumberTargetZ { get; set; }

        public int Value { get; set; } 
    }
}
