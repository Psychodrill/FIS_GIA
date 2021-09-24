namespace Ege.Check.Logic.Services.Participant
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ICacheUpdaterService
    {
        [NotNull]
        Task UpdateBlankCompositionPageCount();
    }
}
