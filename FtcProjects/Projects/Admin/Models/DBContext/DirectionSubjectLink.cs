using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DirectionSubjectLink
    {
        public DirectionSubjectLink()
        {
            DirectionSubjectLinkDirection = new HashSet<DirectionSubjectLinkDirection>();
            DirectionSubjectLinkSubject = new HashSet<DirectionSubjectLinkSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProfileSubjectId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Subject ProfileSubject { get; set; }
        public virtual ICollection<DirectionSubjectLinkDirection> DirectionSubjectLinkDirection { get; set; }
        public virtual ICollection<DirectionSubjectLinkSubject> DirectionSubjectLinkSubject { get; set; }
    }
}
