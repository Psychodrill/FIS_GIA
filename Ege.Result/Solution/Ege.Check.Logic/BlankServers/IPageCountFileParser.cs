namespace Ege.Check.Logic.BlankServers
{
    using System.Collections.Generic;
    using System.IO;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IPageCountFileParser
    {
        ICollection<PageCountData> GetPageCountData([NotNull]Stream file);
    }
}
