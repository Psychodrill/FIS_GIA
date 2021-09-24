namespace Ege.Check.App.Web.Path
{
    using System.IO;
    using System.Web.Hosting;
    using Ege.Check.Common.Path;

    public class PathResolver : IPathResolver
    {
        public string Resolve(string path)
        {
            return Path.IsPathRooted(path)
                       ? path
                       : HostingEnvironment.MapPath(path);
        }
    }
}
