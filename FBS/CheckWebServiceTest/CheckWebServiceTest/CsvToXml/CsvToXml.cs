using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckWebService
{
    public abstract class CsvToXml
    {
        private string RowXmlPattern
        {
            get
            {
                return
                    "<query>" +
                        "<firstName>{1}</firstName>" +
                        "<lastName>{0}</lastName>" +
                        "<patronymicName>{2}</patronymicName>" +
                        "<passportSeria>{3}</passportSeria>" +
                        "<passportNumber>{4}</passportNumber>" +
                        "<certificateNumber>{5}</certificateNumber>" +
                        "<typographicNumber>{6}</typographicNumber>" +
                    "</query>";
            }
        }

        private string BeginXmlPattern
        {  
            get
            {
                return
                    "<items>" +
                        "<login></login>" +
                        "<password></password>" +
                        "<client>sample@sample.smp</client>" +
                        "<year>{0}</year>";
            }
        }
        private string EndXmlPattern { get { return "</items>"; } }

        protected abstract int SurnameCellIndex { get; }
        protected abstract int FirstnameCellIndex { get; }
        protected abstract int PatronymicCellIndex { get; }
        protected abstract int CneNumberCellIndex { get; }
        protected abstract int PassportNumberCellIndex { get; }
        protected abstract int PassportSeriaCellIndex { get; }
        protected abstract int TypographicNumberCellIndex { get; }


        StringBuilder stringBuilder = new StringBuilder();

        public string ExtractXml(TextReader csvReader, int year)
        {
            stringBuilder.AppendFormat(BeginXmlPattern, year);
            while (true)
            {
                string csvString = csvReader.ReadLine();
                if (String.IsNullOrEmpty(csvString))
                {
                    break;
                }
                string[] csvLine = csvString.Split('%');

                ApplendLine(csvLine);
            }

            stringBuilder.Append(EndXmlPattern);

            return stringBuilder.ToString();
        }

        private void ApplendLine(string[] csvLine)
        {
            string surname = GetStringFromCsvLine(csvLine, SurnameCellIndex);
            string firstname = GetStringFromCsvLine(csvLine, FirstnameCellIndex);
            string patronymic = GetStringFromCsvLine(csvLine, PatronymicCellIndex);
            string cneNumber = GetStringFromCsvLine(csvLine, CneNumberCellIndex);
            string passportNumber = GetStringFromCsvLine(csvLine, PassportNumberCellIndex);
            string passportSeria = GetStringFromCsvLine(csvLine, PassportSeriaCellIndex);
            string typographicNumber = GetStringFromCsvLine(csvLine, TypographicNumberCellIndex);
            stringBuilder.AppendFormat(RowXmlPattern, surname, firstname, patronymic, passportSeria, passportNumber, cneNumber, typographicNumber);
        }

        private string GetStringFromCsvLine(string[] csvLine, int cellIndex)
        {
            if (cellIndex < 0)
                return "";
            if (csvLine.Length <= cellIndex)
                return "";
            return csvLine[cellIndex];
        }
    }
}
