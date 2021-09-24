using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TempPopko
    {
        public decimal Id { get; set; }
        public string RegionId { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
    }
}
