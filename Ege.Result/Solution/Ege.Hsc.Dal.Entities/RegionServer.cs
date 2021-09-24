namespace Ege.Hsc.Dal.Entities
{
    /// <summary>
    ///     Серверы бланков в регионах
    /// </summary>
    public class RegionServer
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int RegionId { get; set; }
    }
}