using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FbsServices
{
    public class BatchCheckResultCommonCsvExporter
    {
        /*
         фамилия%имя%отчество%серия док-та%номер док-та%предмет%балл%год%регион%статус
%признак наличия апелляции/перепроверки%номер свидетельства%типографский номер

         */
        public const string Surname = "Surname";
        public const string Name = "Name";
        public const string SecondName = "SecondName";
        public const string DocumentSeries = "DocumentSeries";
        public const string DocumentNumber = "DocumentNumber";
        public const string SubjectName = "SubjectName";
        public const string Mark = "Mark";
        public const string UseYear = "UseYear";
        public const string RegionCode = "RegionId";
        public const string StatusName = "StatusName";
        public const string HasAppeal = "HasAppeal";
        public const string LicenseNumber = "LicenseNumber";
        public const string TypographicNumber = "TypographicNumber";
        public const string AppealStatusName = "AppealStatusName";
        public const string SEPARATOR = "%";

        public void Export(StreamWriter writer, DataTable chunk)
        {
            foreach (DataRow dataRow in chunk.Rows)
            {
                ExportRow(writer, dataRow);
            }
        }

        private void ExportRow(StreamWriter writer, DataRow row)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string fieldValue in GetRowFieldValues(row))
            {
                if (sb.Length > 0)
                {
                    sb.Append(SEPARATOR);
                }

                if (fieldValue != null)
                {
                    sb.Append(fieldValue);
                }
            }

            writer.WriteLine(sb.ToString());
        }

        private static string GetRowValue<TValue>(DataRow row, string fieldName)
        {
            object rawValue;
            if (row.Table.Columns.Contains(fieldName) && !row.IsNull(fieldName))
            {
                rawValue = row[fieldName];
            }
            else
            {
                rawValue = default(TValue);
            }

            return rawValue != null ? rawValue.ToString() : null;
        }

        private static IEnumerable<string> GetRowFieldValues(DataRow row)
        {
            yield return GetRowValue<string>(row, Surname);
            yield return GetRowValue<string>(row, Name);
            yield return GetRowValue<string>(row, SecondName);
            yield return GetRowValue<string>(row, DocumentSeries);
            yield return GetRowValue<string>(row, DocumentNumber);
            yield return GetRowValue<string>(row, SubjectName);
            yield return GetRowValue<int>(row, Mark);
            yield return GetRowValue<int>(row, UseYear);
            yield return GetRowValue<int>(row, RegionCode);
            yield return GetRowValue<string>(row, StatusName);
            yield return GetRowValue<bool>(row, HasAppeal) == Boolean.TrueString ? "1" : "0";
            yield return GetRowValue<string>(row, LicenseNumber);
            yield return GetRowValue<string>(row, TypographicNumber);
            yield return GetRowValue<string>(row, AppealStatusName);
        }
    }
}