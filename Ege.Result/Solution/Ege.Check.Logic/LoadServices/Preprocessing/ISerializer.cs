namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using JetBrains.Annotations;

    public interface ISerializer
    {
        [NotNull]
        Stream Serialize<TDto>([NotNull] TDto[] dtos);
    }
}