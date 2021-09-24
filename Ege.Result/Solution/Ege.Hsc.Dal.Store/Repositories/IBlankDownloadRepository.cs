namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    public interface IBlankDownloadRepository
    {
        /// <summary>
        /// Изменяет текущее состояние загрузки бланка
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id">Идентификатор загрузки бланка</param>
        /// <param name="state">Состояние, которое требуется выставить</param>
        /// <returns></returns>
        [NotNull]
        Task ChangeStateAsync([NotNull]DbConnection connection, int id, BlankDownloadState state);

        /// <summary>
        ///     Получить maxCount ещё не загруженных бланков и выставить им статус "Загружается"
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="maxCount">Сколько взять бланков</param>
        /// <returns></returns>
        [NotNull]
        Task<IList<BlankToDownload>> TopNotDownloadFromServerAsync([NotNull]DbConnection connection, int maxCount);

        /// <summary>
        /// Выгрузить информацию о бланках из основной БД
        /// </summary>
        /// <returns>Идентификатор участника -> бланк</returns>
        [NotNull]
        Task<LoadedBlanks> LoadFromCheckEgeDb([NotNull]DbConnection connection, [NotNull]DbTransaction transaction);

        /// <summary>
        /// Добавить бланки
        /// </summary>
        [NotNull]
        Task AddAsync([NotNull] SqlConnection connection, [NotNull] SqlTransaction transaction, [NotNull]IEnumerable<BlankDownload> blanks);

        /// <summary>
        /// Получить из основной БД информацию о бланках участников, для которых обнаружены несоответствия в числе страниц между основной БД и очередью выгрузки
        /// </summary>
        [NotNull]
        Task<LoadedBlanks> GetInconsistenciesWithCheckEgeDb([NotNull] DbConnection connection);

        /// <summary>
        /// Исправить несоответствия в числе страниц между основной БД и очередью выгрузки
        /// </summary>
        [NotNull]
        Task<int> FixInconsistenciesWithCheckEgeDb([NotNull] DbConnection connection, [NotNull]IEnumerable<BlankDownload> blank, [NotNull]DbTransaction transaction = null);

        /// <summary>
        /// Выставить ошибочный статус для криво загруженных файлов
        /// </summary>
        [NotNull]
        Task<int> SetErrorStatus([NotNull] DbConnection connection,
            [NotNull] IEnumerable<DownloadedBlank> invalidDownloads);
    }
}
