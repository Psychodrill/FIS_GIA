using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class DirectionsFromEiis
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Edulevelfk { get; set; }
        public string Eduprogrammfk { get; set; }
        public string Standart { get; set; }
        public string Ugscode { get; set; }
        public string Ugsname { get; set; }
        public string UgsFk { get; set; }
        public string Reiod { get; set; }
        public string NotTrue { get; set; }
        public string IsActual { get; set; }
    }
}
