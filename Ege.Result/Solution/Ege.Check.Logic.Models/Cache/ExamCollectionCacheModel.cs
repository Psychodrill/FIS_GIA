namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;

    /// <summary>
    ///     Кэш-модель экзаменов участника
    ///     ключ в кэше - ParticipantRbdId
    /// </summary>
    public class ExamCollectionCacheModel
    {
        /// <summary>
        ///     Данные об экзаменах
        /// </summary>
        public ICollection<ExamCacheModel> Exams { get; set; }

        public override string ToString()
        {
            return string.Format("Exams: {0}", Exams == null ? "null" : Exams.Count.ToString());
        }
    }
}