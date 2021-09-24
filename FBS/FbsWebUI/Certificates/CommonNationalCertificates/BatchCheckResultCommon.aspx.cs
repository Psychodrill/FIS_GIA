using FbsServices;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchCheckResultCommon : BasePage, IHistoryNavigator, IBatchCheck
    {
        public string GetPageName()
        {
            return "BatchCheckResultCommonDetails.aspx";
        }

        public CommonCheckType CheckType
        {
            get
            {
                return CommonCheckType.CertificateNumber;
            }
        }
    }
}