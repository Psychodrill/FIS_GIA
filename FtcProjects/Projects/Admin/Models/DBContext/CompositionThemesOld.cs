using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompositionThemesOld
    {
        public CompositionThemesOld()
        {
            ApplicationCompositionResults = new HashSet<ApplicationCompositionResults>();
        }

        public int ThemeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationCompositionResults> ApplicationCompositionResults { get; set; }
    }
}
