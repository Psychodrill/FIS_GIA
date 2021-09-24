using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Data;
using Fbs.Core.Shared;
using Fbs.Utility;

namespace Fbs.Core.CNEChecks
{
    public class CNEInfo
    {
        #region Constructors
        public CNEInfo()
        {
            Marks = new MarkList();
        }

        #endregion

        #region Fields

        private int year;

        #endregion

        #region Properties

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public string CertificateNumber { get; set; }
        public string TypographicNumber { get; set; }
        public string Year
        {
            get
            {
                if (year == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return year.ToString();
                }
            }
            set
            {
                if (value.Trim().Length == 0)
                {
                    year = 0;
                }
                else
                {
                    year = int.Parse(value.Trim());
                }
            }
        }

        public int GlobalStatusID { get; set; }
        public string Status { get; set; }
        public MarkList Marks { get; set; }

        // Уникальные проверки
        public int UniqueChecks { get; set; }
        public int UniqueIHEaFCheck { get; set; }
        public int UniqueIHECheck { get; set; }
        public int UniqueIHEFCheck { get; set; }
        public int UniqueTSSaFCheck { get; set; }
        public int UniqueTSSCheck { get; set; }
        public int UniqueTSSFCheck { get; set; }
        public int UniqueRCOICheck { get; set; }
        public int UniqueOUOCheck { get; set; }
        public int UniqueFounderCheck { get; set; }
        public int UniqueOtherCheck { get; set; }

        //Аннулированный сертификат
        public int CertificateDeny { get; set; }
        public string CertificateNewNumber { get; set; }
        public string CertificateDenyComment { get; set; }

        #endregion

        #region Methods

        private int ParseIntDefaultZero(string str)
        {
            int tryParse;
            if (!int.TryParse(str, out tryParse))
            {
                return 0;
            }

            return tryParse;
        }

        public string GetXML()
        {
            XmlDocument xml = new XmlDocument();

            XmlNode CertificateNode = xml.CreateElement("certificate");
            CertificateNode.InnerXml =
                String.Format(
                "<lastName>{0}</lastName>" +
                "<firstName>{1}</firstName>" +
                "<patronymicName>{2}</patronymicName>" +
                "<passportSeria>{3}</passportSeria>" +
                "<passportNumber>{4}</passportNumber>" +
                "<certificateNumber>{5}</certificateNumber>" +
                "<typographicNumber>{6}</typographicNumber>" +
                "<year>{7}</year>" +
                "<status>{8}</status>" +
                "<uniqueIHEaFCheck>{9}</uniqueIHEaFCheck>" +
                "<certificateDeny>{10}</certificateDeny>" +
                "{11}" +
                "<marks></marks>",
                NameToLower(this.LastName),
                NameToLower(this.FirstName),
                NameToLower(this.PatronymicName),
                this.PassportSeria,
                this.PassportNumber,
                this.CertificateNumber,
                this.TypographicNumber,
                this.Year,
                this.Status,
                this.UniqueIHEaFCheck,
                this.CertificateDeny,
                this.CertificateDeny == 1
                    ? string.Format(@"<certificateNewNumber>{0}</certificateNewNumber>
                                    <certificateDenyComment>{1}</certificateDenyComment>"
                                        , this.CertificateNewNumber
                                        , this.CertificateDenyComment)
                    : string.Empty);
            XmlNode MarksNode = CertificateNode.SelectSingleNode("marks");
            foreach (MarkItem markItem in this.Marks)
            {
                XmlNode MarkNode = MarksNode.AppendChild(xml.CreateElement("mark"));
                MarkNode.AppendChild(xml.CreateElement("subjectName")).InnerText = markItem.SubjectName;
                MarkNode.AppendChild(xml.CreateElement("subjectMark")).InnerText = markItem.SubjectMark;
                MarkNode.AppendChild(xml.CreateElement("subjectAppeal")).InnerText = markItem.SubjectAppeal;
            }

            return CertificateNode.InnerXml;
        }

        private string NameToLower(string text)
        {
            return string.IsNullOrEmpty(text)
                    ? string.Empty
                    : text.Trim().Replace(text.Trim(),
                            text.Trim().Substring(0, 1).ToUpper() +
                            text.Trim().Substring(1, text.Trim().Length - 1).ToLower());
        }

        #endregion
    }

    public class MarkItem
    {
        #region Fields

        private double? subjectMark;
        private int? subjectAppeal;

        #endregion

        #region Properties

