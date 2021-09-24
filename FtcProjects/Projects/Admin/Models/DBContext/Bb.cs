using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Bb
    {
        public Guid? ParticipantId { get; set; }
        public string ТипПоступающего { get; set; }
        public string НаименованиеОо { get; set; }
        public int? РегионОо { get; set; }
        public string УровеньОбразования { get; set; }
        public string НаправлениеПодготовки { get; set; }
        public string ИсточникФинансирования { get; set; }
        public string ФормаОбучения { get; set; }
        public string Статус { get; set; }
        public string НаличиеИндивидуальныхДостижений { get; set; }
        public string ТипМедали { get; set; }
        public string СведенияОбОлимпиадеАх { get; set; }
    }
}
