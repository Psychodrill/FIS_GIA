namespace Ege.Dal.Common.Bulk
{
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

        public async Task<int> MergeData<TDto>(
            DbConnection connection,
            DbTransaction transaction = null)
        {
            var cmd = StoredProcedureCommand(connection, _procedureNameGetter.GetName<TDto>());
            cmd.CommandTimeout = 17200;
            cmd.Transaction = transaction;
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
