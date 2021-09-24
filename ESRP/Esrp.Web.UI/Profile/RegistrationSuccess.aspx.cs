namespace Esrp.Web.Personal.Profile
{
    using System;
    using System.Web.UI;
    using System.Web;

    /// <summary>
    /// Страница успешной регистрации
    /// </summary>
    public partial class RegistrationSuccess : Page
    {
        #region Ovveriding Members

        /// <summary>
        /// Возбуждает события "Инициализации страницы"
        /// </summary>
        /// <param name="e">Параметры события</param>
        protected override void OnInit(EventArgs e)
        {
            var result = Utility.GetUsersAndPasswords();
           
        }

        #endregion
    }
}