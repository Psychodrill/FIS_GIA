namespace Ege.Check.Dal.Store.Mappers
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class GekDocumentMapper : DataReaderMapper<GekDocument>
    {
        private const string ExamDate = "ExamDate";
        private const string SubjectName = "SubjectName";
        private const string Number = "Number";
        private const string CreateDate = "CreateDate";
        private const string Url = "Url";

        public override async Task<GekDocument> Map(DbDataReader @from)
        {
            var examDate = GetOrdinal(from, ExamDate);
            var subjectName = GetOrdinal(from, SubjectName);
            var number = GetOrdinal(from, Number);
            var createDate = GetOrdinal(from, CreateDate);
            var url = GetOrdinal(from, Url);

            GekDocument result = null;
            if (await from.ReadAsync())
            {
                result = new GekDocument
                    {
                        ExamDate = from.GetDateTime(examDate),
                        SubjectName = from.GetString(subjectName),
                        Number = from.GetString(number),
                        CreateDate = from.GetDateTime(createDate),
                        Url = await from.GetNullableStringAsync(url),
                    };
            }
            return result;
        }
    }
}