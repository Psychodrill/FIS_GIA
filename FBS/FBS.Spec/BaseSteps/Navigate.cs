namespace FBS.Spec.BaseSteps
{
    using System;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    using WatiN.Core;

    using Table = TechTalk.SpecFlow.Table;

    /// <summary>
    /// Шаги, связанные с навигацией
    /// </summary>
    [Binding]
    public class Navigate
    {
        /// <summary>
        /// Переход на страницу с заданным Url
        /// </summary>
        /// <param name="url">Url страницы</param>
        public static void NavigateTo(string url)
        {
            WebBrowser.Current.GoTo(url);
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Открытие страницы
        /// </summary>
        /// <param name="page">Наименование страницы</param>
        [Given(@"Я открываю страницу (.*)")]
        [When(@"Я открываю страницу (.*)")]
        public void WhenINavigateTo(string page)
        {
            NavigateTo(PageList.Current[page]);
        }

        /// <summary>
        /// Открытие сайта
        /// </summary>        
        [Given(@"Я открываю сайт")]
        public void WhenINavigateToSite()
        {
            WebBrowser.Current.GoTo(PageList.RootUrl);
        }

        /// <summary>
        /// Проверка, что текущая страница соответствует заданной
        /// </summary>
        /// <param name="page">Заданная страница</param>
        [Then(@"нахожусь на странице (.*)")]
        public void НахожусьНаСтранице(string page)
        {
            System.Threading.Thread.Sleep(2000);
            var currentPage = PageList.Current[page];
            Assert.IsTrue(
                WebBrowser.Current.Url.StartsWith(PageList.Current[page], StringComparison.InvariantCultureIgnoreCase),
                string.Format("Текущая страница '{0}' не соответствует заданной: {1}.", currentPage, page));
        }

        /// <summary>
        /// Вход на сайт
        /// </summary>
        /// <param name="name">
        /// Имя пользователя
        /// </param>
        [Given(@"Я захожу под пользователем ""(.*)""")]
        public void WhenILoginToSite(string name)
        {
            WebBrowser.Current.GoTo("http://vm-fbs:340/auth/check.aspx?ra=1&sid=2&rp=http://vm-fbs:338/default.aspx");

            var login = WebBrowser.Current.TextField(Find.ById("ctl00_cphContent_txtLogin"));
            login.TypeText(name);

            var passwd = WebBrowser.Current.TextField(Find.ById("ctl00_cphContent_txtPassword"));
            passwd.TypeText("1234");

            var enter = WebBrowser.Current.Button(Find.ById("ctl00_cphContent_btnLogin"));
            enter.Click();
        }

        /// <summary>
        /// Проверка, что на странице есть хлебные крошки
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        [Then(@"на странице есть следующие хлебные крошки:")]
        public void ТоНаСтраницеЕстьСледующиеХлебныеКрошки(Table table)
        {
            System.Threading.Thread.Sleep(1000);
            var container = WebBrowser.Current.ElementWithTag("div", Find.ByClass("breadcrumb"));

            Assert.IsTrue(container.Exists, "На странице не найден контейнер с breadcrumbs");

            foreach (var row in table.Rows)
            {
                var text = row["Text"];
                
                var link = ((IElementContainer)container).Link(Find.ByText(text));
                var span = ((IElementContainer)container).Span(Find.ByText(text));

                if (!link.Exists)
                {
                    Assert.NotNull(span, string.Format("В breadcrumbs не найдена ссылка c текстом '{0}'", text));
                    Assert.IsTrue(span.Exists, string.Format("В breadcrumbs не найдена ссылка c текстом '{0}'", text));
                }
                else
                {
                    Assert.NotNull(link, string.Format("В breadcrumbs не найдена ссылка c текстом '{0}'", text));
                    Assert.IsTrue(link.Exists, string.Format("В breadcrumbs не найдена ссылка c текстом '{0}'", text));
                }
            }
        }
    }
}