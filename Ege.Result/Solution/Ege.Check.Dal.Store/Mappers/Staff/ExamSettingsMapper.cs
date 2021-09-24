namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;

    internal class ExamSettingsMapper : DataReaderMapper<ExamSettings>
    {
        private const string ExamGlobalId = "ExamGlobalId";
        private const string SubjectName = "SubjectName";
        private const string ExamDate = "ExamDate";
        private const string ShowResult = "ShowResult";
        private const string ShowBlanks = "ShowBlanks";
        private const string HasGekDocument = "HasGekDocument";
        private const string IsComposition = "IsComposition";

        public override async Task<ExamSettings> Map(DbDataReader @from)
        {
            var settings = new List<ExamSetting>();
            var result = new ExamSettings {Settings = settings};

            var examGlobalId = GetOrdinal(from, ExamGlobalId);
            var subjectName = GetOrdinal(from, SubjectName);
            var examDate = GetOrdinal(from, ExamDate);
            var showResult = GetOrdinal(from, ShowResult);
            var showBlanks = GetOrdinal(from, ShowBlanks);
            var hasGekDocuments = GetOrdinal(from, HasGekDocument);
            var isComposition = GetOrdinal(from, IsComposition);

            while (await from.ReadAsync())
            {
                settings.Add(new ExamSetting
                    {
                        ExamDate = from.GetDateTime(examDate),
                        ShowResult = from.GetBoolean(showResult),
                        ExamGlobalId = from.GetInt32(examGlobalId),
                        HasGekDocument = from.GetBoolean(hasGekDocuments),
                        SubjectName = from.GetString(subjectName),
                        ShowBlank = from.GetBoolean(showBlanks),
                        IsComposition = from.GetBoolean(isComposition),
                    });
            }
            return result;
        }
    }
}