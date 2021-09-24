namespace Ege.Hsc.Logic.Blanks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    public interface IBlankService
    {
        /// <summary>
        /// Получить бланки для загрузки
        /// </summary>
        /// <returns>Коллекция бланков</returns>
        [NotNull]
        Task<ICollection<Blank>> BlanksToDownload();

        /// <summary>
        /// Поставить в очередь загрузки бланки из основной БД
        /// </summary>
        [NotNull]
        Task LoadBlanksFromCheckEgeDb();

        /// <summary>
        /// Исправить несоответствия между количеством страниц в основной БД и очередью выгрузки бланков
        /// </summary>
        /// <returns></returns>
        [NotNull]
        Task FixInconsistenciesWithCheckEgeDb();
    }
}
