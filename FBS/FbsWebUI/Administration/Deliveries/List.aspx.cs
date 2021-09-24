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
using System.Xml.Linq;

namespace Fbs.Web.Administration.Deliveries
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
            foreach (DataGridItem item in DGDeliveriesList.Items)
            {
                CheckBox cb = item.FindControl("CBDeliveries") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField hf = item.FindControl("HFDeliveriesId") as HiddenField;
                    if (hf != null)
                        result.Add(Convert.ToInt64(hf.Value));
                }
            }
            return (long[])result.ToArray(typeof(long));
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {
            Fbs.Core.Deliveries.DeliveryDataAccessor.Delete(GetSelected());
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(CurrentUrl, true);
        }
    }
}
