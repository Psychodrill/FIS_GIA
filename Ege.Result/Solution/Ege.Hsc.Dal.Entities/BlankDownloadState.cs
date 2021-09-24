namespace Ege.Hsc.Dal.Entities
{
    /// <summary>
    /// Состояние загрузки бланка
    /// </summary>
    public enum BlankDownloadState
    {
        /// <summary>
        /// В очереди
        /// </summary>
        Queued = 0,
        /// <summary>
        /// Загружается
        /// </summary>
        Downloading = 1,
        /// <summary>
        /// Загружен
        /// </summary>
        Downloaded = 2,
        /// <summary>
        /// Ошибка во время загрузки
        /// </summary>
        Error = 3,

    }
}