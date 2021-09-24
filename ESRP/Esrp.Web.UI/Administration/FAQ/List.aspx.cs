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
using Esrp.Core;

namespace Esrp.Web.Administration.FAQ
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
            foreach (DataGridItem item in dgAskedQuestionList.Items)
            {
                CheckBox cb = item.FindControl("cbAskedQuestion") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField hf = item.FindControl("hfAskedQuestionId") as HiddenField;
                    if (hf != null)
                        result.Add(Convert.ToInt64(hf.Value));
                }
            }
            return (long[])result.ToArray(typeof(long));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Esrp.Core.AskedQuestion.DeleteAskedQuestions(GetSelected());
            ProcessSuccess();
        }

        protected void btnAcivate_Click(object sender, EventArgs e)
        {
            Esrp.Core.AskedQuestion.ActivateAskedQuestions(GetSelected());
            ProcessSuccess();
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            Esrp.Core.AskedQuestion.DeactivateAskedQuestions(GetSelected());
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(CurrentUrl, true);
        }
    }
}
