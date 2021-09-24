using FogSoft.Import;

namespace GVUZ.ServiceModel.Import.NormativeDictionaries
{
    [DestinationTable("#ProfessionType", PostProcessSql = "EXEC ProfessionType_Transfer", Timeout = 60)]
    [SourceField(0, "Name", "NAME11", Length = 255)]
    [SourceField(1, "Code", "KOD", Length = 7, Optional = true)]
    [SourceField(2, "Status", "STATUS", Type = typeof (decimal), Culture = "en-US")]
    public class Profession : Record
    {
    }
}