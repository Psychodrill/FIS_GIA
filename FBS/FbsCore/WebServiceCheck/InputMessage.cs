using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fbs.Core.WebServiceCheck
{
    public class InputMessage
    {

        #region Constructors

        public InputMessage()
        {
            QueryList = new List<QueryItem>();
        }

        #endregion

        #region Properties

        public List<QueryItem> QueryList { get; private set; } // Список элементов 'query'

        #endregion

        #region Public Methods

        public StreamReader GetPassportCSV()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in this.QueryList)
            {
                txt.WriteLine(string.Format("{0}%{1}%{2}%{3}%{4}",
                    item.QueryLastName.Value,
                    item.QueryFirstName.Value,
                    item.QueryPatronymicName.Value,
                    item.QueryPassportSeria.Value,
                    item.QueryPassportNumber.Value));
            }
            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }

        public StreamReader GetCertificateNumberCSV()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in this.QueryList)
            {
                txt.WriteLine(string.Format("{0}%{1}%{2}%{3}%%%%%%%%%%%%%%",
                    item.QueryCertificateNumber.Value,
                    item.QueryLastName.Value,
                    item.QueryFirstName.Value,
                    item.QueryPatronymicName.Value));
            }
            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }
        public StreamReader GetCertificateNumberCSVNN()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in this.QueryList)
            {
                var str = new StringBuilder();
                str.Append(item.QueryCertificateNumber.Value);
                var subjects = item.Marks.Value.Split(',');
                var dict = new Dictionary<int, string>();
                foreach (var subject in subjects)
                {
                    var mark = subject.Split('=');
                    dict.Add(Convert.ToInt32(mark[0]), mark[1]);
                }
                for (var i = 1; i <= 14; i++)
                {
                    str.Append(string.Format("%{0}", dict.ContainsKey(i) ? dict[i] : string.Empty));
                }
                txt.WriteLine(str.ToString());
            }

            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }

        public StreamReader GetPasspertNumberCSVNN()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in this.QueryList)
            {
                var str = new StringBuilder();
                str.AppendFormat("{0}%{1}", item.QueryPassportSeria.Value, item.QueryPassportNumber.Value);
                var dict = new Dictionary<int, string>();
                if (item.Marks.NodeExists)
                {
                    var subjects = item.Marks.Value.Split(',');
                    foreach (var subject in subjects)
                    {
                        var mark = subject.Split('=');
                        dict.Add(Convert.ToInt32(mark[0]), mark[1]);
                    }
                }

                for (var i = 1; i <= 14; i++)
                {
                    str.Append(string.Format("%{0}", dict.ContainsKey(i) ? dict[i] : string.Empty));
                }
                
                txt.WriteLine(str.ToString());
            }

            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }

        public StreamReader GetTypographicNumberCSV()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter txt = new StreamWriter(stream);

            foreach (QueryItem item in this.QueryList)
            {
                txt.WriteLine(string.Format("{0}%{1}%{2}%{3}",
                    item.QueryTypographicNumber.Value,
                    item.QueryLastName.Value,
                    item.QueryFirstName.Value,
                    item.QueryPatronymicName.Value));
            }
            txt.Flush();

            stream.Position = 0;
            return new StreamReader(stream);
        }

        #endregion
    }
}
