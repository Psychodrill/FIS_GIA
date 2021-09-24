using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public class ApplicationViolationInfo
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public string ViolationMessage { get; set; }
    }
}