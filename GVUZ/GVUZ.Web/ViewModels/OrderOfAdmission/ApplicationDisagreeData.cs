using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationDisagreeData
    {
        public int ApplicationItemId { get; set; }

        public bool IsDisagreed { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
    }
}