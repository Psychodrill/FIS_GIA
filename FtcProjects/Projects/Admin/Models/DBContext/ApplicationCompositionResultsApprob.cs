using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationCompositionResultsApprob
    {
        public int CompositionId { get; set; }
        public int ApplicationId { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Year { get; set; }
        public int ThemeId { get; set; }
        public bool Result { get; set; }
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }

        public virtual Application Application { get; set; }
        public virtual CompositionThemesApprob Theme { get; set; }
    }
}
