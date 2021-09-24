namespace Ege.Check.App.Web
{
    using System.Web.Routing;
    using JetBrains.Annotations;

    public static class RouteConfig
    {
        public static void RegisterRoutes([NotNull]RouteCollection routes)
        {
            routes.MapPageRoute("Start", "", "~/Content/Build/html/start.html");
            routes.MapPageRoute("Exams", "exams", "~/Content/Build/html/exams.html");
            routes.MapPageRoute("Exam", "exams/{id}", "~/Content/Build/html/exam.html");
            routes.MapPageRoute("Appeal", "appeal/{id}", "~/Content/Build/html/appeal.html");
        }
    }
}
