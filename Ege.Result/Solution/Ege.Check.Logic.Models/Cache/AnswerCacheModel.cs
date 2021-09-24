namespace Ege.Check.Logic.Models.Cache
{
    using Ege.Check.Logic.Services.Dtos.Enums;

    /// <summary>
    ///     Кэш-модель одного ответа
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class AnswerCacheModel
    {
        /// <summary>
        ///     Из какой части задание: А, Б, Ц, Д
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        ///     Номер задания
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     Ответ
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        ///     Полученный балл
        /// </summary>
        public int Mark { get; set; }
    }
}