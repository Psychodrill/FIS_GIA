using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Fbs.Utility; 

namespace Fbs.Web
{
    /// <summary>
    /// "Ручная" обрабтка некоторых ошибок
    /// </summary>
    public static class ManualErrorHandler
    {
        // Ошибка доступа к WebDAV.
        // Возникает при открытии ссылки на *.doc файл из MS Office, т.к. Office пытается проверить 
        // доступность WebDAV на сервере, посылая OPTIONS запрос.
        private static readonly string[] OptionsMessage = new[]
                                                              {
                                                                  "Path 'OPTIONS' is forbidden", 
                                                                  "Проверка подлинности форм"
                                                              };

        /// <summary>
        /// Проверка того, что ошибка должна обрабатываться "руками"
        /// </summary>
        /// <param name="ex">Ошибка</param>
        public static bool CanHandle(Exception ex)
        {
            foreach (string om in OptionsMessage)
                if (ex.Message.Contains(om))
                    return true;
            return false;
        }

        /// <summary>
        /// Обработка ошибки
        /// </summary>
        /// <param name="ex">Ошибка</param>
        public static void Handle(Exception ex)
        {
            LogManager.Error(ex);
        }
    }
}
