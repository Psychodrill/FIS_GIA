using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core
{
    partial class Subject
    {
        private static Subject[] mSubjects = null;

        public static Subject NullSubject = new Subject() { Code = null, Id = 0, Name = null };

        public static Subject[] GetSubjects()
        {
            if (mSubjects == null)
            {
                CommonNationalCertificateContext.BeginLock();
                try
                {
                    mSubjects = CommonNationalCertificateContext.Instance().GetSubject().ToArray();
                }
                finally
                {
                    CommonNationalCertificateContext.EndLock();
                }
            }
            return mSubjects;
        }

        public static Subject GetSubjectByCode(string code)
        {
            foreach (Subject subject in GetSubjects())
                if (subject.Code == code)
                    return subject;
            return NullSubject;
        }

        public static Subject GetSubjectById(int id)
        {
            foreach (Subject subject in GetSubjects())
                if (subject.Id == id)
                    return subject;
            return NullSubject;
        }
    }
}
