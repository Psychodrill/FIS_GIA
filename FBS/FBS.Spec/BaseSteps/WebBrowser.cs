namespace FBS.Spec.BaseSteps
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;

    using NUnit.Framework;

    using SHDocVw;

    using TechTalk.SpecFlow;

    using WatiN.Core;
    using WatiN.Core.Exceptions;

    /// <summary>
    /// Работа с браузером
    /// </summary>
    [Binding]
    public class WebBrowser
    {
        private static HttpStatusCode currentStatusCode;

        /// <summary>
        /// Текущий статус ответа сервера
        /// </summary>
        public static HttpStatusCode CurrentStatusCode
        {
            get
            {
                return currentStatusCode;
            }
        }

        /// <summary>
        /// Текущий объект браузера
        /// </summary>
        public static Browser Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("browser"))
                {
                    const int Timeout = 60;
                    Settings.AttachToBrowserTimeOut = Timeout;
                    Settings.WaitUntilExistsTimeOut = Timeout;
                    Settings.WaitForCompleteTimeOut = Timeout;

                    var ie = new IE();
                    ((InternetExplorer)ie.InternetExplorer).NavigateError += WebBrowserNavigateError;
                    ScenarioContext.Current["browser"] = ie;
                    ie.ClearCookies();
                    ie.ClearCache();
                    ie.BringToFront();
                    Console.WriteLine(ie.hWnd.ToString());
                }

                currentStatusCode = HttpStatusCode.OK;
                return (Browser)ScenarioContext.Current["browser"];
            }
        }

        /// <summary>
        /// Ожидание, пока в окне браузера окончится ajax запрос
        /// </summary>
        /// <param name="browser">Окно браузера</param>
        public static void WaitForJQueryAjaxComplete(Browser browser)
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (!Convert.ToBoolean(browser.Eval("ajaxMonitorForWatiN.asyncInProgress()")))
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Установка маркера для Ajax запроса
        /// </summary>
        /// <param name="browser">Окно браузера</param>
        public static void StartJQueryAjaxMonitoring(Browser browser)
        {
            try
            {
                browser.Eval(
                    "function AjaxMonitorForWatiN() {" + " var count = 0;"
                    + " $(document).ajaxStart(function() { count++; });"
                    + " $(document).ajaxComplete(function() { count--; });"
                    + " this.asyncInProgress = function () { return (count > 0); }; }"
                    + "var ajaxMonitorForWatiN = new AjaxMonitorForWatiN();");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Получение окна браузера (IE) с текущим url
        /// </summary>
        /// <param name="url">Текущий Url</param>
        /// <returns>Окно браузера</returns>
        public static IE GetBrowser(string url)
        {
            try
            {
                var regex = new Regex(url);
                var browser = Browser.AttachTo<IE>(Find.ByUrl(regex));
                browser.AutoClose = true;

                return browser;
            }
            catch (BrowserNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Ожидание открытия окна браузера с заданным Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Окно браузера</returns>
        public static Browser WaitBrowser(string url)
        {
            Browser browser = null;
            int count = 0;
            while (count < 2)
            {
                browser = GetBrowser(url);
                if (browser != null)
                {
                    break;
                }

                Thread.Sleep(1000);
                count++;
            }

            return browser;
        }

        /// <summary>
        /// Закрытие браузера
        /// </summary>
        [BeforeScenario]
        public static void BeforeScenario()
        {
            Close();
        }

        /// <summary>
        /// Закрытие браузера
        /// </summary>
        [AfterScenario]
        public static void Close()
        {
            if (ScenarioContext.Current.ContainsKey("browser"))
            {
                var browser = (IE)Current;
                browser.ClearCookies();
                browser.ClearCache();
                browser.ForceClose();
                ScenarioContext.Current.Remove("browser");
            }
        }

        /// <summary>
        /// Проверка, что открылось новое окно браузера с заданным адресом
        /// </summary>
        /// <param name="url">Адрес страницы</param>
        [Then(@"открывается новое окно с url ""(.*)""")]
        public void ТоОткрываетсяНовоеОкноСUrl(string url)
        {
            Browser browser = WaitBrowser(url);

            Assert.NotNull(browser, string.Format("Не найдено окно с url '{0}'", url));
            browser.Dispose();
        }

        private static void WebBrowserNavigateError(object pdisp, ref object url, ref object frame, ref object statuscode, ref bool cancel)
        {
            currentStatusCode = (HttpStatusCode)statuscode;
        }
    }
}