using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.OlympicFiles
{
    public class OlympicFilesListViewModel
    {
        private List<OlympicFileRecordViewModel> _records;

        public List<OlympicFileRecordViewModel> Records
        {
            get { return _records ?? (_records = new List<OlympicFileRecordViewModel>()); }
            set { _records = value; }
        }

        public bool IsEmpty
        {
            get { return _records == null || _records.Count == 0; }
        }
    }
}