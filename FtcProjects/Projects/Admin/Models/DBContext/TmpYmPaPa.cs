using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class TmpYmPaPa
    {
        public string СерияДокумента { get; set; }
        public string НомерДокумента { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string ДатаРождения { get; set; }
        public string Пол { get; set; }
        public string ВузВКоторомПродолжилОбучение { get; set; }
        public short? УровеньОбразования { get; set; }
        public string КодСпециальностиНаКоторойПродолжилОбучение { get; set; }
        public string СпециальностьНаКоторойПродолжилОбучение { get; set; }
        public short? ФормаОбучения { get; set; }
        public short? ИсточникФинансирования { get; set; }
    }
}
