using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntHistAft
    {
        public int EntrantId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int? PersonId { get; set; }
        public Guid? ParticipantId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string DocumentNumber { get; set; }
    }
}
