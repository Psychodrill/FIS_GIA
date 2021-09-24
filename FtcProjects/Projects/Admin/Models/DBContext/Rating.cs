using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Rating
    {
        public Rating()
        {
            RatingList = new HashSet<RatingList>();
        }

        public int Id { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int Stage { get; set; }
        public bool IsFinish { get; set; }

        public virtual CompetitiveGroup CompetitiveGroup { get; set; }
        public virtual ICollection<RatingList> RatingList { get; set; }
    }
}
