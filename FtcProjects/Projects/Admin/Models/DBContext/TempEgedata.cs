using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TempEgedata
    {
        public Guid? ИдентефикаторЗапроса { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public DateTime? ДатаРождения { get; set; }
        public string Пол { get; set; }
        public string ТипДокумента { get; set; }
        public string СерияДокументаЛат { get; set; }
        public string СерияДокументаРус { get; set; }
        public string НомерДокумента { get; set; }
    }
}
