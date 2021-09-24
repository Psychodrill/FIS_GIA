namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Servers;

    class ServerErrorsTableMapper : IDataTableMapper<IEnumerable<ServerErrors>>
    {
        private const string CodeColumn = "Code";
        private const string ExamDateColumn = "ExamDate";
        private const string ErrorColumn = "Error";

        public DataTable Map(IEnumerable<ServerErrors> @from)
        {
            var result = new DataTable();
            result.Columns.Add(CodeColumn, typeof(string));
            result.Columns.Add(ExamDateColumn, typeof(DateTime));
            result.Columns.Add(ErrorColumn, typeof(int));
            foreach (var errors in from ?? Enumerable.Empty<ServerErrors>())
            {
                if (errors == null)
                {
                    continue;
                }
                foreach (var missingInDb in errors.Extra)
                {
                    result.Rows.Add(missingInDb, errors.ExamDate, (int) BlankServerErrorType.MissingInDb);
                }
                foreach (var missingOnServer in errors.Missing)
                {
                    result.Rows.Add(missingOnServer, errors.ExamDate, (int) BlankServerErrorType.MissingOnServer);
                }
            }
            return result;
        }
    }
}
