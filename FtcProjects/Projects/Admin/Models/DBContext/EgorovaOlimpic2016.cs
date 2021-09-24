using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EgorovaOlimpic2016
    {
        public int CompetitiveGroupId { get; set; }
        public string Ugscode { get; set; }
        public int? Avgmark { get; set; }
        public int Cnt { get; set; }
    }
}
