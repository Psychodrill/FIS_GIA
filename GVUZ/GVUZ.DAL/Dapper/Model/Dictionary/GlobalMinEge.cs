using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Model.Dictionary
{
    [Table("GlobalMinEge")]
    public class GlobalMinEge 
    {
        public int EgeYear { get; set; }
        public int MinEgeScore { get; set; }
    }
}
