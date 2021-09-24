using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ApplicationCompositionResults
    {
        public int CompositionId { get; set; }
        public int ApplicationId { get; set; }
        public string SourceId { get; set; }
        public DateTime Year { get; set; }
        public int? ThemeId { get; set; }
        public bool Result { get; set; }
        public string CompositionPaths { get; set; }
        public DateTime? ExamDate { get; set; }
        public DateTime? DownloadDate { get; set; }
        public bool? HasAppeal { get; set; }
        public string AppealStatus { get; set; }

        public virtual Application Application { get; set; }
        public virtual CompositionThemesOld Theme { get; set; }
    }
}
