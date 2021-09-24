namespace FBS.Spec.BaseSteps
{
    using NUnit.Framework;

    using TechTalk.SpecFlow;

    using WatiN.Core;

    using Table = TechTalk.SpecFlow.Table;

    /// <summary>
    /// Описание шагов, связанных с проверками
    /// </summary>
    [Binding]
    public class Acceptens
    {
        /// <summary>
        /// Проверяет, что на экране присутствуют надписи из таблицы
        /// </summary>
        /// <param name="table">Таблица с надписями</param>
        [Then(@"на экране есть:")]
        public void ThenIShouldSeeTheFollowingDetailsOnTheScreen(Table table)
        {
            System.Threading.Thread.Sleep(1000);
            foreach (var tableRow in table.Rows)
            {
                var value = tableRow["Value"];

                Assert.IsTrue(WebBrowser.Current.ContainsText(value), string.Format("Текста '{0}' на странице не найдено", value));
            }
        }

        /// <summary>
        /// Проверяет, что на экране присутствуют надписи из таблицы
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="table">
        /// Таблица с надписями
        /// </param>
        [Then("на экране есть таблица \"(.*)\" со значениями:")]
        public void ThenIShouldSeeTheFollowingDetailsOnTheTable(string id, Table table)
        {
            System.Threading.Thread.Sleep(1000);

            var otherEvidence =
                WebBrowser.Current.Table(Find.ById(t => t.Contains(id)));

            foreach (var tableRow in table.Rows)
            {
                var value = tableRow["Value"];

                Assert.IsTrue(otherEvidence.Text.Contains(value), string.Format("Текста '{0}' на странице не найдено", value));
            }
        }

        /// <summary>
        /// Проверяет, что на экране не присутствуют надписи из таблицы
        /// </summary>
        /// <param name="table">Таблица с надписями</param>
        [Then(@"на экране нет:")]
        public void ThenIShouldNotSeeTheFollowingDetailsOnTheScreen(Table table)
        {
            System.Threading.Thread.Sleep(1000);
            foreach (var tableRow in table.Rows)
            {
                var value = tableRow["Value"];

                Assert.IsFalse(WebBrowser.Current.ContainsText(value), string.Format("Текст '{0}' найден на странице", value));
            }
        }
    }
}