        public int SubjectId { get; private set; }
        public string SubjectName { get; set; }
        public string SubjectMark
        {
            get
            {
                if (!subjectMark.HasValue)
                {
                    return string.Empty;
                }

                List<string> lst = ((Math.Round(subjectMark.Value, 1)).ToString()).Replace(".", ",").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return (lst[0]) + "," + (lst.Count > 1 ? lst[1] : "0");
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    subjectMark = null;
                    return;
                }

                string stringMark =
                    value
                    .Trim()
                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace("!", string.Empty);

                subjectMark = Convert.ToDouble(stringMark);
            }
        }
        public string SubjectAppeal
        {
            get
            {
                if (!subjectAppeal.HasValue)
                {
                    return "0";
                }

                return subjectAppeal.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    subjectAppeal = null;
                    return;
                }
                subjectAppeal = Convert.IsDBNull(value) || value == Boolean.FalseString || value == "0" ? 0 : 1;
            }
        }


        #endregion

        public MarkItem()
        {
            
        }

        public MarkItem(int subjectId)
        {
            this.SubjectId = subjectId;
        }
    }

    public class MarkList : List<MarkItem>
    {
        public void Add(string subjectName, string subjectMark, string subjectAppeal)
        {
            int subjectId = -1;
            switch (subjectName)
            {
                case "Русский язык":
                    subjectId = 1;
                    break;
                case "Математика":
                    subjectId = 2;
                    break;
                case "Физика":
                    subjectId = 3;
                    break;
                case "Химия":
                    subjectId = 4;
                    break;
                case "Биология":
                    subjectId = 5;
                    break;
                case "История":
                    subjectId = 6;
                    break;
                case "География":
                    subjectId = 7;
                    break;
                case "Английский язык":
                    subjectId = 8;
                    break;
                case "Немецкий язык":
                    subjectId = 9;
                    break;
                case "Французский язык":
                    subjectId = 10;
                    break;
                case "Обществознание":
                    subjectId = 11;
                    break;
                case "Литература":
                    subjectId = 12;
                    break;
                case "Испанский язык":
                    subjectId = 13;
                    break;
                case "Информатика и ИКТ":
                    subjectId = 14;
                    break;
            }

            MarkItem item = new MarkItem(subjectId)
                { SubjectName = subjectName, SubjectMark = subjectMark, SubjectAppeal = subjectAppeal };
            
            this.Add(item);
        }

        public string FormMarksString()
        {
            string result = string.Empty;
            foreach (var mark in this)
            {
                if (!string.IsNullOrEmpty(mark.SubjectMark))
                {
                    result += string.Format(
                        "{0}={1},", mark.SubjectId, mark.SubjectMark.Replace(',', '.').TrimStart('!'));
                }
            }

            return result;
        }

        public static MarkList FromMarksString(string marks)
        {
            MarkList ret =  new MarkList();
            foreach (string subjectMarks in marks.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                
                string[] mark = subjectMarks.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                Subject s = Subject.GetSubjectById(int.Parse(mark[0]));
                ret.Add(new MarkItem{SubjectMark=mark[1],SubjectName=s.Name});
                

            }
            return ret;
        }
    }

    public static class Extensions
    {
        public static List<CNEInfo> ToCertificatesCollection(this DataTable table, bool assumeName)
        {
            var certificates = new List<CNEInfo>();
            if (table.Rows.Count == 0)
            {
                return certificates;
            }

            var items = (from c in table.AsEnumerable()
                         select new
                         {
                             ParticipantId = Convert.ToString(c["ParticipantId"]),
                             LastName = Convert.ToString(c["Surname"]),
                             FirstName = Convert.ToString(c["Name"]),
                             PatronymicName = Convert.ToString(c["SecondName"]),
                             PassportSeria = Convert.ToString(c["DocumentSeries"]),
                             PassportNumber = Convert.ToString(c["DocumentNumber"]),
                             CertificateId = Convert.ToString(c["CertificateID"]),
                             CertificateNumber = Convert.ToString(c["LicenseNumber"]),
                             TypographicNumber = Convert.ToString(c["TypographicNumber"]),
                             Year = Convert.ToString(c["UseYear"]),
                             CertificateDeny = Convert.ToInt32(c["Cancelled"]),
                             GlobalStatusID = Convert.ToInt32(c["GlobalStatusID"]),
                             Status = Convert.ToString(c["StatusName"]),
                             CertificateNewNumber = Convert.ToString(c["DenyNewCertificateNumber"]),
                             CertificateDenyComment = Convert.ToString(c["DenyComment"]),
                             UniqueIHEaFCheck = Convert.ToInt32(c["UniqueIHEaFCheck"]),
                             SubjectName = Convert.ToString(c["SubjectName"]),
                             Mark = Convert.ToString(c["Mark"]),
                             HasAppeal = Convert.ToString(c["HasAppeal"])
                         });

              var groups = items.GroupBy(c => new
                           {
                               c.ParticipantId,
                               c.LastName,
                               c.FirstName,
                               c.PatronymicName,
                               c.PassportSeria,
                               c.PassportNumber,
                               c.CertificateId,
                               c.CertificateNumber,
                               c.TypographicNumber,
                               c.Year,
                               c.CertificateDeny,
                               c.GlobalStatusID,
                               c.Status,
                               c.CertificateNewNumber,
                               c.CertificateDenyComment,
                               c.UniqueIHEaFCheck
                           }).Select(c => new { c.Key, Items = c }).ToList();

            foreach (var c in groups)
            {
                var info = new CNEInfo()
                {
                    LastName = c.Key.LastName,
                    FirstName = c.Key.FirstName,
                    PatronymicName = c.Key.PatronymicName,
                    PassportSeria = c.Key.PassportSeria,
                    PassportNumber = c.Key.PassportNumber,
                    CertificateNumber = c.Key.CertificateNumber,
                    TypographicNumber = c.Key.TypographicNumber,
                    Year = c.Key.Year,
                    CertificateDeny = c.Key.CertificateDeny,
                    Status = c.Key.Status,
                    CertificateNewNumber = c.Key.CertificateNewNumber,
                    CertificateDenyComment = c.Key.CertificateDenyComment,
                    UniqueIHEaFCheck = c.Key.UniqueIHEaFCheck                        
                };

                foreach (var m in c.Items)
                {
                    info.Marks.Add(m.SubjectName, m.Mark, m.HasAppeal);
                }

                certificates.Add(info);
            }

            return certificates;
        }
    }
}
