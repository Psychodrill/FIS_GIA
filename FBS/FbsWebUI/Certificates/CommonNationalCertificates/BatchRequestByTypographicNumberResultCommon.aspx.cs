using FbsServices;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByTypographicNumberResultCommon : BasePage, IHistoryNavigator, IBatchCheck
    {
        public string GetPageName()
        {
            return "BatchRequestByTypographicNumberResultCommonDetails.aspx";
        }

        public CommonCheckType CheckType
        {
            get
            {
                return CommonCheckType.TypographicNumber;
            }
        }
    }
}