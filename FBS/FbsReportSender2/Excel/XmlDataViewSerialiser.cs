using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace FbsReportSender.Excel
{
    /// <summary>
    /// ������������ DataView � XML
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
        /// �������� ������������ xml-������
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
        /// �������� xml-��������� � ����������
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
        /// ��������� �������
        /// </summary>
        /// <param name="dv"></param>
        public void AddDataView(DataView dv)
        {
            if (mAllowWrite)
            {
                Program.LogMessage("������������ ������� " + dv.Table.TableName + " ������.");
                mWriter.WriteStartElement("Table");
                mWriter.WriteAttributeString("name", dv.Table.TableName);

                // �������
                mWriter.WriteStartElement("Columns");
                foreach (DataColumn column in dv.Table.Columns)
                {
                    mWriter.WriteStartElement("Column");
                    mWriter.WriteAttributeString("name", column.ColumnName);
                    mWriter.WriteAttributeString("userName", column.Caption);
                    mWriter.WriteAttributeString("type", column.DataType.ToString());
                    mWriter.WriteEndElement();
                }
                mWriter.WriteEndElement();

                mWriter.WriteStartElement("Data");
                for (int i = 0; i < dv.Count; i++)
                {
                    DataRowView row = dv[i];
                    mWriter.WriteStartElement("Row");
                    // ������ ��������
                    foreach (DataColumn column in dv.Table.Columns)
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
                Program.LogMessage("������������ ������� " + dv.Table.TableName + " ��������.");
            }
            else
                throw new ApplicationException(
                    "���������� ���������� �������������, ��� ��� ��� ���� ����������� ������!");
        }
    }
}