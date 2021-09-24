namespace Ege.Check.App.Web.Blanks.ViewModels.Hsc
{
    using System;

    public class IndexPageViewModel
    {
        public IndexPageViewModel(DateTime openDate, bool csvUploadAllowed)
        {
            OpenDate = openDate;
            CsvUploadAllowed = csvUploadAllowed;
        }

        public DateTime OpenDate { get; private set; }
        public bool CsvUploadAllowed { get; private set; }
    }
}