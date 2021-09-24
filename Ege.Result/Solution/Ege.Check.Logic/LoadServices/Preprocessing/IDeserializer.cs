namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using JetBrains.Annotations;

    public interface IDeserializer
    {
        TDto[] Deserialize<TDto>([NotNull] Stream stream);
    }
}