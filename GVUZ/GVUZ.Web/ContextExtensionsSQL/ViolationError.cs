using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Web.SQLDB
{
    public class ViolationErrorList
    {
        private List<ViolationError> _errors;

        public bool NeedChekEge;

        public int ErrorCode
        {
            get
            {
                return Errors.Any(e => e.ViolationId > 0) ? Errors.First(e => e.ViolationId > 0).ViolationId : 0;
            }
        }

        public List<ViolationError> Errors
        {
            get { return _errors ?? (_errors = new List<ViolationError>()); }
        }

        public string ViolationMessages
        {
            get
            {
                if (Errors.Any(e => e.ViolationId > 0))
                {
                    return string.Join(";", Errors.Where(e => e.ViolationId > 0).Select(e => e.ViolationMessage));    
                }

                return null;
            }
        }

        public void Add(int violationId, string violationMessage)
        {
            Errors.Add(new ViolationError(violationId, violationMessage));
        }
    }

    public class ViolationError
    {
        public ViolationError()
        {   
        }

        public ViolationError(int violationId, string violationMessage)
        {
            ViolationId = violationId;
            ViolationMessage = violationMessage;
        }

        public int ViolationId { get; set; }
        public string ViolationMessage { get; set; }
    }
}