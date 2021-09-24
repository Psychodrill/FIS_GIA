namespace Ege.Check.Logic.Services.Staff.Rcoi
{
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Repositories.Regions;
    using Ege.Check.Logic.Helpers;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class RcoiInfoService : IRcoiInfoService
    {
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IUrlCorrector _corrector;
        [NotNull] private readonly IRcoiInfoRepository _repository;

        public RcoiInfoService(
            [NotNull] IRcoiInfoRepository repository,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IUrlCorrector corrector)
        {
            _repository = repository;
            _connectionFactory = connectionFactory;
            _corrector = corrector;
        }

        public async Task<RcoiInfo> GetRcoiInfoByRegion(int regionId)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.GetById(connection, regionId);
            }
        }

        public async Task UpdateRcoiInfo(int regionId, RcoiInfo rcoi)
        {
            rcoi.BlanksServer = _corrector.Correct(rcoi.BlanksServer);
            rcoi.CompositionBlanksServer = _corrector.Correct(rcoi.CompositionBlanksServer);
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.UpdateRcoiInfo(connection, regionId, rcoi);
            }
        }
    }
}