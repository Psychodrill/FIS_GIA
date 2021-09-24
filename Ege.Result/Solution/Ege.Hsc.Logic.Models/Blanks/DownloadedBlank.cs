namespace Ege.Hsc.Logic.Models.Blanks
{
    using System;

    /// <summary>
    /// Информация о бланке на диске, которую можно получить из имени файла
    /// </summary>
    public class DownloadedBlank
    {
        public Guid RbdId { get; set; }
        public string Hash { get; set; }
        public string DocumentNumber { get; set; }
        public int Order { get; set; }
    }
}
