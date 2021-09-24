namespace Ege.Check.Logic.Models.Cache
{
    using Newtonsoft.Json;

    public class BlankClientModel
    {
        /// <summary>
        ///     Название бланка
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Путь к файлу относительно сервера
        /// </summary>
        public string Url { get; set; }

        [JsonIgnore]
        public string Code { get; set; }
    }
}
