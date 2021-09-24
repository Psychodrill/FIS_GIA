using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class UserPolicy
    {
        public UserPolicy()
        {
            RatingList = new HashSet<RatingList>();
        }

        public Guid UserId { get; set; }
        public int? InstitutionId { get; set; }
        public bool IsMainAdmin { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public int AvailableEgeCheckCount { get; set; }
        public string PinCode { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsReadOnly { get; set; }
        public int Subrole { get; set; }
        public int? FilialId { get; set; }
        public bool? IsDeactivated { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual AspnetUsers User { get; set; }
        public virtual ICollection<RatingList> RatingList { get; set; }
    }
}
