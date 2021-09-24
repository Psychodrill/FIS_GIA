namespace Ege.Check.Logic.Services.Staff.TaskSettings
{
    using System.Threading.Tasks;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Repositories.Answers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class AnswerCriteriaService : IAnswerCriteriaService
    {
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IAnswerCriteriaRepository _repository;

        public AnswerCriteriaService(
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IAnswerCriteriaRepository repository)
        {
            _connectionFactory = connectionFactory;
            _repository = repository;
        }

        public async Task<ExamInfoCacheModel> GetTaskSettings(int subjectCode)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                var result = await _repository.GetByCode(connection, subjectCode);
                return result;
            }
        }

        public async Task SetTaskSettings(int subjectCode, ExamInfoCacheModel model)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.SetByCode(connection, subjectCode, model);
            }
        }
    }
}
