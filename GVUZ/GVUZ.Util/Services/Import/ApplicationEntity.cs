using System;

namespace GVUZ.Util.Services.Import
{
    public class ApplicationEntity
    {
        public int ApplicationId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int InstitutionId { get; set; }
        public int StatusId { get; set; }
    }
}