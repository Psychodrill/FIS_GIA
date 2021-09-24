namespace Ege.Check.Logic.Models.Cache
{
    /// <summary>
    ///     Кэш-модель информации РЦОИ на странице участника
    ///     Хранится в кэше в составе RegionSettingsCacheModel
    /// </summary>
    public class RegionInfoCacheModel
    {
        /// <summary>
        ///     Номер телефона горяей линии РЦОИ
        /// </summary>
        public string HotlinePhone { get; set; }

        /// <summary>
        ///     Информация для участников ЕГЭ
        /// </summary>
        public string Info { get; set; }
    }
}