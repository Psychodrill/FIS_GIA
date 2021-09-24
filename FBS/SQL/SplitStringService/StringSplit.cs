using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace SQLRegEx
{
    public class StringSplit
    {
        #region Parse_LicenseNumberFio
        public struct LicenseNumberFioRow
        {
            public LicenseNumberFioRow(int lineNubmer, 
                SqlString licenseNumber, SqlString firstName, SqlString lastName, SqlString middleName,
                SqlString checkLicenseNumber, SqlString checkFirstName, SqlString checkLastName, SqlString checkMiddleName)
            {
                LineNubmer = lineNubmer;
                LicenseNumber = licenseNumber;
                FirstName = firstName;
                LastName = lastName;
                MiddleName = middleName;
                CheckLicenseNumber = checkLicenseNumber;
                CheckFirstName = checkFirstName;
                CheckLastName = checkLastName;
                CheckMiddleName = checkMiddleName;
            }

            public int LineNubmer;
            public SqlString LicenseNumber;
            public SqlString FirstName;
            public SqlString LastName;
            public SqlString MiddleName;
            public SqlString CheckLicenseNumber;
            public SqlString CheckFirstName;
            public SqlString CheckLastName;
            public SqlString CheckMiddleName;
        }

        [SqlFunction(FillRowMethodName = "Fill_LicenseNumberFioRow")]
        public static IEnumerable Parse_LicenseNumberFio(SqlString sourceString, string delimiter)
        {
            char delim = delimiter[0];
            var source = sourceString.Value.Replace("\r\n", "$").Replace("\r", "$").Replace("\n", "$").Replace('$', delim);
            var items = source.ToUpper().Replace('Ё', 'Е').Replace('Й', 'И').Split(delim);
            var originalItems = source.Split(delim);

            for (int lineNumber = 0; lineNumber < items.Length; lineNumber++)
            {
                var columns = items[lineNumber].Split('%');
                var originalColumns = originalItems[lineNumber].Split('%');
                
                if (columns.Length < 3 || originalColumns.Length < 3)
                    continue;

                yield return new LicenseNumberFioRow(lineNumber,
                    originalColumns[0], originalColumns[1], originalColumns[2], originalColumns[3],
                    columns[0], columns[1], columns[2], columns[3]);
            }
        }

        /// <summary>
        /// <номер свидетельства>%<фамилия>%<имя>%<отчество>%
        /// </summary>
        public static void Fill_LicenseNumberFioRow(object obj, out int lineNubmer, 
            out SqlString licenseNumber, out SqlString firstName, out SqlString lastName, out SqlString middleName,
            out SqlString checkLicenseNumber, out SqlString checkFirstName, out SqlString checkLastName, out SqlString checkMiddleName)
        {
            var r = (LicenseNumberFioRow)obj;
            lineNubmer = r.LineNubmer;
            licenseNumber = r.LicenseNumber;
            firstName = r.FirstName;
            lastName = r.LastName;
            middleName = r.MiddleName;
            checkLicenseNumber = r.CheckLicenseNumber;
            checkFirstName = r.CheckFirstName;
            checkLastName = r.CheckLastName;
            checkMiddleName = r.CheckMiddleName;
        }
        #endregion

        #region Parse_TypographicNumberFio
        public struct TypographicNumberFioRow
        {
            public TypographicNumberFioRow(int lineNubmer, 
                SqlString typographicNumber, SqlString firstName, SqlString lastName, SqlString middleName,
                SqlString checkTypographicNumber, SqlString checkFirstName, SqlString checkLastName, SqlString checkMiddleName)
            {
                LineNubmer = lineNubmer;
                TypographicNumber = typographicNumber;
                FirstName = firstName;
                LastName = lastName;
                MiddleName = middleName;
                CheckTypographicNumber = checkTypographicNumber;
                CheckFirstName = checkFirstName;
                CheckLastName = checkLastName;
                CheckMiddleName = checkMiddleName;
            }

            public int LineNubmer;
            public SqlString TypographicNumber;
            public SqlString FirstName;
            public SqlString LastName;
            public SqlString MiddleName;
            public SqlString CheckTypographicNumber;
            public SqlString CheckFirstName;
            public SqlString CheckLastName;
            public SqlString CheckMiddleName;
        }

        [SqlFunction(FillRowMethodName = "Fill_TypographicNumberFioRow")]
        public static IEnumerable Parse_TypographicNumberFio(SqlString sourceString, string delimiter)
        {
            char delim = delimiter[0];
            var source = sourceString.Value.Replace("\r\n", "$").Replace("\r", "$").Replace("\n", "$").Replace('$', delim);
            var items = source.ToUpper().Replace('Ё', 'Е').Replace('Й', 'И').Split(delim);
            var originalItems = source.Split(delim);

            for (int lineNumber = 0; lineNumber < items.Length; lineNumber++)
            {
                var columns = items[lineNumber].Split('%');
                var originalColumns = originalItems[lineNumber].Split('%');

                if (columns.Length < 3 || originalColumns.Length < 3)
                    continue;

                yield return new TypographicNumberFioRow(lineNumber,
                    originalColumns[0], originalColumns[1], originalColumns[2], originalColumns[3],
                    columns[0], columns[1], columns[2], columns[3]);
            }
        }

        /// <summary>
        /// <типографский номер>%<фамилия>%<имя>%<отчество>
        /// </summary>
        public static void Fill_TypographicNumberFioRow(object obj, out int lineNubmer, 
            out SqlString typographicNumber, out SqlString firstName, out SqlString lastName, out SqlString middleName,
            out SqlString checkTypographicNumber, out SqlString checkFirstName, out SqlString checkLastName, out SqlString checkMiddleName)
        {
            var r = (TypographicNumberFioRow)obj;
            lineNubmer = r.LineNubmer;
            typographicNumber = r.TypographicNumber;
            firstName = r.FirstName;
            lastName = r.LastName;
            middleName = r.MiddleName;
            checkTypographicNumber = r.CheckTypographicNumber;
            checkFirstName = r.CheckFirstName;
            checkLastName = r.CheckLastName;
            checkMiddleName = r.CheckMiddleName;
        }
        #endregion

        #region Parse_FioDocumentNumberSeries
        public struct FioDocumentNumberSeriesRow
        {
            public FioDocumentNumberSeriesRow(int lineNubmer, 
                SqlString firstName, SqlString lastName, SqlString middleName, SqlString documentSeries, SqlString documentNumber,
                SqlString checkFirstName, SqlString checkLastName, SqlString checkMiddleName, SqlString checkDocumentSeries, SqlString checkDocumentNumber)
            {
                LineNubmer = lineNubmer;                
                FirstName = firstName;
                LastName = lastName;
                MiddleName = middleName;                
                DocumentSeries = documentSeries;
                DocumentNumber = documentNumber;
                CheckFirstName = checkFirstName;
                CheckLastName = checkLastName;
                CheckMiddleName = checkMiddleName;
                CheckDocumentSeries = checkDocumentSeries;
                CheckDocumentNumber = checkDocumentNumber;
            }

            public int LineNubmer;            
            public SqlString FirstName;
            public SqlString LastName;
            public SqlString MiddleName;
            public SqlString DocumentNumber;
            public SqlString DocumentSeries;
            public SqlString CheckFirstName;
            public SqlString CheckLastName;
            public SqlString CheckMiddleName;
            public SqlString CheckDocumentNumber;
            public SqlString CheckDocumentSeries;
        }

        [SqlFunction(FillRowMethodName = "Fill_FioDocumentNumberSeriesRow")]
        public static IEnumerable Parse_FioDocumentNumberSeries(SqlString sourceString, string delimiter)
        {
            char delim = delimiter[0];
            var source = sourceString.Value.Replace("\r\n", "$").Replace("\r", "$").Replace("\n", "$").Replace('$', delim);
            var items = source.ToUpper().Replace('Ё', 'Е').Replace('Й', 'И').Split(delim);
            var originalItems = source.Split(delim);

            for (int lineNumber = 0; lineNumber < items.Length; lineNumber++)
            {
                var columns = items[lineNumber].Split('%');
                var originalColumns = originalItems[lineNumber].Split('%');

                if (columns.Length < 4 || originalColumns.Length < 4)
                    continue;

                yield return new FioDocumentNumberSeriesRow(lineNumber,
                    originalColumns[0], originalColumns[1], originalColumns[2], originalColumns[3], originalColumns[4],
                    columns[0], columns[1], columns[2], columns[3], columns[4]);
            }
        }

        /// <summary>
        /// <фамилия>%<имя>%<отчество>%<серия документа>%<номер документа>
        /// </summary>
        public static void Fill_FioDocumentNumberSeriesRow(object obj, out int lineNubmer, 
            out SqlString firstName, out SqlString lastName, out SqlString middleName, out SqlString documentSeries, out SqlString documentNumber,
            out SqlString checkFirstName, out SqlString checkLastName, out SqlString checkMiddleName, out SqlString checkDocumentSeries, out SqlString checkDocumentNumber)
        {
            var r = (FioDocumentNumberSeriesRow)obj;
            lineNubmer = r.LineNubmer;            
            firstName = r.FirstName;
            lastName = r.LastName;
            middleName = r.MiddleName;
            documentSeries = r.DocumentSeries;
            documentNumber = r.DocumentNumber;
            checkFirstName = r.CheckFirstName;
            checkLastName = r.CheckLastName;
            checkMiddleName = r.CheckMiddleName;
            checkDocumentSeries = r.CheckDocumentSeries;
            checkDocumentNumber = r.CheckDocumentNumber;
        }
        #endregion
    }
}
