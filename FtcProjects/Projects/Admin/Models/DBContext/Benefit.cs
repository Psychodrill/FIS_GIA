using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Benefit
    {
        public Benefit()
        {
            Application = new HashSet<Application>();
            ApplicationCompetitiveGroupItem = new HashSet<ApplicationCompetitiveGroupItem>();
            ApplicationEntranceTestDocument = new HashSet<ApplicationEntranceTestDocument>();
            BenefitItemC = new HashSet<BenefitItemC>();
        }

        public short BenefitId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
        public virtual ICollection<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual ICollection<BenefitItemC> BenefitItemC { get; set; }
    }
}
