namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Repositories;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    public interface IStateRepository
    {
        /// <summary>
        /// Во всех очередях заменить состояние "Обрабатывается" на "Не обработано" - следует выполнять при перезапуске сервиса
        /// </summary>
        Task<int> ResetState([NotNull]DbConnection connection);
    }

    class StateRepository : Repository, IStateRepository
    {
        private const string ResetProc = "ResetState";
        private const string DownloadProcessingParam = "DownloadProcessingState";
        private const string DownloadNotProcessedParam = "DownloadNotProcessedState";
        private const string RequestProcessingParam = "RequestProcessingState";
        private const string RequestNotProcessedParam = "RequestNotProcessedState";

        public async Task<int> ResetState(DbConnection connection)
        {
            var cmd = StoredProcedureCommand(connection, ResetProc);
            AddParameter(cmd, DownloadProcessingParam, (int) BlankDownloadState.Downloading);
            AddParameter(cmd, DownloadNotProcessedParam, (int)BlankDownloadState.Queued);
            AddParameter(cmd, RequestProcessingParam, (int)BlankRequestState.Zipping);
            AddParameter(cmd, RequestNotProcessedParam, (int)BlankRequestState.Queued);
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
