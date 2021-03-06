namespace Ege.Check.Dal.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("ap_MarkBorders")]
    public class MarkBorder
    {
        /// <summary>
        /// </summary>
        public int SubjectCode { get; set; }

        [ForeignKey("SubjectCode")]
        public Subject Subject { get; set; }

        /// <summary>
        /// </summary>
        public int MaxValue { get; set; }
    }
}