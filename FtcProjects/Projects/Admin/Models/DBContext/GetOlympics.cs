using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class GetOlympics
    {
        public int OlympicTypeProfileId { get; set; }
        public int OlympicTypeId { get; set; }
        public string OlympicTypeName { get; set; }
        public int? OlympicNumber { get; set; }
        public int OlympicYear { get; set; }
        public short? OlympicLevelId { get; set; }
        public string OlympicLevelName { get; set; }
        public int OlympicProfileId { get; set; }
        public string ProfileName { get; set; }
        public int? InstitutionId { get; set; }
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
    }
}
