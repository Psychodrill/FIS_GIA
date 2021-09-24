namespace Ege.Hsc.Logic.Blanks
{
    using JetBrains.Annotations;

    public interface IBlankApplicationSettings
    {
        int BatchBlankDownload();

        int BatchBlankRequest();

        [NotNull]
        string BlanksRootPath();

        [NotNull]
        string ZipRootPath();
    }
}