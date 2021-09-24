namespace GVUZ.Web.ViewModels.ApplicationsList
{
    /// <summary>
    /// Параметры перехода к заявлению в списке
    /// </summary>
    public class ApplicationNavigationParameters
    {
        public static class TabPage
        {
            public const int New = 0;
            public const int Unchecked = 1;
            public const int Revoked = 2;
            public const int Accepted = 3;
        }

        // Закладка, которую нужно показать при загрузке
        public int? Tab { get; set; }

        // Начальная страница, на которой находится заявление, которое требуется выделить в списке
        public int? Page { get; set; }

        // Номер заявления, которое нужно выделить в списке
        public int? ApplicationId { get; set; }
    }
}