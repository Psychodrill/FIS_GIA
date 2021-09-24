using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Utility;

namespace Fbs.Web.Personal.Profile
{
    public partial class View : BasePage
    {
        // Пути до .ascx файлов контролов просмотра профиля
        private const string UserProfileViewControlPath = "/Controls/UserProfileView.ascx";
        private const string IntrantProfileViewControlPath = "/Controls/IntrantProfileView.ascx";

        private Type mAccountType;

        public Type AccountType
        {
            get
            {
                if (mAccountType == null)
                {
                    mAccountType = Account.GetType(Account.ClientLogin);
                }

                if (this.mAccountType == null)
                {
                    LogManager.Error(string.Format("невозможно определить тип пользователя {0}. Скорее всего у него не проставлены группы в бд", Account.ClientLogin));
                    Response.Redirect(@"~\Error.aspx");
                }

                return mAccountType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Добавление контролов происходит в коде, т.к. при загрузке страницы необходмо 
            // знать статус пользователя ВУЗ/СУЗ, а для этого его (пользователя) нужно получить.
            // В этом случае я получу ошибку, т.к. отработают оба метода получения пользователя - 
            // GetUserAccount и GetIntrantAccount и один из них вернет null. Кроме того, 
            // исключается повторный возов метода Account.GetType().
            LogManager.Info(AccountType.ToString());
            if (AccountType == typeof(UserAccount))
            {
                Form.Controls.Add(Page.LoadControl(UserProfileViewControlPath));
            }
            else
            {
                Form.Controls.Add(Page.LoadControl(IntrantProfileViewControlPath));
            }
        }
    }
}
