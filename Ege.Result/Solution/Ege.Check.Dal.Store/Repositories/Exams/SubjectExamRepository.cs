namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class SubjectExamRepository : Repository, ISubjectExamRepository
    {
        private const string ProcedureName = "GetExamList";

        [NotNull] private readonly IDataReaderMapper<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>> _mapper;

        public SubjectExamRepository(
            [NotNull] IDataReaderMapper<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>> mapper)
        {
            _mapper = mapper;
        }

        public async Task<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>> GetAll(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, ProcedureName);
            command.CommandTimeout = 300;
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _mapper.Map(reader);
            }
        }
    }
}
