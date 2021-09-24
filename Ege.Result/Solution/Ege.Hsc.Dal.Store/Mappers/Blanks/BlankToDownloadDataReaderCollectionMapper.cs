namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    public class BlankToDownloadDataReaderCollectionMapper : DataReaderListMapper<BlankToDownload>
    {
        private const string IdName = "Id";
        private const string RelativePathName = "RelativePath";
        private const string ServerUrlName = "ServerUrl";
        private const string ParticipantRbdIdName = "ParticipantRbdId";
        private const string ParticipantHashName = "ParticipantHash";
        private const string ParticipantDocumentNumberName = "DocumentNumber";
        private const string OrderName = "Order";

        public override async Task<IList<BlankToDownload>> Map(DbDataReader @from)
        {
            var idOrdinal = GetOrdinal(from, IdName);
            var relativePathOrdinal = GetOrdinal(from, RelativePathName);
            var serverUrlOrdinal = GetOrdinal(from, ServerUrlName);
            var participantRbdIdOrdinal = GetOrdinal(from, ParticipantRbdIdName);
            var participantHashOrdinal = GetOrdinal(from, ParticipantHashName);
            var participantDocumentNumberOrdinal = GetOrdinal(from, ParticipantDocumentNumberName);
            var orderOrdinal = GetOrdinal(from, OrderName);

            var res = new List<BlankToDownload>();
            while (await @from.ReadAsync())
            {
                res.Add(new BlankToDownload
                    {
                        Id = from.GetInt32(idOrdinal),
                        RelativePath = from.GetString(relativePathOrdinal),
                        ServerUrl = await from.GetNullableStringAsync(serverUrlOrdinal),
                        ParticipantRbdId = from.GetGuid(participantRbdIdOrdinal),
                        ParticipantHash = from.GetString(participantHashOrdinal),
                        ParticipantDocumentNumber = from.GetString(participantDocumentNumberOrdinal),
                        Order = from.GetInt32(orderOrdinal)
                    });
            }
            return res;
        }
    }
}