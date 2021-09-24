using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class FindPathTableEge
    {
        public Guid? ParticipantId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
    }
}
