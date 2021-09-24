
namespace Esrp.EIISIntegration.Import
{
    internal class RetryInfo
    {
        public RetryInfo(string eIISId,EIISObject objectToRetry, ErrorMessage errorMessage)
        {
            EIISId = eIISId;
            ObjectToRetry = objectToRetry;
            ErrorMessage = errorMessage;
        }

        public string EIISId { get; set; }
        public EIISObject ObjectToRetry { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
    }
}
