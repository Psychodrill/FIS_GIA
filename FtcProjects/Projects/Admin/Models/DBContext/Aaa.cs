using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Aaa
    {
        public Guid ParticipantId { get; set; }
        public string НаименованиеОо { get; set; }
        public string УровеньОбразования { get; set; }
        public string НаправлениеПодготовки { get; set; }
        public string ИсточникФинансирования { get; set; }
        public string ФормаОбучения { get; set; }
        public int CompetitiveGroupId { get; set; }
        public double? EgeBall { get; set; }
        public int? Apps { get; set; }
        public int? Places { get; set; }
        public double? Konkurs { get; set; }
        public decimal? ProhodnoyBall { get; set; }
    }
}
