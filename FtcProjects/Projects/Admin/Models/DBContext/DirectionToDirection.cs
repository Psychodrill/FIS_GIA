using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DirectionToDirection
    {
        public int? Id { get; set; }
        public string Guid1 { get; set; }
        public string Guid2 { get; set; }
    }
}
