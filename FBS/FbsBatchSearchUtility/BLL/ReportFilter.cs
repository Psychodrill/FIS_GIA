namespace FbsBatchSearchUtility.BLL
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// ������ ������ �������� ������� ��� �������
    /// </summary>
    public class ReportFilter
    {
        public const string FULL_ORG_NAME = "������ ������������ �����������";
        public const string SHORT_ORG_NAME = "������� ������������ �����������";
        public const string INN = "���";
        public const string OGRN = "����";
        public const string FOUNDER = "������������ ����������";

        /// <summary>
        /// ������ ������� � ���� �������
        /// </summary>
        public DataTable FilterData { get; set; }

        /// <summary>
        /// ���� ������������� ������� ������������ �����������
        /// </summary>
        public bool UseFullOrgName { get; set; }

        /// <summary>
        /// ���� ������������� �������� ������������ �����������
        /// </summary>
        public bool UseShortOrgName { get; set; }

        /// <summary>
        /// ���� ������������� ���
        /// </summary>
        public bool UseInn { get; set; }

        /// <summary>
        /// ���� ������������� ����
        /// </summary>
        public bool UseOgrn { get; set; }

        /// <summary>
        /// ���� ������������� ����������
        /// </summary>
        public bool UseFounder { get; set; }

        /// <summary>
        /// ��������� ������ ������� �� CSV �����
        /// </summary>
        /// <param name="fileName">
        /// ��� csv �����
        /// </param>
        public bool LoadFilterFromCsvFile(string fileName)
        {
            StreamReader fileReader;

            try
            {
                fileReader = new StreamReader(fileName, Encoding.Default);
            }
            catch (IOException e)
            {
                MessageBox.Show(
                    "���������� ������� ����: ���� ������������.",
                    "������ �������� ��������� �����",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            this.FilterData = new DataTable();

            // Match csv row element
            var rexp = new Regex("(\"\"([^\"\"]*|\"\"{2})*\"\"(;|$))|\"\"[^\"\"]*\"\"(;|$)|[^;]+(;|$)|(;)");

            // Read csv headers
            if (!fileReader.EndOfStream)
            {
                string s = fileReader.ReadLine();

                if (rexp.IsMatch(s))
                {
                    MatchCollection matches = rexp.Matches(s);

                    foreach (Group header in matches)
                    {
                        var column = new DataColumn();
                        column.DataType = typeof(string);
                        column.ColumnName = header.Value.EndsWith(";")
                                                ? header.Value.Substring(0, header.Value.Length - 1)
                                                : header.Value;

                        this.FilterData.Columns.Add(column);
                    }
                }
            }

            // Read csv data
            while (!fileReader.EndOfStream)
            {
                string s = fileReader.ReadLine();
                MatchCollection matches = rexp.Matches(s);

                DataRow dataRow = this.FilterData.NewRow();

                int i = 0;
                foreach (Group element in matches)
                {
                    var data = element.Value.EndsWith(";")
                                   ? element.Value.Substring(0, element.Value.Length - 1)
                                   : element.Value;

                    // remove any quotes, because they affect nothing
                    data = data.Replace("\"", string.Empty).Replace("'", string.Empty);

                    dataRow[i] = data;
                    i++;
                }

                this.FilterData.Rows.Add(dataRow);
            }

            return true;
        }
    }
}