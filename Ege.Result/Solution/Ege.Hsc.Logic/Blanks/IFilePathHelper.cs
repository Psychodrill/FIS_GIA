namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    public interface IFilePathHelper
    {
        /// <summary>
        /// Получает путь к архиву, формирующемуся/сформированному по запросу
        /// </summary>
        /// <param name="requestId">Идентификатор запроса</param>
        [NotNull]
        string GetZipPath(Guid requestId);

        /// <summary>
        /// Получает место хранения бланков.
        /// </summary>
        /// <param name="hash">Хеш фио участника</param>
        /// <param name="documentNumber">Номер документа участника</param>
        /// <param name="rbdId">Глобальный идентификатор участника</param>
        /// <returns>Путь к папке с бланками</returns>
        [NotNull]
        string GetOrCreatePath([NotNull]string hash, [NotNull]string documentNumber, Guid rbdId);

        /// <summary>
        /// Получает место для загрузки файла. Создает папку при необходимости
        /// </summary>
        /// <param name="hash">Хеш фио участника</param>
        /// <param name="documentNumber">Номер документа участника</param>
        /// <param name="rbdId">Глобальный идентификатор участника</param>
        /// <param name="order">Номер бланка по порядку</param>
        /// <returns>Путь к файлу бланка</returns>
        [NotNull]
        string GetOrCreatePath([NotNull]string hash, [NotNull]string documentNumber, Guid rbdId, int order);

        /// <summary>
        /// Получает информацию о бланке по имени загруженного файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        DownloadedBlank TryParsePath([NotNull] string path);
    }
}
