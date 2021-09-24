using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class BenefitItemSubject
    {
        public int Id { get; set; }
        public int BenefitItemId { get; set; }
        public int SubjectId { get; set; }
        public int EgeMinValue { get; set; }

        public virtual BenefitItemC BenefitItem { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
