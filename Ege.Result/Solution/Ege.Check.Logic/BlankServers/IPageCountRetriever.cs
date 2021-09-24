namespace Ege.Check.Logic.BlankServers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IPageCountRetriever
    {
        [NotNull]
        Task<ICollection<PageCountData>> GetPageCountData([NotNull] string serverUrl, DateTime examDate, int subjectCode);
    }
}
