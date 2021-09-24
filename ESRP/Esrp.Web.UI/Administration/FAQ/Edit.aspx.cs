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
    public partial class Edit : BasePage
    {
        private const string AskedQuestionQueryKey = "id";
        private const string ErrorAskedQuestionNotFound = "Вопрос не найден";
        private const string SuccessUri = "/Administration/FAQ/Edit.aspx?id={0}";

        private long AskedQuestionId
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[AskedQuestionQueryKey]))
                    throw new NullReferenceException(ErrorAskedQuestionNotFound);

                long result;
                if (!long.TryParse(Request.QueryString[AskedQuestionQueryKey], out result))
                    throw new NullReferenceException(ErrorAskedQuestionNotFound);

                return result;
            }
        }

        private Esrp.Core.AskedQuestion mCurrentAskedQuestion;
        public Esrp.Core.AskedQuestion CurrentAskedQuestion
        {
            get
            {
                if (mCurrentAskedQuestion == null)
                {
                    if ((mCurrentAskedQuestion = Esrp.Core.AskedQuestion.GetAskedQuestion(AskedQuestionId, false)) == null)
                        throw new NullReferenceException(ErrorAskedQuestionNotFound);
                    if (Page.IsPostBack)
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Создам новый документ
            CurrentAskedQuestion.Update();

            // Выполню действия после успешного создания документа
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(String.Format(SuccessUri, CurrentAskedQuestion.Id));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            // Заполню соответствующие контролы
            txtName.Text = CurrentAskedQuestion.Name;
            txtQuestion.Text = CurrentAskedQuestion.Question;
            txtAnswer.Text = CurrentAskedQuestion.Answer;
            ddlIsActive.SelectedValue = CurrentAskedQuestion.IsActive.ToString();
            cblContext.SelectedValues = CurrentAskedQuestion.ContextCodes;

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование вопроса “{0}”", CurrentAskedQuestion.Name);
        }
    }
}