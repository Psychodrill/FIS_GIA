namespace Ege.Check.Logic.Models.Staff
{
    /// <summary>
    ///     Экзамен
    /// </summary>
    public class ExamListElement
    {
        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Название экзамена
        /// </summary>
        public string SubjectName { get; set; }
    }
}