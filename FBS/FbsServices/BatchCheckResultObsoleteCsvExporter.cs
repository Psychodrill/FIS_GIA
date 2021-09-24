using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FbsServices
{
    public class BatchCheckResultObsoleteCsvExporter
    {
        /*
         Номер свидетельства%Типографский номер%Фамилия%Имя%Отчество%Серия документа% Номер документа%Регион%Год%Статус%Русский язык%Апелляция% Математика %Апелляция %Физика%Апелляция%Химия%Апелляция%Биология%Апелляция%История России%Апелляция %География%Апелляция%Английский язык%Апелляция%Немецкий язык%Апелляция %Французский язык%Апелляция%Обществознание%Апелляция% Литература %Апелляция%Испанский язык%Апелляция%Информатика%Апелляция% Проверок ВУЗами и их филиалами
         */
        public const string Header = "Номер свидетельства%Типографский номер%Фамилия%Имя%Отчество%Серия документа%Номер документа%Регион%Год%Статус%Русский язык%Апелляция%Математика%Апелляция%Физика%Апелляция%Химия%Апелляция%Биология%Апелляция%История России%Апелляция%География%Апелляция%Английский язык%Апелляция%Немецкий язык%Апелляция%Французский язык%Апелляция%Обществознание%Апелляция%Литература%Апелляция%Испанский язык%Апелляция%Информатика%Апелляция%Сочинение%Апелляция%Изложение%Апелляция%Математика базовая%Апелляция%Английский язык (устный)%Апелляция%Немецкий язык (устный)%Апелляция%Французский язык (устный)%Апелляция%Испанский язык (устный)%Апелляция%Проверок ВУЗами и их филиалами";
        public const string LicenseNumber = "LicenseNumber";
        public const string TypographicNumber = "TypographicNumber";
        public const string Surname = "Surname";
        public const string Name = "Name";
        public const string SecondName = "SecondName";
        public const string DocumentSeries = "DocumentSeries";
        public const string DocumentNumber = "DocumentNumber";
        public const string RegionCode = "RegionId";
        public const string UseYear = "UseYear";
        public const string StatusName = "StatusName";
        public const string UniqueChecks = "UniqueChecks";
        public const string SEPARATOR = "%";
        public const string ZERO = "0";
        private bool _headersWritten;

        // порядок выгрузки полей с оценками и апелляциями по предметам
        // таблица [dat].[Subjects]
        private static readonly int[] SubjectMarksOrder = new[]
            {
                1, // Русский язык
                2, // Математика
                3, // Физика
                4, // Химия
                6, // Биология
                7, // История
                8, // География
                9, // Английский язык
                10, // Немецкий язык
                11, // Французский язык
                12, // Обществознание
                18, // Литература
                13, // Испанский язык
                5, // Информатика и ИКТ
                20, //Сочинение
                21, //Изложение
                22, //Математика базовая
                29, //Английский язык (устный)
                30, //Немецкий язык (устный)
                31, //Французcкий язык (устный)
                33, //Испанский язык (устный)
            };

        public void Export(StreamWriter writer, DataTable chunk)
        {
            if (!_headersWritten)
            {
                writer.WriteLine(Header);
                _headersWritten = true;
            }
            
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
            string statusName = GetRowValue<string>(row, StatusName);

            if (!string.IsNullOrEmpty(statusName) && statusName.ToLower().Trim() == "не найдено")
            {
                yield return statusName.ToLower().Trim(); // в поле свид-во пишем "не найдено"
                yield return string.Empty; // поле ТН пустое
            }
            else // статус отличается от "не найдено"
            {
                string licenseNumber = GetRowValue<string>(row, LicenseNumber);

                if (string.IsNullOrEmpty(licenseNumber))
                {
                    yield return "нет свидетельства"; // в поле свид-во пишем "нет свидетельства"
                    yield return string.Empty; // поле ТН пустое
                }
                else
                {
                    yield return licenseNumber; // в поле свид-во пишем номер свид-ва
                    yield return GetRowValue<string>(row, TypographicNumber); // в поле ТН пишем типографский номер
                }
            }
            
            // пишем остальные поля в требуемом порядке
            yield return GetRowValue<string>(row, Surname);
            yield return GetRowValue<string>(row, Name);
            yield return GetRowValue<string>(row, SecondName);
            yield return GetRowValue<string>(row, DocumentSeries);
            yield return GetRowValue<string>(row, DocumentNumber);
            yield return GetRowValue<int>(row, RegionCode);
            yield return GetRowValue<int>(row, UseYear);
            yield return GetRowValue<string>(row, StatusName);

            // пишем баллы и оценки по предметам в заданном в SubjectMarksOrder порядке
            for (int i = 0; i < SubjectMarksOrder.Length; i++)
            {
                int id = SubjectMarksOrder[i];
                string subjectName = string.Format("sbj{0}", id);
                string subjectAppeal = string.Format("sbj{0}ap", id);
                string mark = GetRowValue<string>(row, subjectName);
                string appeal = (mark == null || mark.Trim().Length == 0) ? ZERO : GetRowValue<string>(row, subjectAppeal);
                if (appeal == null || appeal.Trim().Length == 0)
                {
                    appeal = ZERO;
                }

                yield return mark;
                yield return appeal;
            }

            yield return GetRowValue<int>(row, UniqueChecks);
        }
    }
}