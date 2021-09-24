using FbsServices;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByPassportNewResultCommon : BasePage, IHistoryNavigator, IBatchCheck
    {
        public string GetPageName()
        {
            return "BatchRequestByPassportNewResultCommonDetails.aspx";
        }

        public CommonCheckType CheckType
        {
            get
            {
                return CommonCheckType.DocumentNumber;
            }
        }
    }
}