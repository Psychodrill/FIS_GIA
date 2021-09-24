using FogSoft.Import;

namespace GVUZ.ServiceModel.Import.NormativeDictionaries
{
    [DestinationTable("#Direction", PostProcessSql = "EXEC Direction_Transfer 0", Timeout = 300)]
    // UniqueColumns = "Code, Name"
    [SourceField(0, "Code", Length = 6)]
    [SourceField(1, "Name", Length = 250)]
    public class Direction : Record
    {
    }
}