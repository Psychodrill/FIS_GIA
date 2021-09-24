using FogSoft.Import;

namespace GVUZ.ServiceModel.Import.NormativeDictionaries
{
    [DestinationTable("#CountryType", PostProcessSql = "EXEC CountryType_Transfer", Timeout = 60)]
    [SourceField(0, "Name", "name11", Length = 255)]
    [SourceField(1, "DigitCode", "kod", Length = 3, Optional = true)]
    [SourceField(2, "Alfa2Code", "alfa2", Length = 2, Optional = true)]
    [SourceField(3, "Alfa3Code", "alfa3", Length = 3, Optional = true)]
    [SourceField(4, "Status", "status", Type = typeof (decimal), Culture = "en-US")]
    public class Country : Record
    {
    }
}