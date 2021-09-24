using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Esrp.Core;

namespace Esrp.Web.Personal.Profile
{
    public partial class Edit : BasePage
    {
        // Пути до .ascx файлов контролов редактирования профиля
        private const string UserProfileEditControlPath = "/Controls/UserProfileEdit.ascx";
        private const string IntrantProfileEditControlPath = "/Controls/IntrantProfileEdit.ascx";

        private Type mAccountType;

        public Type AccountType
        {
            get
            {
                if (mAccountType == null)
                    mAccountType = Account.GetType(Account.ClientLogin);

                return mAccountType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Добавление контролов происходит в коде, т.к. при загрузке страницы необходмо 
            // отобразить текущие данные пользователя, а для этого его (пользователя) получить.
            // В этом случае я получу ошибку, т.к. отработают оба метода получения пользователя - 
            // GetUserAccount и GetIntrantAccount и один из них вернет null. Кроме того, 
            // исключается повторный возов метода Account.GetType().
            if (AccountType == typeof(UserAccount))
                Form.Controls.Add(Page.LoadControl(UserProfileEditControlPath));
            else
                Form.Controls.Add(Page.LoadControl(IntrantProfileEditControlPath));
        }
    }
}
