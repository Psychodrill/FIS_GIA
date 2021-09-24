using System.Web.UI;

namespace Fbs.Web.Controls
{
    public class BaseControl : UserControl
    {
        protected bool IsAdmin
        {
            get
            {
                return this.Page.User.IsInRole("EditAdministratorAccount");
            }
        }

        /// <summary>
        /// Gets CurrentUserName.
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.Page.User.Identity.Name;
            }
        }

    }
}