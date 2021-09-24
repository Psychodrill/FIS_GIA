namespace Ege.Check.Logic.Models.Cache
{
    /// <summary>
    ///     Серверы хранения бланков
    ///     Хранится в кэше только в составе коллекции
    /// </summary>
    public class BlanksServerCacheModel
    {
        /// <summary>
        ///     Бланки обычных экзаменов
        /// </summary>
        public string Common { get; set; }

        /// <summary>
        ///     Бланки сочинения
        /// </summary>
        public string Composition { get; set; }
    }
}