using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompositionThemes
    {
        public int ID { get; set; }
        public int ThemeID { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
