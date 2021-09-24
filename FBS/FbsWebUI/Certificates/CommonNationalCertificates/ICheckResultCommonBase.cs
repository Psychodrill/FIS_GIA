using System.Collections.Generic;
using System.Data;
using FbsServices;
using Fbs.Web.Helpers;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Интерфейс общей страницы результатов запроса
    /// </summary>
    public interface ICheckResultCommonBase
    {
        /// <summary>
        /// Возвращает SqlDataSource для выбранного типа запроса
        /// </summary>
        SqlDataSource GetQuerySource();

        /// <summary>
        /// Возвращает данные для отображения в справке об отсутствии результатов (данные в форме запроса)
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetNotFoundPrintData();
    }

    public interface IBatchCheck
    {
        CommonCheckType CheckType { get; }
    }

    public class PrintNoteData
    {
        private PrintNoteMarkItem[] _marks;

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public PrintNoteMarkItem[] Marks
        {
            get { return _marks ?? (_marks = new PrintNoteMarkItem[0]); }
            set { _marks = value; }
        }


        public static PrintNoteData Parse(DataView source)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            PrintNoteData result = new PrintNoteData();
            DataRow row = source[0].Row;

            result.LastName = row.IsNull("Surname") ? null : row.Field<string>("Surname");
            result.FirstName = row.IsNull("Name") ? null : row.Field<string>("Name");
            result.MiddleName = row.IsNull("SecondName") ? null : row.Field<string>("SecondName");
            result.DocumentNumber = row.IsNull("DocumentNumber") ? null : row.Field<string>("DocumentNumber");
            result.DocumentSeries = row.IsNull("DocumentSeries") ? null : row.Field<string>("DocumentSeries");
            result.Marks = new PrintNoteMarkItem[source.Count];

            for (int i = 0; i < source.Count; i++)
            {
                result.Marks[i] = PrintNoteMarkItem.Parse(source[i]);
            }

            return result;
        }
    }

    public class PrintNoteMarkItem
    {
        public string SubjectName { get; set; }
        public string Mark { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public string CertificateNumber { get; set; }

        public static PrintNoteMarkItem Parse(DataRowView source)
        {
            PrintNoteMarkItem result = new PrintNoteMarkItem();
            DataRow row = source.Row;
            result.CertificateNumber = row.IsNull("LicenseNumber") ? null : row.Field<string>("LicenseNumber");
            
            if ((!row.IsNull("SubjectCode"))&&(SubjectsHelper.SubjectHasBoolMark( row.Field<int>("SubjectCode"))))
            {
                result.Mark = SubjectsHelper.BoolMarkToText(row.IsNull("Mark") ? 0 : row.Field<int>("Mark"));
            }
            else
            {
                result.Mark = row.IsNull("Mark") ? "0" : row.Field<int>("Mark").ToString();
            }
            result.Year = row.IsNull("UseYear") ? 0 : row.Field<int>("UseYear");
            result.SubjectName = row.IsNull("SubjectName") ? null : row.Field<string>("SubjectName");
            result.Status = row.IsNull("StatusName") ? null : row.Field<string>("StatusName");

            return result;
        }
    }
}