namespace Ege.Hsc.Logic.Models.Servers
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Бланки, которые должны храниться на сервере
    /// </summary>
    public class ServerBlanks
    {
        public int RegionId { get; set; }

        public string Url { get; set; }

        [NotNull]
        public IDictionary<ExamFolder, ISet<string>> Blanks { get; set; }
    }
}
