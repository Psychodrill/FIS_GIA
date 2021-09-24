namespace Ege.Check.Logic.Models.Json
{
    using Newtonsoft.Json.Converters;

    public class OnlyDateConverter : IsoDateTimeConverter
    {
        public OnlyDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
