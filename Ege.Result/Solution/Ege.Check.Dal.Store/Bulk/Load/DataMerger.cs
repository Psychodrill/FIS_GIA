namespace Ege.Check.Dal.Store.Bulk.Load
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Helpers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class DataMerger : Repository, IDataMerger
    {
        [NotNull] private readonly IProcedureNameGetter _procedureNameGetter;

        public DataMerger([NotNull] IProcedureNameGetter procedureNameGetter)
        {
            _procedureNameGetter = procedureNameGetter;
        }

        public async Task MergeData<TDto>(string fullTableName, DbConnection connection,
                                          DbTransaction transaction = null)
        {
            if (string.IsNullOrWhiteSpace(fullTableName))
            {
                throw new ArgumentNullException("fullTableName", "Tabla name can not be null or empty");
            }
            var cmd = StoredProcedureCommand(connection, _procedureNameGetter.GetName<TDto>());
            AddParameter(cmd, "TableName", fullTableName);
            cmd.Transaction = transaction;
            await cmd.ExecuteNonQueryAsync();
        }
    }
}