namespace Ege.Hsc.Logic.Servers
{
    using System.Collections.Generic;
    using System.IO;
    using JetBrains.Annotations;

    public interface IServerFileParser
    {
        [NotNull]
        ISet<string> GetCodes([NotNull]Stream file);
    }
}
