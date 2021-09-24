namespace Ege.Hsc.Logic.Blanks
{
    using System.IO;
    using System.Threading.Tasks;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    public interface IBlankZipper
    {
        /// <summary>
        /// Создать zip-архив для выгрузки бланков
        /// </summary>
        [NotNull]
        Task Zip([NotNull]BlankRequest request);

        /// <summary>
        /// Получить имя файла с ФИО участника, бланки которого зазипованы
        /// </summary>
        /// <param name="zipStream">Бланки участника</param>
        /// <returns>Иванов_ИИ.zip</returns>
        [NotNull]
        string GetFioFilename([NotNull]Stream zipStream);
    }
}
