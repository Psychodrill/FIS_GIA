using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicLevel
    {
        public OlympicLevel()
        {
            OlympicTypeProfile = new HashSet<OlympicTypeProfile>();
        }

        public short OlympicLevelId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfile { get; set; }
    }
}
