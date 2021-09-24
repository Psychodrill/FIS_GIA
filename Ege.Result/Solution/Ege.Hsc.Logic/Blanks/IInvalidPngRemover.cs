namespace Ege.Hsc.Logic.Blanks
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IInvalidPngRemover
    {
        [NotNull]
        Task RemoveInvalidPngsFromStorage(bool remove = true);
    }
}

