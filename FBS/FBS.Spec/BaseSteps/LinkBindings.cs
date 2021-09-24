namespace FBS.Spec.BaseSteps
{
    using NUnit.Framework;

    using TechTalk.SpecFlow;

    using WatiN.Core;

    /// <summary>
    /// Описания шагов, связанных с ссылками
    /// </summary>
    [Binding]
    public class LinkBindings
    {
        /// <summary>
        /// Нажатие на ссылку
        /// </summary>
        /// <param name="linkName">Текст ссылки</param>
        public static void ClickLink(string linkName)
        {
            var link = WebBrowser.Current.Link(Find.ByText(linkName));
            if (!link.Exists)
            {
                link = WebBrowser.Current.Link(Find.ByTitle(linkName));
            }

            link.Click();
        }

        /// <summary>
        /// Проверяет, что ссылка существует
        /// </summary>
        /// <param name="linkName">Текст ссылки</param>
        public static void LinkExist(string linkName)
        {
            var link = WebBrowser.Current.Element(Find.ByText(linkName));
            if (!link.Exists)
            {
                link = WebBrowser.Current.Element(Find.ByTitle(linkName));
            }

            Assert.IsTrue(link.Exists, string.Format("Не найдена ссылка '{0}'", linkName));
        }

        /// <summary>
        /// Проверяет, что ссылка не существует
        /// </summary>
        /// <param name="linkName">Текст ссылки</param>
        public static void LinkNotExist(string linkName)
        {
            var link = WebBrowser.Current.Element(Find.ByText(linkName));
            if (!link.Exists)
            {
                link = WebBrowser.Current.Element(Find.ByTitle(linkName));
            }

            Assert.IsFalse(link.Exists, string.Format("Найдена ссылка '{0}'", linkName));
        }

        /// <summary>
        /// Нажатие на ссылку
        /// </summary>
        /// <param name="linkName">Название ссылки</param>
        [When("я нажимаю на ссылку \"(.*)\"")]
        [Given("я нажимаю на ссылку \"(.*)\"")]
        [Then("я нажимаю на ссылку \"(.*)\"")]
        public void ЕслиЯНажимаюНаСсылку(string linkName)
        {
            ClickLink(linkName);
        }

        /// <summary>
        /// Нажатие на ajax ссылку
        /// </summary>
        /// <param name="linkName">Название ссылки</param>
        [When("я нажимаю на ajax-ссылку \"(.*)\"")]
        [Given("я нажимаю на ajax-ссылку \"(.*)\"")]
        [Then("я нажимаю на ajax-ссылку \"(.*)\"")]
        public void ЕслиЯНажимаюНаAjaxСсылку(string linkName)
        {
            // ждем пока все скрипты на странице загрузятся
            System.Threading.Thread.Sleep(1000);
            ClickLink(linkName);
        }
    }
}