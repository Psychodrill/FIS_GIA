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

namespace Fbs.Web
{
    public partial class AskedQuestion : System.Web.UI.Page
    {
        private const string AskedQuestionQueryKey = "id";
        private const string ErrorAskedQuestionNotFound = "Вопрос не найден";
        private const string JsonFormat = "{{question:'{0}', answer:'{1}'}}";

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

        private Fbs.Core.AskedQuestion mCurrentAskedQuestion;
        public Fbs.Core.AskedQuestion CurrentAskedQuestion
        {
            get
            {
                if (mCurrentAskedQuestion == null)
                {
                    mCurrentAskedQuestion = Fbs.Core.AskedQuestion.GetAskedQuestion(AskedQuestionId, true);
                    if (mCurrentAskedQuestion == null || !mCurrentAskedQuestion.IsActive)
                        throw new NullReferenceException(ErrorAskedQuestionNotFound);
                }
                return mCurrentAskedQuestion;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Установлю заголовок страницы
            //this.Title = CurrentAskedQuestion.Name; //Вылетает тут, пропускаем
            
            // AJAX запрс выполняется с параметром textonly, 
            // в этом случеа формирую json и завершаю ответ
            if (Request.RawUrl.ToLower().EndsWith("textonly")){
                Response.Write(String.Format(JsonFormat, CurrentAskedQuestion.Question,
                    CurrentAskedQuestion.Answer).Replace("\r\n", "<br>"));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.End();
                return;
            }
        }
    }
}
