using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckPreprocessor
    {
        private BatchCheckFilter filter;

        public BatchCheckPreprocessor(BatchCheckFilter filter)
        {
            this.filter = filter;
        }

        public Stream Process(Stream sourceStream)
        {
            Stream destStream = new MemoryStream();
            Encoding encoding = Encoding.GetEncoding(1251);
            using (var reader = new StreamReader(sourceStream, encoding, true))
            {
                reader.BaseStream.Position = 0;
                var writer = new StreamWriter(destStream, encoding);

                // Пропускаем первую строку с фильтром
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(ProcessRow(line));
                }
                writer.Flush();

            }
            return destStream;
        }

        private string ProcessRow(string row)
        {
            string[] rowCells = row.Trim().Split(BatchCheckFormat.SEPARATOR);
            if (rowCells.Length <= filter.SubjectIds.Length)
            {
                throw new ArgumentException("Указанный фильтр неприменим к данной строке");
            }

            string[] filteredCells = new string[filter.SubjectIds.Length];
            string[] staticCells = new string[rowCells.Length - filteredCells.Length];
            Array.Copy(rowCells, staticCells.Length, filteredCells, 0, filteredCells.Length);
            Array.Copy(rowCells, staticCells, staticCells.Length);
            string[] expandedCells = new string[14];
            for (int i = 0; i < filteredCells.Length; i++)
            {
                int expandedIndex = filter.SubjectIds[i] - 1;
                expandedCells[expandedIndex] = filteredCells[i];
            }
            return string.Join(BatchCheckFormat.SEPARATOR.ToString(), staticCells)
                + BatchCheckFormat.SEPARATOR.ToString()
                + string.Join(BatchCheckFormat.SEPARATOR.ToString(), expandedCells);
        }
    }
}
