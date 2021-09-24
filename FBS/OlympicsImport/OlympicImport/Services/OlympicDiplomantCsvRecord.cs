using System;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

namespace OlympicImport.Services
{
    public class OlympicDiplomantCsvRecord
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SchoolRegion { get; set; }
        public long? SchoolEgeCode  { get; set; }
        public string SchoolEgeName { get; set; }
        public int? FormNumber { get; set; }
        public string RegCode { get; set; }
        public int? ResultLevel { get; set; }
        public string EgeId { get; set; }
        private ICollection<Guid> _participantId;
        public ICollection<Guid> ParticipantId
        {
            get { return _participantId ?? (_participantId = new List<Guid>()); }
            set { _participantId = value; }
        }

        public static OlympicDiplomantCsvRecord ParseCsv(CsvReader csv)
        {
            return new OlympicDiplomantCsvRecord
                {
                    BirthDate = csv[OlympicsImportSchema.BirthDate].AsDate(),
                    EgeId = csv[OlympicsImportSchema.EgeId],
                    FirstName = csv[OlympicsImportSchema.FirstName],
                    FormNumber = csv[OlympicsImportSchema.FormNumber].AsNullableInt(),
                    Id = csv[OlympicsImportSchema.Id].AsLong(),
                    LastName = csv[OlympicsImportSchema.LastName],
                    MiddleName = csv[OlympicsImportSchema.MiddleName],
                    ParticipantId = csv[OlympicsImportSchema.EgeId].AsGuidCollection(),
                    RegCode = csv[OlympicsImportSchema.RegCode],
                    ResultLevel = csv[OlympicsImportSchema.ResultLevel].AsNullableInt(),
                    SchoolEgeCode = csv[OlympicsImportSchema.SchoolEgeCode].AsNullableLong(),
                    SchoolEgeName = csv[OlympicsImportSchema.SchoolEgeName],
                    SchoolRegion = csv[OlympicsImportSchema.SchoolRegion]
                };
        }
    }
}