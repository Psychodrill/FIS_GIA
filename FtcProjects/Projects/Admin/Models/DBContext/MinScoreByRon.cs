using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MinScoreByRon
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int Year { get; set; }
        public int MinScore { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
