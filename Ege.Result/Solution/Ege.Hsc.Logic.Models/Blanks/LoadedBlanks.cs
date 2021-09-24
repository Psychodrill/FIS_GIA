namespace Ege.Hsc.Logic.Models.Blanks
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Blanks;
    using JetBrains.Annotations;

    public class LoadedBlanks
    {
        [NotNull]
        public ICollection<LoadedBlank> Blanks { get; set; }
    }

    public class LoadedBlank
    {
        public int ParticipantId { get; set; }
        public int RegionId { get; set; }
        [NotNull]
        public BlankIntermediateModel BlankData { get; set; }
    }
}
