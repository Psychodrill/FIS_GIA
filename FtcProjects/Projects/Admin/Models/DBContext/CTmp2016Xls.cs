using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class CTmp2016Xls
    {
        public Guid ИдУчастникаЕгэ { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string СтатусЗаявления { get; set; }
        public string НаименованиеОо { get; set; }
        public string КодСпециальности { get; set; }
        public string НаименованиеСпециальности { get; set; }
        public string УровеньОбразования { get; set; }
        public string ИсточникФинансирования { get; set; }
        public string ФормаОбучения { get; set; }
        public string НомерПриказа { get; set; }
        public string НаименованиеПриказа { get; set; }
    }
}
