namespace Ege.Check.Logic.Helpers
{
    public class UrlCorrector : IUrlCorrector
    {
        public string Correct(string url)
        {
            if (url == null)
            {
                return null;
            }
            return url.StartsWith("http://") || url.StartsWith("https://")
                       ? url
                       : "http://" + url;
        }
    }
}