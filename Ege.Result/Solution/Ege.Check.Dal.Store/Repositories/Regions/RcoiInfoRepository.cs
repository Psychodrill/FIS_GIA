namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;

    internal class RcoiInfoRepository : Repository, IRcoiInfoRepository
    {
        private const string GetProcedureName = "GetRcoiInfo";
        private const string UpdateProcedureName = "MergeRcoiInfo";

        private const string RegionParameterName = "@RegionId";
        private const string FioParameterName = "@Fio";
        private const string PhoneParameterName = "@Phone";
        private const string EmailParameterName = "@Email";
        private const string HotLinePhoneParameterName = "@HotLineData";
        private const string BlanksServerParameterName = "@BlanksServer";
        private const string CompositionBlanksServerParameterName = "@CompositionBlanksServer";
        private const string DescriptionParameterName = "@Description";

        private readonly IDataReaderMapper<RcoiInfo> _mapper;

        public RcoiInfoRepository(IDataReaderMapper<RcoiInfo> mapper)
        {
            _mapper = mapper;
        }

        public async Task<RcoiInfo> GetById(DbConnection connection, int regionId)
        {
            var command = connection.CreateCommand();
            command.CommandText = GetProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            var parameter = command.CreateParameter();
            parameter.ParameterName = RegionParameterName;
            parameter.Value = regionId;
            command.Parameters.Add(parameter);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task UpdateRcoiInfo(DbConnection connection, int regionId, RcoiInfo rcoi)
        {
            var command = StoredProcedureCommand(connection, UpdateProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, FioParameterName, rcoi.Fio);
            AddParameter(command, PhoneParameterName, rcoi.Phone);
            AddParameter(command, EmailParameterName, rcoi.Email);
            AddParameter(command, HotLinePhoneParameterName, rcoi.HotLinePhone);
            AddParameter(command, BlanksServerParameterName, rcoi.BlanksServer);
            AddParameter(command, CompositionBlanksServerParameterName, rcoi.CompositionBlanksServer);
            AddParameter(command, DescriptionParameterName, rcoi.Description);
            await command.ExecuteNonQueryAsync();
        }
    }
}