using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fbs.Core
{
    [DataContract]
    public class CommonNationalCertificateCheckResult
    {
        [DataMember]
        public string CertificateNumber = null;
        [DataMember]
        public bool? IsOriginal = null;
        [DataMember]
        public bool IsExist = false;
        [DataMember]
        public bool IsDeny = false;
        [DataMember]
        public string DenyComment = null;
        [DataMember]
        public string CheckLastName = null;
        [DataMember]
        public string LastName = null;
        [DataMember]
        public bool LastNameIsCorrect = false;
        [DataMember]
        public string CheckFirstName = null;
        [DataMember]
        public string FirstName = null;
        [DataMember]
        public bool FirstNameIsCorrect = false;
        [DataMember]
        public string CheckPatronymicName = null;
        [DataMember]
        public string PatronymicName = null;
        [DataMember]
        public bool PatronymicNameIsCorrect = false;
        [DataMember]
        public CommonNationalCertificateSubjectCheckResult[] Subjects = null;

        static public CommonNationalCertificateCheckResult CheckCertificateByNumber(
                string key, string certificateNumber, bool isOriginal, string lastName, string firstName,
                string patronymicName, string[] subjectCodes, int[] subjectMarks)
        {
            string login;
            string clientIp = string.Empty;
            AccountContext.BeginLock();
            try
            {
                login = AccountContext.GetAccountLoginByKey(key, clientIp);
                if (login == null)
                    return null;
            }
            finally
            {
                AccountContext.EndLock();
            }

            CommonNationalCertificateContext.BeginLock();
            try
            {
                return CommonNationalCertificateContext.Instance().CheckByNumber(login, clientIp, certificateNumber,
                        isOriginal, lastName, firstName, patronymicName, subjectCodes, subjectMarks);
            }
            finally
            {
                CommonNationalCertificateContext.EndLock();
            }
        }
    }

    partial class CommonNationalCertificateContext
    {
        internal CommonNationalCertificateCheckResult CheckByNumber(string login, string ip,
                string certificateNumber, bool isOriginal, string lastName, string firstName, string patronymicName,
                string[] subjectCodes, int[] subjectMarks)
        {
            StringBuilder marks = new StringBuilder();
            for (int ind = 0; ind < subjectCodes.Length && ind < subjectMarks.Length; ind++)
                marks.Append(string.Format("{0}={1},", Subject.GetSubjectByCode(subjectCodes[ind]).Id,
                        subjectMarks[ind]));
            marks.Remove(marks.Length - 1, 1);
            CheckCommonNationalExamCertificateByNumberResult[] checkResults =
                    this.CheckCommonNationalExamCertificateByNumber(certificateNumber, isOriginal,
                        lastName, firstName, patronymicName, marks.ToString(), login, ip).ToArray();
            List<CommonNationalCertificateSubjectCheckResult> subjectResult =
                    new List<CommonNationalCertificateSubjectCheckResult>();
            foreach (CheckCommonNationalExamCertificateByNumberResult checkResult in checkResults)
                subjectResult.Add(new CommonNationalCertificateSubjectCheckResult()
                {
                    CheckMark = checkResult.CheckSubjectMark,
                    Mark = checkResult.SubjectMark,
                    MarkIsCorrect = checkResult.SubjectMarkIsCorrect,
                    HasAppeal = checkResult.HasAppeal,
                    SubjectCode = Subject.GetSubjectById(checkResult.SubjectId).Code
                });
            return new CommonNationalCertificateCheckResult()
            {
                CertificateNumber = checkResults[0].Number,
                IsOriginal = checkResults[0].IsOriginal,
                IsExist = checkResults[0].IsExist,
                IsDeny = checkResults[0].IsDeny,
                DenyComment = checkResults[0].DenyComment,
                CheckLastName = checkResults[0].CheckLastName,
                LastName = checkResults[0].LastName,
                LastNameIsCorrect = checkResults[0].LastNameIsCorrect,
                CheckFirstName = checkResults[0].CheckFirstName,
                FirstName = checkResults[0].FirstName,
                FirstNameIsCorrect = checkResults[0].FirstNameIsCorrect,
                CheckPatronymicName = checkResults[0].CheckPatronymicName,
                PatronymicName = checkResults[0].PatronymicName,
                PatronymicNameIsCorrect = checkResults[0].PatronymicNameIsCorrect,
                Subjects = subjectResult.ToArray()
            };
        }
    }
}
