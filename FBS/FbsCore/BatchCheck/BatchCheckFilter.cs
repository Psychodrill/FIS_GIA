using System;
using System.Data;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckFilter
    {
        public int[] SubjectIds { get; private set; }
        public string FilterString { get; private set; }

        private BatchCheckFilter() { }

        public static BatchCheckFilter Parse(string filterString)
        {
            BatchCheckFilter filter = new BatchCheckFilter();
            filter.FilterString = filterString;
            string[] subjects = filterString.Trim().Split(BatchCheckFormat.SEPARATOR);
            if (subjects.Length < 2)
            {
                throw new ArgumentException("Неверный формат фильтра: должен быть указан хотя бы один код предмета");
            }
            filter.SubjectIds = new int[subjects.Length - 1];
            for (int i = 1; i < subjects.Length; i++)
            {
                int subjectIndex;
                if (int.TryParse(subjects[i].Trim(), out subjectIndex) && subjectIndex > 0 && subjectIndex <= 14)
                {
                    filter.SubjectIds[i - 1] = subjectIndex;
                }
                else
                {
                    throw new ArgumentException(string.Format("Неверный формат фильтра: "
                     + "недопустимый номер предмета {0} (ожидается целое число от 1 до 14)", subjects[i]));
                }
            }
            return filter;
        }

        public static BatchCheckFilter Parse(string filterString, ref DataTable dataTable)
        {
            BatchCheckFilter filter = new BatchCheckFilter();
            filter.FilterString = filterString;
            string[] subjects = filterString.Trim().Split(BatchCheckFormat.SEPARATOR);
            if (subjects.Length < 2)
            {
                DataRow reportRow = dataTable.NewRow();
                reportRow["RowIndex"] = "[НЕВЕРЕН ФОРМАТ]";
                reportRow["Комментарий"] = "П";
                dataTable.Rows.Add(reportRow);
            }
            filter.SubjectIds = new int[subjects.Length - 1];
            for (int i = 1; i < subjects.Length; i++)
            {
                int subjectIndex;
                if (int.TryParse(subjects[i].Trim(), out subjectIndex) && subjectIndex > 0 && subjectIndex <= 14)
                {
                    filter.SubjectIds[i - 1] = subjectIndex;
                }
                else
                {
                    DataRow reportRow = dataTable.NewRow();
                    reportRow["RowIndex"] = "[НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    dataTable.Rows.Add(reportRow);
                }
            }
            return filter;
        }

        public static BatchCheckFilter EmptyFilter()
        {
            BatchCheckFilter filter = new BatchCheckFilter();
            filter.SubjectIds = new int[14];
            for (int i = 0; i < filter.SubjectIds.Length; i++)
            {
                filter.SubjectIds[i] = i + 1;
            }
            return filter;
        }
    }
}
