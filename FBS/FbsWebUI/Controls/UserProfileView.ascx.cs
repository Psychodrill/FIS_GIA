using System;
using System.Web;
using Fbs.Core;
using Fbs.Utility;

namespace Fbs.Web.Controls
{
    public partial class UserProfileView : System.Web.UI.UserControl
    {
        UserAccount mCurrentUser;

        public UserAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = UserAccount.GetUserAccount(Account.ClientLogin);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Account.ClientLogin));

                return mCurrentUser;
            }
        }

        public System.Security.Principal.IPrincipal User
        {
            get { return HttpContext.Current.User; }
        }

        public bool IsRegistrationDocumentExists
        {
            get
            {
                return CurrentUser.RegistrationDocument.Document != null;
            }
        }

        protected override void  OnPreRender(EventArgs e)
        {
            // Обновляю список ролей пользователя если это необходимо
            LogManager.Info("12");
            if (ShouldRefreshUserRolesLis())
            {
                // Очищаю список ролей текущего пользователя, для того, чтобы этот самый список 
                // обновить, не дожидаясь, пока истечет время жизни кукиса.
                if (Request.Cookies[Config.RoleManagerCookeiName] != null)
                {
                    Request.Cookies[Config.RoleManagerCookeiName].Value = string.Empty;
                }

                if (Response.Cookies[Config.RoleManagerCookeiName] != null)
                {
                    Response.Cookies[Config.RoleManagerCookeiName].Value = string.Empty;
                }

                // Перезагружаю текущую страницу
                Response.Redirect(Request.Url.ToString(), true);
            }
        }

        // Установление необходимости обновления списка ролей пользователя
        private bool ShouldRefreshUserRolesLis()
        {
            // TODO: выделить статичесский класс под роли и перечислить их там
            // Если пользователь активирован и у него нет роли на просмотр секции свидетельств или,
            // наоборот, пользователь неактивен, а роль у него есть,то обновлю список ролей 
            // пользователя
            LogManager.Info("Проверка ролей пользователя. Ожидается что у него появится роль ViewCertificateSection если он активен. Или эта роль исчезнет, если не активен");
            return (
                    //CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated &&
                    //    !User.IsInRole("ViewCertificateSection")) ||
                   (CurrentUser.Status != UserAccount.UserAccountStatusEnum.Activated &&
                        User.IsInRole("ViewCertificateSection")));
        }
    }
}