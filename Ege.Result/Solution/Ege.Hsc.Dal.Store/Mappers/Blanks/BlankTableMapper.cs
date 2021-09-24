namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class BlankTableMapper : IDataTableMapper<IEnumerable<BlankDownload>>
    {
        private const string ParticipantIdColumn = "ParticipantId";
        private const string RegionIdColumn = "RegionId";
        private const string StateColumn = "State";
        private const string OrderColumn = "Order";
        private const string RelativePathColumn = "RelativePath";
        private const string CodeColumn = "Code";
        private const string ExamDateColumn = "ExamDate";
        private const string SubjectCodeColumn = "SubjectCode";

        public DataTable Map(IEnumerable<BlankDownload> from)
        {
            var result = new DataTable();
            result.Columns.Add(ParticipantIdColumn, typeof(int));
            result.Columns.Add(RegionIdColumn, typeof(int));
            result.Columns.Add(StateColumn, typeof(int));
            result.Columns.Add(OrderColumn, typeof(int));
            result.Columns.Add(RelativePathColumn, typeof(string));
            result.Columns.Add(CodeColumn, typeof(string));
            result.Columns.Add(ExamDateColumn, typeof(DateTime));
            result.Columns.Add(SubjectCodeColumn, typeof(int));
            foreach (var blank in from ?? Enumerable.Empty<BlankDownload>())
            {
                if (blank == null)
                {
                    continue;
                }
                result.Rows.Add(blank.ParticipantId, blank.RegionId, blank.State, blank.Order, blank.RelativePath, blank.Code, blank.ExamDate, blank.SubjectCode);
            }
            return result;
        }
    }
}
