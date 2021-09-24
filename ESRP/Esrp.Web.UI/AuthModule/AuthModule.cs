namespace Esrp.Web.AuthModule
{
    using System;
    using System.Web;

    using Esrp.Core.Systems;

    /// <summary>
    /// модуль аутентификации по страницам
    /// </summary>
    public class AuthModule : IHttpModule
    {

        /// <summary>
        /// Адрес страницы с информацией о запрещенном доступе.
        /// </summary>
        public const string FORBIDDEN_PAGE_URL = "~/HaveNoAccessPage.aspx";
        

        #region IHttpModule Members

        void IHttpModule.Dispose() { }

        void IHttpModule.Init(HttpApplication context)
        {
            // Добавляю обработчик для события "начало запроса"
            context.AuthorizeRequest += this.AuthRequest;
        }

        #endregion

        #region "Event Handlers / Events"

        /// <summary>
        /// Обработчик начала запроса
        /// </summary>
        /// <param name="sender">HttpApplication</param>
        /// <param name="e">EventArgs</param>
        private void AuthRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;

            if (application == null)
            {
                throw new Exception("application is null");
            }

            var path = application.Context.Request.Path.ToLower();

            // в веб конфиге стоит разрешение всем пользователям видеть секцию "Administration/Organizations". Поэтому нужно тут ее отдельно обработать
            if (path.Contains("Administration/Organizations") && !application.Request.IsAuthenticated)
            {
                application.Context.Response.Redirect(FORBIDDEN_PAGE_URL);
            }

            if (!application.Request.IsAuthenticated)
            {
                return;
            }

            // права учередителя и головной ОУ
            bool canViewSubordinate = GeneralSystemManager.CanViewSubordinateOrganizations(application.Context.User.Identity.Name);
            bool canViewAdminSection = application.Context.User.IsInRole("ViewAdministrationSection");
            if ((path.Contains("administration/organizations/userdepartments")
                || path.Contains("administration/organizations/organizationhistory"))
                && !canViewSubordinate
                && !canViewAdminSection)
            {
                application.Context.Response.Redirect(FORBIDDEN_PAGE_URL);
            }

            // запрет видеть секцию administration/organizations/administrators всем кроме админа. Не включая страницу orgcard_edit.aspx
            if (path.Contains("administration/organizations/administrators") && !path.Contains("orgcard_edit.aspx")
                && !canViewAdminSection)
            {
                application.Context.Response.Redirect(FORBIDDEN_PAGE_URL);
            }

            // права на редактирование организаций
            if (path.Contains("administration/organizations/administrators/orgcard_edit.aspx")
                && !application.Context.User.IsInRole("EditSelfOrganization")
                && !canViewAdminSection)
            {
                application.Context.Response.Redirect(FORBIDDEN_PAGE_URL);
            }
        }

        #endregion
    }
}