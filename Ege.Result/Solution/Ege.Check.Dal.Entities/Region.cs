namespace Ege.Check.Dal.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("rbdc_Regions")]
    public class Region
    {
        public Region()
        {
            Answers = new HashSet<Answer>();
            AppealsSettings = new HashSet<AppealsSetting>();
            FctUsers = new HashSet<FctUser>();
            GekDocuments = new HashSet<GekDocument>();
            Operators = new HashSet<Operator>();
            Participants = new HashSet<Participant>();
            RegionInfo = new HashSet<RegionInfo>();
        }

        /// <summary>
        /// </summary>
        public int REGION { get; set; }

        /// <summary>
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// </summary>
        public bool Enable { get; set; }

        [InverseProperty("Region")]
        public ICollection<Answer> Answers { get; set; }

        [InverseProperty("Region")]
        public ICollection<AppealsSetting> AppealsSettings { get; set; }

        [InverseProperty("Region")]
        public ICollection<FctUser> FctUsers { get; set; }

        [InverseProperty("Region")]
        public ICollection<GekDocument> GekDocuments { get; set; }

        [InverseProperty("Region")]
        public ICollection<Operator> Operators { get; set; }

        [InverseProperty("Region")]
        public ICollection<Participant> Participants { get; set; }

        [InverseProperty("rbd_REGI")]
        public ICollection<RegionInfo> RegionInfo { get; set; }
    }
}