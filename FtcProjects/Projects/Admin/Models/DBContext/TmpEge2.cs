using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpEge2
    {
        public int ApplicationId { get; set; }
        public string Предмет { get; set; }
        public decimal? БаллПриЗачислении { get; set; }
        public int? БаллЕгэ { get; set; }
        public int InstitutionId { get; set; }
    }
}
