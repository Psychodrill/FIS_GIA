using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Rvipersons
    {
        public Rvipersons()
        {
            Entrant1 = new HashSet<Entrant1>();
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
            RvipersonIdentDocs = new HashSet<RvipersonIdentDocs>();
        }

        public int PersonId { get; set; }
        public bool IsRecordDeleted { get; set; }
        public string NormSurname { get; set; }
        public string NormName { get; set; }
        public string NormSecondName { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool? Sex { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Snils { get; set; }
        public string Inn { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime IntegralUpdateDate { get; set; }
        public Guid? ParticipantId { get; set; }
        public int? UseYear { get; set; }

        public virtual ICollection<Entrant1> Entrant1 { get; set; }
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
        public virtual ICollection<RvipersonIdentDocs> RvipersonIdentDocs { get; set; }
    }
}
