namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Servers;

    class ServerBlanksMapper : DataReaderCollectionMapper<ServerBlanks>
    {
        public override async Task<ICollection<ServerBlanks>> Map(DbDataReader @from)
        {
            var regionId = GetOrdinal(from, "RegionId");
            var url = GetOrdinal(from, "Url");
            var code = GetOrdinal(from, "Code");
            var examDate = GetOrdinal(from, "ExamDate");
            var subjectCode = GetOrdinal(from, "SubjectCode");

            var result = new List<ServerBlanks>();
            ServerBlanks current = null;
            ISet<string> currentExamBlanks = null;
            int? lastRegion = null;
            ExamFolder? lastFolder = null;
            while (await from.ReadAsync())
            {
                var currentRegion = from.GetInt32(regionId);
                if (currentRegion != lastRegion)
                {
                    current = new ServerBlanks
                    {
                        RegionId = currentRegion,
                        Url = from.GetString(url),
                        Blanks = new Dictionary<ExamFolder, ISet<string>>(),
                    };
                    result.Add(current);
                    lastRegion = currentRegion;
                }
                var currentFolder = new ExamFolder(from.GetDateTime(examDate), from.GetInt32(subjectCode));
                if (currentFolder != lastFolder)
                {
                    currentExamBlanks = new HashSet<string>();
                    current.Blanks.Add(currentFolder, currentExamBlanks);
                    lastFolder = currentFolder;
                }
                currentExamBlanks.Add(from.GetString(code));
            }

            return result;
        }
    }
}
