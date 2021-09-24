namespace Esrp.Web.ViewModel.Organizations
{
    using System.Data.Linq;

    /// <summary>
    /// Представление для письма
    /// </summary>
    public class Letter
    {
        /// <summary>
        /// Содержимое письма
        /// </summary>
        public Binary Content { get; set; }

        /// <summary>
        /// Тип письма
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Имя письма
        /// </summary>
        public string Name { get; set; }
    }
}
