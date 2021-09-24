namespace Ege.Hsc.Dal.Store.Mappers.Requests
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class RequestPermissionMapper : DataReaderMapper<RequestPermission>
    {
        public async override Task<RequestPermission> Map(DbDataReader @from)
        {
            if (!await from.ReadAsync())
            {
                return default(RequestPermission);
            }
            var multi = GetOrdinal(from, "MultiRequestPermission");
            var single = GetOrdinal(from, "SingleRequestPermission");
            return new RequestPermission
            {
                MultiRequestPermission = from.GetBoolean(multi),
                SingleRequestPermission = from.GetBoolean(single),
            };
        }
    }
}
