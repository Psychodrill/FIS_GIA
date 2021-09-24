using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace Fbs.Core.Reports.Excel
{
    /// <summary>
    /// Сериализация DataView в XML
    /// </summary>
    public class XmlDataViewSerialiser
    {
        private readonly MemoryStream mMemoryStream;
        private readonly string mReportName;
        private readonly XmlWriter mWriter;
        private bool mAllowWrite;

        public XmlDataViewSerialiser(string reportName)
        {
            mReportName = reportName;
            mAllowWrite = true;
            mMemoryStream = new MemoryStream();

            var xmlWriterSettings = new XmlWriterSettings {Encoding = Encoding.UTF8, OmitXmlDeclaration = true};

            mWriter = XmlWriter.Create(mMemoryStream, xmlWriterSettings);
            BeginXmlWrite();
        }

        /// <summary>
        /// Начинаем формирование xml-строки
        /// </summary>
        private void BeginXmlWrite()
        {
            if (mAllowWrite)
            {
                mWriter.WriteStartDocument();
                mWriter.WriteStartElement("Tables");
                mWriter.WriteAttributeString("reportName", mReportName);
            }
        }

        private void EndXmlWrite()
        {
            if (mAllowWrite)
            {
                mWriter.WriteEndElement();
                mWriter.WriteEndDocument();
                mWriter.Flush();
                mWriter.Close();
                mAllowWrite = false;
            }
        }

        /// <summary>
        /// Получаем xml-закрываем и возвращаем
        /// </summary>
        /// <returns></returns>
        public string GetXmlAndClose()
        {
            EndXmlWrite();

            var readerXml = new StreamReader(mMemoryStream);
            mMemoryStream.Position = 0;
            return readerXml.ReadToEnd();
        }

        /// <summary>
        /// Добавляем таблицу
        /// </summary>
        /// <param name="dv"></param>
        public void AddTable(DataTableWithTag table)
        {
            if (mAllowWrite)
            {
                mWriter.WriteStartElement("Table");
            
                mWriter.WriteAttributeString("name",table.Tag. ReportName);
                mWriter.WriteAttributeString("code", table.Tag.ExtractorMethodName);


                // колонки
                mWriter.WriteStartElement("Columns");
                foreach (DataColumn column in table.Columns)
                {
                    mWriter.WriteStartElement("Column");
                    mWriter.WriteAttributeString("name", column.ColumnName);
                    mWriter.WriteAttributeString("userName", column.Caption);
                    mWriter.WriteAttributeString("type", column.DataType.ToString());
                    mWriter.WriteEndElement();
                }
                mWriter.WriteEndElement();

                mWriter.WriteStartElement("Data");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow  row = table.Rows[i];
                    mWriter.WriteStartElement("Row");
                    // запись значения
                    foreach (DataColumn column in table.Columns)
                    {
                        mWriter.WriteStartElement("Cell");
                        if (row[column.ColumnName] != DBNull.Value)
                            mWriter.WriteValue(row[column.ColumnName]);
                        mWriter.WriteEndElement();
                    }
                    mWriter.WriteEndElement();
                }
                mWriter.WriteEndElement();

                mWriter.WriteEndElement();
            }
            else
                throw new ApplicationException(
                    "Заперещено добавление представлений, так как уже было произведено чтение!");
        }
    }
}