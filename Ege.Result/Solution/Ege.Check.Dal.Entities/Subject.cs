namespace Ege.Check.Dal.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// </summary>
    [Table("dat_Subjects")]
    public class Subject
    {
        public Subject()
        {
            BallSettings = new HashSet<BallSetting>();
            MarkBorders = new HashSet<MarkBorder>();
            TaskSettings = new HashSet<TaskSetting>();
            Exams = new HashSet<Exam>();
        }

        /// <summary>
        /// </summary>
        public int SubjectCode { get; set; }

        /// <summary>
        /// </summary>
        public string SubjectName { get; set; }

        [InverseProperty("SubjectCo")]
        public ICollection<BallSetting> BallSettings { get; set; }

        [InverseProperty("SubjectCo")]
        public ICollection<MarkBorder> MarkBorders { get; set; }

        [InverseProperty("SubjectCo")]
        public ICollection<TaskSetting> TaskSettings { get; set; }

        [InverseProperty("SubjectCo")]
        public ICollection<Exam> Exams { get; set; }
    }
}