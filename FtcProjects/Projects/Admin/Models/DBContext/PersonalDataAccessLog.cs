using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class PersonalDataAccessLog
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string ObjectType { get; set; }
        public string AccessMethod { get; set; }
        public int? InstitutionId { get; set; }
        public string UserLogin { get; set; }
        public int? ObjectId { get; set; }
        public DateTime AccessDate { get; set; }
    }
}
