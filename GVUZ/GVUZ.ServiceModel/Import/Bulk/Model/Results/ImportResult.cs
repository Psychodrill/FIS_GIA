using System.Collections.Generic;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Results
{
    /// <summary>
    ///     Результат импорта
    /// </summary>
    public class ImportResult : IEmptyResult
    {
        /// <summary>
        ///     Не добавленные
        /// </summary>
        public List<ImportResultItem> Failed = new List<ImportResultItem>();

        /// <summary>
        ///     Успешно добавленные
        /// </summary>
        public List<ImportResultItem> Successful = new List<ImportResultItem>();

        public bool IsEmpty
        {
            get { return Successful.Count == 0 && Failed.Count == 0; }
        }
    }

    public class ImportResultItem
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("UID")]
        public string UID { get; set; }

        [XmlAttribute("Text")]
        public string Text { get; set; }
    }
}