using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TownType
    {
        public TownType()
        {
            Entrant1 = new HashSet<Entrant1>();
        }

        public int TownTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Entrant1> Entrant1 { get; set; }
    }
}
