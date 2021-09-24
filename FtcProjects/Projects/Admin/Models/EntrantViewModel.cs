using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class EntrantViewModel
    {
        public Entrant Entrant { get; set; }
        public int InstitutionId { get; set; } 
        public EntrantApplication Application { get; set; }
    }

    public class Entrant
    {
        public int EntrantId { get; set; }
        public int IdentityDocumentId { get; set; }
        public string LastName { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class EntrantApplication
    {
        public int ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}
