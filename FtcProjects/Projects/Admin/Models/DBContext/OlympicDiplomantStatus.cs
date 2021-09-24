using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicDiplomantStatus
    {
        public OlympicDiplomantStatus()
        {
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
        }

        public int OlympicDiplomantStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
    }
}
