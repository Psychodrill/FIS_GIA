using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fbs.Core;
using System.Data;

namespace Fbs.Web.Helpers
{
    public static class DataRowChecksHelper
    {
        public static IList<Subject> ExtractSubjects(this DataRowView row)
        {
            List<Subject> ret = new List<Subject>();
            foreach (Subject s in Subject.GetSubjects())
            {
                if (!Convert.IsDBNull(row[s.Code + "CheckMark"]))
                {
                    ret.Add(s);
                }
            }
            return ret;
        }

        public static Dictionary<Subject, int> ExtractSubjectsWithMarks(this DataRowView row)
        {
            Dictionary<Subject, int> ret = new Dictionary<Subject, int>();
            IList<Subject> subjects = row.ExtractSubjects();
            foreach (Subject s in subjects)
            {
                ret.Add(s, (int)Double.Parse(row[s.Code+"CheckMark"].ToString()));
            }
            return ret;
        }

        public static string ExtractSubjectsWithMarksString(this DataRowView row)
        {
            Dictionary<Subject, int> val = row.ExtractSubjectsWithMarks();
            return string.Join(",", val.Select(x => String.Format("{0}={1}", x.Key.Id, x.Value)).ToArray());
        }
    
    }
}