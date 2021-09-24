using System;
using FogSoft.Import;

namespace GVUZ.ServiceModel.Import.NormativeDictionaries
{
    [DestinationTable("#Okato", PostProcessSql = "EXEC Okato_Transfer 0", Timeout = 600)]
    // UniqueColumns = "Code, Name"
    [SourceField(0, "Code", "TER", AggregateWithSeparator = "/", Length = 16)]
    [SourceField(1, "Code", "KOD1", CleanRegex = "000")]
    [SourceField(2, "Code", "KOD2", CleanRegex = "000")]
    [SourceField(3, "Code", "KOD3", CleanRegex = "000")]
    [SourceField(4, "Razdel", "RAZDEL", Length = 3)]
    [SourceField(5, "Name", "NAME1", Length = 1000)]
    [SourceField(6, "Centrum", "CENTRUM", Optional = true, Length = 500)]
    [SourceField(7, "Description", "NOMDESCR", Optional = true)]
    [SourceField(8, "NOMAKT", "NOMAKT", Ignored = true)]
    [SourceField(9, "Status", "STATUS", Type = typeof (byte))]
    [SourceField(10, "ModifiedDate", "DATA_UPD", Type = typeof (DateTime), Culture = "en-US")]
    [SourceField(11, "IDX", "IDX", Ignored = true)]
    public class Okato : Record
    {
        public override bool SkipThisRecord
        {
            get
            {
                // включаем если село и т.п.
                if (SourceValues[3] != "000")
                    return false;
                // пропускаем группирующие элементы
                string name = SourceValues[5];
                if (name.EndsWith("/"))
                    return true;

                // включаем регионы кроме пустого, Москвы и Питера
                string region = SourceValues[0];
                if (region == "00" || region == "45" || region == "40")
                    return true;
                string city = SourceValues[1];
                if (city == "000")
                    return false;

                // исключаем районы и т.п.
                if (city == "400" || city == "200")
                    return true;

                // исключаем внутригородской район, округ города
                string afterCity = SourceValues[2];
                if (afterCity.StartsWith("3"))
                    return true;

                // включаем города
                if (city.StartsWith("4") && afterCity == "000")
                    return false;

                // исключаем районы / округа;
                if (afterCity == "000")
                    return true;
                // не задана последняя часть кода, но это сельсовет.
                if (afterCity.StartsWith("8") || afterCity.StartsWith("9") || afterCity.StartsWith("6"))
                    return true;

                return false;
            }
        }
    }
}