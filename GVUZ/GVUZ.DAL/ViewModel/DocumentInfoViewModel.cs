using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.ViewModel
{
    public class DocumentInfoViewModel
    {
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string Info
        {
            get
            {
                var sn = DocumentNumber ?? "";
                var sd = DocumentDate == null ? "" : DocumentDate.Value.ToShortDateString();

                return string.Format("№ {0} от {1}", sn, sd);
            }
        }
    }
}
