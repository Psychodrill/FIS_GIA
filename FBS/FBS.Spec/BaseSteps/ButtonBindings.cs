namespace FBS.Spec.BaseSteps
{
    using NUnit.Framework;

    using TechTalk.SpecFlow;

    using WatiN.Core;

    /// <summary>
    /// Описание шагов, связанных с кнопками
    /// </summary>
    [Binding]
    public class ButtonBindings
    {
        /// <summary>
        /// Нажатие на кнопку в браузере
        /// </summary>
        /// <param name="browser">Браузер</param>
        /// <param name="buttonName">Название кнопки</param>
        public static void ClickButton(Browser browser, string buttonName)
        {
            var button = browser.Button(Find.ByValue(buttonName));
            if (!button.Exists)
            {
                button = browser.Button(Find.ByText(buttonName));
            }

            if (button.Exists)
            {
                WebBrowser.StartJQueryAjaxMonitoring(browser);
                button.Click();
                WebBrowser.WaitForJQueryAjaxComplete(browser);
            }
            else
            {
                LinkBindings.ClickLink(buttonName);
            }

            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Проверяет существование кнопки в браузере
        /// </summary>
        /// <param name="browser">Браузер</param>
        /// <param name="buttonName">Название кнопки</param>
        /// <returns>Существует ли кнопка</returns>
        public static bool ButtonExist(Browser browser, string buttonName)
        {
            var button = browser.Button(Find.ByValue(buttonName));
            if (!button.Exists)
            {
                button = browser.Button(Find.ByText(buttonName));
            }

            return button.Exists;
        }

        /// <summary>
        /// Нажатие на кнопку
        /// </summary>
        /// <param name="buttonName">Название кнопки</param>
        [When("нажимаю кнопку \"(.*)\"")]
        [Given("нажимаю кнопку \"(.*)\"")]
        public void ClickButton(string buttonName)
        {
            ClickButton(WebBrowser.Current, buttonName);
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Проверяет, что кнопка активна
        /// </summary>
        /// <param name="buttonName">Название кнопки</param>
        [Then(@"кнопка ""(.*)"" активна")]
        public void ButtonIsEnabled(string buttonName)
        {
            var button = WebBrowser.Current.Button(Find.ByValue(buttonName));
            if (button.Exists)
            {
                Assert.IsFalse(this.IsDisabled(button), "Кнопка не активна!");
            }

            var link = WebBrowser.Current.Link(Find.ByText(buttonName));
            Assert.IsFalse(this.IsDisabled(link), "Кнопка не активна!");
        }

        /// <summary>
        /// Проверяет, что кнопка неактивна
        /// </summary>
        /// <param name="buttonName">Название кнопки</param>
        [Given(@"кнопка ""(.*)"" неактивна")]
        [Then(@"кнопка ""(.*)"" неактивна")]
        public void ButtonIsDisabled(string buttonName)
        {
            var button = WebBrowser.Current.Button(Find.ByValue(buttonName));
            if (button.Exists)
            {
                Assert.IsTrue(this.IsDisabled(button), "Кнопка активна!");
            }

            var link = WebBrowser.Current.Link(Find.ByText(buttonName));
            Assert.IsTrue(this.IsDisabled(link), "Кнопка активна!");
        }

        private bool IsDisabled(Element element)
        {
            return !element.Enabled | element.ClassName.Contains("disabled");
        }
    }
}