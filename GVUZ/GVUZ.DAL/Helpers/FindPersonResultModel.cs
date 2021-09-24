using GVUZ.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.Data.Helpers
{
    public class FindPersonResultModel
    {
        public int Status { get; set; }
        public string Text { get; set; }

        public IEnumerable<FindPerson> Persons { get; set; }
    }
}
