using FogSoft.Import;

namespace GVUZ.ServiceModel.Import.NormativeDictionaries
{
    [DestinationTable("#FormOfLaw", PostProcessSql = "EXEC FormOfLaw_Transfer 0", Timeout = 60)]
    [SourceField(0, "Code", "SCHOOLPROPERTYCODE", Type = typeof (int))]
    [SourceField(1, "Name", "SCHOOLPROPERTYNAME", Length = 255)]
    public class FormOfLaw : Record
    {
    }
}