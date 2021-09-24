using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Query
    {
        public string СерияДокумента { get; set; }
        public string НомерДокумента { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string ДатаРождения { get; set; }
        public string Пол { get; set; }
    }
}
