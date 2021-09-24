using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fbs.Core.WebServiceCheck
{
    public class WSXMLtoCSV
    {

        #region Private Methods

        public StreamReader GetPassportCSV(List<QueryItem> queryList)
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in queryList)
            {
                txt.WriteLine(string.Format("{0}%{1}%{2}%{3}%{4}",
                    item.QueryLastName,
                    item.QueryFirstName,
                    item.QueryPatronymicName,
                    item.QueryPassportSeria,
                    item.QueryPassportNumber));
            }
            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }

        /*
        public string WriteCsvArray<T>(IEnumerable<T> list)
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            new CsvContext().Write(list ?? new List<T>(), txt, new CsvFileDescription
            {
                SeparatorChar = '%',
                QuoteAllFields = true, //Ставить кавычки только у тех полей, которые содержат знак разделителя
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.GetEncoding(1251)
            });

            txt.Flush();
            stream.Position = 0;
            TextReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
         */

        #endregion
    }
}
