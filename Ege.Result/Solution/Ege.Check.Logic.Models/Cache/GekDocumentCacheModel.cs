namespace Ege.Check.Logic.Models.Cache
{
    using System;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    /// <summary>
    ///     Документ ГЭК
    ///     Хранится в кэше только в составе коллекции
    /// </summary>
    public class GekDocumentCacheModel
    {
        public string GekNumber { get; set; }

        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime? GekDate { get; set; }

        public string Url { get; set; }
    }
}