namespace Esrp.Web
{
    using System.Web;

    public class BasePage : System.Web.UI.Page
    {
        public string CurrentUrl
        {
            get { return Request.Url.AbsoluteUri; }
        }

        public string CurrentUserName
        {
            get { return User.Identity.Name; }
        }

        public string CurrentUserIp
        {
            get { return Request.UserHostAddress; }
        }

        public string PageTitle
        {
            get
            {
                return SiteMap.CurrentNode.GetActualTitle();
            }

            set
            {
                SiteMap.CurrentNode.StoreProperties(
                    new SiteMapNodeProperties(Request.Url.PathAndQuery, value));
            }
        }

        /// <summary>
        /// получить get параметр типа инт
        /// </summary>
        /// <param name="name">
        /// название параметра
        /// </param>
        /// <returns>
        /// значение параметра
        /// </returns>
        public int GetParamInt(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                int returnVal;
                if (int.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }
    }
}
