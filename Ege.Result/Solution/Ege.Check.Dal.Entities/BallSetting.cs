namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Ege.Check.Dal.Entities.Interfaces;

    /// <summary>
    /// </summary>
    [Table("ap_BallSettings")]
    public class BallSetting : IHasIntId
    {
        /// <summary>
        /// </summary>
        public int SubjectCode { get; set; }

        [ForeignKey("SubjectCode")]
        public Subject Subject { get; set; }

        /// <summary>
        /// </summary>
        public int TaskNumber { get; set; }

        /// <summary>
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// </summary>
        public int TaskTypeCode { get; set; }

        /// <summary>
        /// </summary>
        public string LegalSymbols { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }
    }
}