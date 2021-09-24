using System.Collections.Generic;
using System.Linq;
using LumenWorks.Framework.IO.Csv;

namespace OlympicImport.Services
{
    public class OlympicsCsvRecord
    {
        public string CodeName { get; set; }
        public string OlympiadName { get; set; }
        public int OlympiadNumber { get; set; }
        public int? OlympiadLevel { get; set; }
        public int OlympiadYear { get; set; }
        public string OlympiadSubjectName { get; set; }
        public string OlympiadSubjectProfileName { get; set; }
        private IList<OlympicSubjectCsvRecord> _olympicThemeSubjects;

        public IList<OlympicSubjectCsvRecord> OlympicThemeSubjects
        {
            get { return _olympicThemeSubjects ?? (_olympicThemeSubjects = new List<OlympicSubjectCsvRecord>()); }
            set { _olympicThemeSubjects = value; }
        }

        private IList<OlympicSubjectCsvRecord> _olympicProfileSubjects;

        public IList<OlympicSubjectCsvRecord> OlympicProfileSubjects
        {
            get { return _olympicProfileSubjects ?? (_olympicProfileSubjects = new List<OlympicSubjectCsvRecord>()); }
            set { _olympicProfileSubjects = value; }
        }

        public long Id { get; set; }

        public static OlympicsCsvRecord ParseCsv(CsvReader csv)
        {
            return new OlympicsCsvRecord
                {
                    CodeName = csv[OlympicsImportSchema.CodeName],
                    OlympiadName = csv[OlympicsImportSchema.OlympiadName],
                    OlympiadLevel = csv[OlympicsImportSchema.OlympiadLevel].AsNullableInt(),
                    OlympiadNumber = csv[OlympicsImportSchema.OlympiadNumber].AsInt(),
                    OlympiadSubjectName = csv[OlympicsImportSchema.OlympiadSubjectName],
                    OlympiadSubjectProfileName = csv[OlympicsImportSchema.OlympiadSubjectProfileName],
                    OlympiadYear = ImportSettings.OlympiadYear,
                    OlympicThemeSubjects = csv[OlympicsImportSchema.OlympiadSubjectName].AsDistinctSubjectNames().Select(x => new OlympicSubjectCsvRecord{ SubjectName = x}).ToList(),
                    OlympicProfileSubjects = csv[OlympicsImportSchema.OlympiadSubjectProfileName].AsDistinctSubjectNames().Select(x => new OlympicSubjectCsvRecord { SubjectName = x }).ToList(),
                };
        }
    }
}