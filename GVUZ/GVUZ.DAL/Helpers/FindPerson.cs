using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.Data.Helpers
{
    public class FindPerson
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDay { get; set; }
        public Guid? ParticipantID { get; set; }
        public int? UseYear { get; set; }
        public int PersonIdentDocID { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTypeName { get; set; }
    }
}
