using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GenderType
    {
        public GenderType()
        {
            Entrant1 = new HashSet<Entrant1>();
        }

        public int GenderId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Entrant1> Entrant1 { get; set; }
    }
}
