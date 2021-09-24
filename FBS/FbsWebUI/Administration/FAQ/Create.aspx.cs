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
using Fbs.Core;

namespace Fbs.Web.Administration.FAQ
{
    public partial class Create : BasePage
    {
        private const string SuccessUri = "/Administration/FAQ/Edit.aspx?id={0}";

        private Fbs.Core.AskedQuestion mCurrentAskedQuestion;
        private Fbs.Core.AskedQuestion CurrentAskedQuestion
        {
            get
            {
                if (mCurrentAskedQuestion == null)
                {
                    mCurrentAskedQuestion = new Fbs.Core.AskedQuestion();
                    DataBindCurrenAskedQuestion();
                }
                return mCurrentAskedQuestion;
            }
        }

        private void DataBindCurrenAskedQuestion()
        {
            mCurrentAskedQuestion.Name = txtName.Text.Trim();
            mCurrentAskedQuestion.Question = txtQuestion.Text.Trim();
            mCurrentAskedQuestion.Answer = txtAnswer.Text.Trim();
            mCurrentAskedQuestion.IsActive = Convert.ToBoolean(ddlIsActive.SelectedValue);
            mCurrentAskedQuestion.ContextCodes = cblContext.SelectedValues;
        }

        private void ProcessSuccess()
        {
            Response.Redirect(String.Format(SuccessUri, CurrentAskedQuestion.Id));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Создам новый документ
            CurrentAskedQuestion.Update();

            // Выполню действия после успешного создания документа
            ProcessSuccess();
        }
    }
}
