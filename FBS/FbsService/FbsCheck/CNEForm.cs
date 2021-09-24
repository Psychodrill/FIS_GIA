using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsCheck
{
    partial class CNEForm
    {
        public bool IsDeny = false;
        public bool IsBlank = false;
        public bool IsDuplicate = false;

        static public CNEForm[] GetActiveForms(string regionCode)
        {
            CheckContext.BeginLock();
            try
            {
                return CheckContext.Instance().CheckCommonNationalExamCertificateForm(regionCode).ToArray();
            }
            finally
            {
                CheckContext.EndLock();
            }
        }
    }
}
