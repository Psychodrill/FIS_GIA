using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Esrp.Core
{
    public partial class AskedQuestion
    {
        public static AskedQuestion GetAskedQuestion(long id, bool isViewCount)
        {
            PressContext.BeginLock();
            try
            {
                return PressContext.Instance().GetAskedQuestion(id, isViewCount).Single<AskedQuestion>();
            }
            finally
            {
                PressContext.EndLock();
            }
        }

        /// <summary>
        /// Удалить указанный список вопросов
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteAskedQuestions(long[] ids)
        {
            PressContext.BeginLock();
            try
            {
                PressContext.Instance().DeleteAskedQuestion(IdsToString(ids), ClientLogin, ClientIp);
            }
            finally
            {
                PressContext.EndLock();
            }
        }

        /// <summary>
        /// Опубликовать указанный список вопросов
        /// </summary>
        /// <param name="ids">массив идентификаторов вопросов</param>
        public static void ActivateAskedQuestions(long[] ids)
        {
            PressContext.BeginLock();
            try
            {
                PressContext.Instance().SetActiveAskedQuestion(IdsToString(ids), true, ClientLogin, ClientIp);
            }
            finally
            {
                PressContext.EndLock();
            }
        }

        /// <summary>
        /// Снять с публикации указанный список вопросов
        /// </summary>
        /// <param name="ids">массив идентификаторов вопросов</param>
        public static void DeactivateAskedQuestions(long[] ids)
        {
            PressContext.BeginLock();
            try
            {
                PressContext.Instance().SetActiveAskedQuestion(IdsToString(ids), false, ClientLogin, ClientIp);
            }
            finally
            {
                PressContext.EndLock();
            }
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
            PressContext.BeginLock();
            try
            {
                PressContext.Instance().InternalUpdateAskedQuestion(this);
            }
            finally
            {
                PressContext.EndLock();
            }
        }
    }

    partial class PressContext
    {
        internal void InternalUpdateAskedQuestion(AskedQuestion question)
        {
            UpdateAskedQuestion(question);
        }
    }
}
