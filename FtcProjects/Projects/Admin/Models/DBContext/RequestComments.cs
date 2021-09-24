using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RequestComments
    {
        public int CommentId { get; set; }
        public string Commentor { get; set; }
        public string Comment { get; set; }
        public DateTime? Date { get; set; }
        public int DirectionId { get; set; }
        public int InstitutionId { get; set; }

        public virtual RequestDirection RequestDirection { get; set; }
    }
}
