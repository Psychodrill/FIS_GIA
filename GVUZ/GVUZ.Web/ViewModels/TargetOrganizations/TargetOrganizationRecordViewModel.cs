using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.Web.ViewModels.TargetOrganizations
{
    public class TargetOrganizationRecordViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int InstitutionID { get; set; }
    }
}
