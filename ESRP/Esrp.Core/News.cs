using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Esrp.Core
{
    public partial class News
    {
        private static PressContext mContext = null;
        protected static PressContext Context
        {
            get 
            {
                if (mContext == null)
                {
                    mContext = new PressContext();
                    mContext.ObjectTrackingEnabled = false;
                }
                return mContext; 
            }
        }

        public static News GetNews(long id)
        {
            return Context.GetNews(id).Single<News>();
        }

        /// <summary>
        /// Удалить указанный список документов
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteNews(long[] ids)
        {
            Context.DeleteNews(IdsToString(ids), ClientLogin, ClientIp);
        }

        /// <summary>
        /// Опубликовать указанный список документов
        /// </summary>
        /// <param name="ids">массив идентификаторов документов</param>
        public static void ActivateNews(long[] ids)
        {
            Context.SetActiveNews(IdsToString(ids), true, ClientLogin, ClientIp);
        }

        /// <summary>
        /// Снять с публикации указанный список документов
        /// </summary>
        /// <param name="ids">массив идентификаторов документов</param>
        public static void DeactivateNews(long[] ids)
        {
            Context.SetActiveNews(IdsToString(ids), false, ClientLogin, ClientIp);
        }

        static private string IdsToString(long[] ids)
        {
            string[] result = new string[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                result[i] = ids[i].ToString();
            return String.Join(",", result);
        }

        partial void OnCreated()
        {
            // означиваю поля при создании объекта
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        #region Текущий пользователь

        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        #endregion

        public void Update()
        {
            Context.InternalUpdateNews(this);
        }
    }


    partial class PressContext
    {
        internal void InternalUpdateNews(News news)
        {
            UpdateNews(news);
        }
    }
}
