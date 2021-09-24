namespace Ege.Hsc.Logic.Models.Servers
{
    using System;

    public class BlankServerAvailabilityModel
    {
        public int RegionId { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public DateTime ExamDate { get; set; }
        public int SubjectCode { get; set; }
    }
}
