namespace Ege.Hsc.Logic.Configuration
{
    using System;

    public interface IHscSettings
    {
        DateTime OpenDate { get; }
        bool CsvUploadAllowedForEsrp { get; }
    }
}