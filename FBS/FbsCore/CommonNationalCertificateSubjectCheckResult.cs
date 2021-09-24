using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fbs.Core
{
    [DataContract]
    public class CommonNationalCertificateSubjectCheckResult
    {
        [DataMember]
        public string SubjectCode = null;
        [DataMember]
        public int? CheckMark = 0;
        [DataMember]
        public int? Mark = 0;
        [DataMember]
        public bool MarkIsCorrect = true;
        [DataMember]
        public bool? HasAppeal = false;
    }
}
