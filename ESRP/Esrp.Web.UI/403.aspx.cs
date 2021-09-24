using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Esrp.Web
{
    public partial class _403 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Server.GetLastError() is HttpException)
            {
                HttpException ex = this.Server.GetLastError() as HttpException;
                if (ex.ErrorCode == 403)
                {
                    this.errorMessage.Text = ex.Message;
                }
            }
        }
    }
}