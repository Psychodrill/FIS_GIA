using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class UgsFromEiis
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Standart { get; set; }
        public bool? IsActual { get; set; }
        public string NotTrue { get; set; }
    }
}
