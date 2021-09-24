namespace Ege.Hsc.Logic.Models.Blanks
{
    public class BlankDownloadResult
    {
        public BlankDownloadResult(string message, bool successfully)
        {
            Message = message;
            Successfully = successfully;
        }

        public BlankDownloadResult(bool successfully)
        {
            Successfully = successfully;
        }

        public string Message { get; set; }

        public bool Successfully { get; set; }
    }
}