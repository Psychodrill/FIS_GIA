using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CompositionThemesApprob
    {
        public CompositionThemesApprob()
        {
            ApplicationCompositionResultsApprob = new HashSet<ApplicationCompositionResultsApprob>();
        }

        public int ThemeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationCompositionResultsApprob> ApplicationCompositionResultsApprob { get; set; }
    }
}
