namespace Ege.Check.Logic.BlankServers
{
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IPageCountDataParser
    {
        PageCountData Parse([NotNull]string line);
    }
}
