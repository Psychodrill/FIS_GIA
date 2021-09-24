using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Model.Common
{
    public class SPResult
    {
        public bool returnValue { get; set; }
        public string errorMessage { get; set; }
        public string violationMessage { get; set; }
        public int violationId { get; set; }
    }
}
