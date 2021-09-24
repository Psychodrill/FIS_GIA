namespace Ege.Check.Logic.Models.Staff
{
    using System.Collections.Generic;

    /// <summary>
    ///     Настройки экзаменов для региона
    /// </summary>
    public class ExamSettings
    {
        /// <summary>
        ///     Настройки
        /// </summary>
        public ICollection<ExamSetting> Settings { get; set; }
    }
}