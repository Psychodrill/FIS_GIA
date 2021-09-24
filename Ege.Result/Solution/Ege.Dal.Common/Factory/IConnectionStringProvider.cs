namespace Ege.Dal.Common.Factory
{
    using JetBrains.Annotations;

    public interface IConnectionStringProvider
    {
        [NotNull]
        string CheckEge();

        [NotNull]
        string Hsc();
    }
}