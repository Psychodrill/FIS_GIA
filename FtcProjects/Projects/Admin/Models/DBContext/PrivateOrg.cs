using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PrivateOrg
    {
        public int? Id { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
        public string FisName { get; set; }
        public string Profile { get; set; }
    }
}
