using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Fbs.Web.Administration.News
{
    public partial class List : BasePage
    {
        /// <summary>
        /// Метод получающий идентификаторы помеченных в таблице документов
        /// </summary>
        /// <returns>массив идентификаторов</returns>
        private long[] GetSelected()
        {
            ArrayList result = new ArrayList();
            foreach (DataGridItem item in dgNewsList.Items)
            {
                CheckBox cb = item.FindControl("cbNews") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField hf = item.FindControl("hfNwesId") as HiddenField;
                    if (hf != null)
                        result.Add(Convert.ToInt64(hf.Value));
                }
            }
            return (long[])result.ToArray(typeof(long));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Fbs.Core.News.DeleteNews(GetSelected());
            ProcessSuccess();
        }

        protected void btnAcivate_Click(object sender, EventArgs e)
        {
            Fbs.Core.News.ActivateNews(GetSelected());
            ProcessSuccess();
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            Fbs.Core.News.DeactivateNews(GetSelected());
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(CurrentUrl, true);
        }
    }
}
