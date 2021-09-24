using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Fbs.Core;

namespace Fbs.Web.Controls
{
    public partial class IntrantProfileView : System.Web.UI.UserControl
    {
        IntrantAccount mCurrentUser;

        public IntrantAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = IntrantAccount.GetIntrantAccount(Account.ClientLogin);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Account.ClientLogin));

                return mCurrentUser;
            }
        }
    }
}