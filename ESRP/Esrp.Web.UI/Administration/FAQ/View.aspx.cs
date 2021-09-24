using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Esrp.Core;

namespace Esrp.Web.Administration.FAQ
{
    public partial class View : BasePage
    {
        private const string AskedQuestionQueryKey = "id";
        private const string ErrorAskedQuestionNotFound = "Вопрос не найден";

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
                }
                return mCurrentAskedQuestion;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            cblContext.SelectedValues = CurrentAskedQuestion.ContextCodes;

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Просмотр вопроса “{0}”", CurrentAskedQuestion.Name);
        }
    }
}
