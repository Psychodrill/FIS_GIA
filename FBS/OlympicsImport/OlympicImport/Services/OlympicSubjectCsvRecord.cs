using System.Collections.Generic;

namespace OlympicImport.Services
{
    public class OlympicSubjectCsvRecord
    {
        private ICollection<long> _fbsSubjectId;

        public ICollection<long> FbsSubjectId
        {
            get { return _fbsSubjectId ?? (_fbsSubjectId = new List<long>()); }
            set { _fbsSubjectId = value; }
        }
        public long Id { get; set; }
        public string SubjectName { get; set; }
    }
}