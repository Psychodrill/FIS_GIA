using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OlympicTypeProfile
    {
        public OlympicTypeProfile()
        {
            OlympicDiplomant = new HashSet<OlympicDiplomant>();
            OlympicSubject = new HashSet<OlympicSubject>();
        }

        public int OlympicTypeProfileId { get; set; }
        public int OlympicTypeId { get; set; }
        public short? OlympicLevelId { get; set; }
        public int OlympicProfileId { get; set; }
        public int? OrganizerId { get; set; }
        public string OrganizerName { get; set; }
        public string OrganizerAddress { get; set; }
        public bool? OrganizerConnected { get; set; }
        public int? CoOrganizerId { get; set; }
        public bool? CoOrganizerConnected { get; set; }
        public int? OrgOlympicEnterId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Institution CoOrganizer { get; set; }
        public virtual OlympicLevel OlympicLevel { get; set; }
        public virtual OlympicProfile OlympicProfile { get; set; }
        public virtual OlympicType OlympicType { get; set; }
        public virtual Institution OrgOlympicEnter { get; set; }
        public virtual Institution Organizer { get; set; }
        public virtual ICollection<OlympicDiplomant> OlympicDiplomant { get; set; }
        public virtual ICollection<OlympicSubject> OlympicSubject { get; set; }
    }
}
