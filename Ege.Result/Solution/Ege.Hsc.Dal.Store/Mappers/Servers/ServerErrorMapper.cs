namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Servers;

    class ServerErrorMapper : DataReaderCollectionMapper<BlankServerError>
    {
        public override async Task<ICollection<BlankServerError>> Map(DbDataReader @from)
        {
            var code = GetOrdinal(from, "Code");
            var examDate = GetOrdinal(from, "ExamDate");
            var error = GetOrdinal(from, "Error");
            var rbdId = GetOrdinal(from, "RbdId");

            var result = new List<BlankServerError>();
            while (await from.ReadAsync())
            {
                result.Add(new BlankServerError
                {
                    Code = from.GetString(code),
                    ExamDate = from.GetDateTime(examDate),
                    RbdId = await from.GetNullableGuidAsync(rbdId),
                    Error = (BlankServerErrorType)from.GetInt32(error),
                });
            }
            return result;
        }
    }
}
