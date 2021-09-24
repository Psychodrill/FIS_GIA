namespace Ege.Check.Dal.Store.Repositories
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;
    using global::Common.Logging;

    public static class DbCommandExtensions
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<Repository>();

        public static async Task<DbDataReader> ExecuteReaderWithTimeElapsedLogAsync([NotNull] this DbCommand command)
        {
            var now = DateTime.Now;
            var result = await command.ExecuteReaderAsync();
            Logger.TraceFormat("{0} executed in {1} ms", command.CommandText, (DateTime.Now - now).Milliseconds);
            return result;
        }
    }
}