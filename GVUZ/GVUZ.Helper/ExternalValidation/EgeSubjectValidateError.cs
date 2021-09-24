using System.Collections.Generic;

namespace GVUZ.Helper.ExternalValidation
{
    public class EgeSubjectValidateError
    {
        public string SubjectName { get; set; }
        public decimal ResultValue { get; set; }
        public string Error { get; set; }
    }

    public class EgeSubjectValidateErrorList
    {
        public EgeSubjectValidateErrorList(int entrantDocumentId,bool isLoaded)
        {
            Errors = new List<EgeSubjectValidateError>();
            ErrorsFiltered = new List<EgeSubjectValidateError>();

            EntrantDocumentID = entrantDocumentId;
            IsLoaded = isLoaded;
        }

        public bool IsLoaded { get; private set; }
        public List<EgeSubjectValidateError> Errors { get; set; }
        public List<EgeSubjectValidateError> ErrorsFiltered { get; set; }
        public int EntrantDocumentID { get; private set; }
        public string MainError { get; set; }

        public bool HasErrors
        {
            get
            {
                return !string.IsNullOrEmpty(MainError) ||
                       Errors.Count > 0 ||
                       ErrorsFiltered.Count > 0;
            }
        }
    }
}