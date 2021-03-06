namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_RegionInfo")]
    public class RegionInfo : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int? rbd_REGION { get; set; }

        [ForeignKey("rbd_REGION")]
        public Region RbdRegion { get; set; }

        /// <summary>
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        public string HotLineData { get; set; }

        /// <summary>
        /// </summary>
        public string WebServerBlanks { get; set; }

        /// <summary>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        public int Region { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}