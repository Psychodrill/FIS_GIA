namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using JetBrains.Annotations;

    internal class AppealCollectionMapper : DataReaderMapper<AppealCollectionCacheModel>
    {
        [NotNull] private const string Date = "CreateDate";
        [NotNull] private const string Status = "AppealStatus";

        public override async Task<AppealCollectionCacheModel> Map(DbDataReader @from)
        {
            var date = GetOrdinal(from, Date);
            var status = GetOrdinal(from, Status);

            var appeals = new List<AppealCacheModel>();

            while (await from.ReadAsync())
            {
                appeals.Add(new AppealCacheModel
                    {
                        Date = from.GetDateTime(date),
                        Status = from.GetInt32(status),
                    });
            }
            var result = new AppealCollectionCacheModel
                {
                    Appeals = appeals,
                };
            return result;
        }
    }
}