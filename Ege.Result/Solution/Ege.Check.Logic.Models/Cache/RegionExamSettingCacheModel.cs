namespace Ege.Check.Logic.Models.Cache
{
    /// <summary>
    ///     Настройки экзамена для региона
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class RegionExamSettingCacheModel
    {
        /// <summary>
        ///     Показывать ли результат
        /// </summary>
        public bool ShowResult { get; set; }

        /// <summary>
        ///     Показывать ли бланки
        /// </summary>
        public bool ShowBlanks { get; set; }

        /// <summary>
        ///     Информация о документе ГЭК
        /// </summary>
        public GekDocumentCacheModel GekDocument { get; set; }
    }
}