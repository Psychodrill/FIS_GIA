namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class RequestedParticipantMapper : DataReaderMapper<RequestedParticipant>
    {
        private const string RbdId = "ParticipantRbdId";
        private const string Hash = "ParticipantHash";
        private const string DocumentNumber = "DocumentNumber";
        private const string RegionId = "RegionId";
        private const string State = "State";
        private const string RegionName = "RegionName";

        public override async Task<RequestedParticipant> Map(DbDataReader @from)
        {
            var idOrdinal = GetOrdinal(from, "Id");
            var rbdIdOrdinal = GetOrdinal(from, RbdId);
            var hashOrdinal = GetOrdinal(from, Hash);
            var documentOrdinal = GetOrdinal(from, DocumentNumber);
            var regionOrdinal = GetOrdinal(from, RegionId);
            var stateOrdinal = GetOrdinal(from, State);
            var regionNameOrdinal = GetOrdinal(from, RegionName);

            if (!(await from.ReadAsync()))
            {
                return null;
            }

            var result = new RequestedParticipant
                {
                    Id = from.GetInt32(idOrdinal),
                    RbdId = from.GetGuid(rbdIdOrdinal),
                    Hash = from.GetString(hashOrdinal),
                    DocumentNumber = from.GetString(documentOrdinal),
                    RegionId = from.GetInt32(regionOrdinal),
                    Region = await from.GetNullableStringAsync(regionNameOrdinal),
                };

            do
            {
                result.IsCollision |= from.GetGuid(rbdIdOrdinal) != result.RbdId;

                var state = (BlankDownloadState?) await from.GetNullableInt32Async(stateOrdinal);
                switch (state)
                {
                    case null:
                        result.HasNoBlanks = true;
                        result.ProcessingUnfinished = true;
                        break;
                    case BlankDownloadState.Queued:
                    case BlankDownloadState.Downloading:
                        result.ProcessingUnfinished = true;
                        break;
                    case BlankDownloadState.Downloaded:
                        break;
                    case BlankDownloadState.Error:
                        result.HasErrors = true;
                        break;
                    default:
                        result.ProcessingUnfinished = result.HasErrors = true;
                        break;
                }
            } while (await from.ReadAsync());

            return result;
        }
    }
}
