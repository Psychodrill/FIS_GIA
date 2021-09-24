namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using Ege.Check.Logic.Models.Services;

    public interface IBatchSizeSettingsReader
    {
        int Read(ServiceDto dto);
    }
}
