using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Fbs.Core
{
    [ServiceContract]
    public class CommonNationalCertificateService
    {
        static string ClientIp = string.Empty;

        [WebGet(UriTemplate = "/CheckCommonaNationalCertificate?key={key}&" +
            "isOriginal={isOriginal}&lastName={lastName}&firstName={firstName}&patronymicName={patronymicName}&" +
            "subjectCodes={subjectCodes}&subjectMarks={subjectMarks}")]
        [OperationContract]
        public CommonNationalCertificateCheckResult CheckCertificateByNumber(
                string key, string certificateNumber, bool isOriginal, string lastName, string firstName,
                string patronymicName, string[] subjectCodes, int[] subjectMarks)
        {
            return CommonNationalCertificateCheckResult.CheckCertificateByNumber(key, certificateNumber,
                    isOriginal, lastName, firstName, patronymicName, subjectCodes, subjectMarks);
        }
    }
}
