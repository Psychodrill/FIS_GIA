namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    ///     Настройки экзаменов для региона
    ///     Ключ в кэше - RegionId
    /// </summary>
    public class RegionSettingsCacheModel
    {
        /// <summary>
        ///     Серверы хранения бланков
        /// </summary>
        [NotNull]
        public BlanksServerCacheModel Servers { get; set; }

        /// <summary>
        ///     Информация для участников
        /// </summary>
        [NotNull]
        public RegionInfoCacheModel Info { get; set; }

        /// <summary>
        ///     ExamId -> настройка экзамена
        /// </summary>
        [NotNull]
        public IDictionary<int, RegionExamSettingCacheModel> Settings { get; set; }

        public override string ToString()
        {
            return string.Format("Servers: {0}, Info: {1}, Settings: {2}", Servers, Info, string.Join(",",Settings));
        }
    }
}