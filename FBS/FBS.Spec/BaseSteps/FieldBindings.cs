namespace FBS.Spec.BaseSteps
{
    using System;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    using WatiN.Core;

    using Table = TechTalk.SpecFlow.Table;
    using TableRow = TechTalk.SpecFlow.TableRow;

    /// <summary>
    /// Описание шагов, свзяанных с полями ввода
    /// </summary>
    [Binding]
    public class FieldBindings
    {
        /// <summary>
        /// Устанавливает значения полей
        /// </summary>
        /// <param name="elementContainer">Контейнер элементов, в котором нужно выставить поля</param>
        /// <param name="values">Таблица с значениями полей</param>
        public static void SetFieldValues(IElementContainer elementContainer, Table values)
        {
            foreach (var tableRow in values.Rows)
            {
                var textField = FindTextField(elementContainer, tableRow);

                if (textField != null && textField.Exists)
                {
                    textField.Value = tableRow["Value"];
                    continue;
                }

                var selectList = FindSelectList(elementContainer, tableRow);
                if (selectList != null && selectList.Exists)
                {
                    selectList.Select(tableRow["Value"]);
                    continue;
                }

                throw new Exception(string.Format("Не найден элемент '{0}' на странице '{1}'", tableRow["Name"], WebBrowser.Current.Url));
            }
        }

        /// <summary>
        /// Проверяет значения полей
        /// </summary>
        /// <param name="elementContainer">Контейнер элементов, в котором нужно проверить значения полей</param>
        /// <param name="values">Таблица с значениями полей</param>
        public static void CheckFieldValues(IElementContainer elementContainer, Table values)
        {
            foreach (var tableRow in values.Rows)
            {
                string selectedValue = null;
                var textField = FindTextField(elementContainer, tableRow);
                if (textField != null && textField.Exists)
                {
                    selectedValue = textField.Value ?? string.Empty;
                }
                else
                {
                    var selectList = FindSelectList(elementContainer, tableRow);
                    if (selectList != null && selectList.Exists)
                    {
                        selectedValue = selectList.SelectedItem;
                    }
                }

                if (selectedValue == null)
                {
                    throw new Exception(
                        string.Format(
                            "Не найден элемент '{0}' на странице '{1}'", tableRow["Name"], WebBrowser.Current.Url));
                }

                Assert.AreEqual(
                    tableRow["Value"],
                    selectedValue,
                    string.Format("Значение в поле {0} отличается от ожидаемого", tableRow["Name"]));
            }
        }

        /// <summary>
        /// Устанавливает чекбокс
        /// </summary>
        /// <param name="browser">Экземпляр браузера,</param>
        /// <param name="name">Название чекбокса</param>
        /// <param name="value">Значение</param>
        public static void SetCheckbox(Browser browser, string name, bool value)
        {
            var chname = browser.Label(Find.ByText(name)).For;
            browser.CheckBox(Find.ById(chname)).Checked = value;
        }

        /// <summary>
        /// Устанавливает чекбокс 
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        [Given(@"выбираю чекбокс (.*)")]
        [When(@"выбираю чекбокс (.*)")]
        public void Выбираю(string name)
        {
            SetCheckbox(WebBrowser.Current, name, true);
        }
        
        /// <summary>
        /// Устанавливает значения в полях
        /// </summary>
        /// <param name="values">Таблица значений</param>
        [Given(@"вношу в поля следующие данные:")]
        [When(@"вношу в поля следующие данные:")]
        public void ВношуВПоляСледующиеДанные(Table values)
        {
            SetFieldValues(WebBrowser.Current, values);
        }

        /// <summary>
        /// Проверяет, что в полях утсановлены нужные значения
        /// </summary>
        /// <param name="table">Таблица значений</param>
        [Then(@"вижу в полях следующие данные:")]
        public void ТоВижуВПоляхСледующиеДанные(Table table)
        {
            CheckFieldValues(WebBrowser.Current, table);
        }

        private static TextField FindTextField(IElementContainer elementContainer, TableRow tableRow)
        {
            var field = elementContainer.TextField(Find.ById(t => t.Contains(tableRow["Name"])));
            
            if (field.Exists && field.GetAttributeValue("type").Equals("hidden", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return field;
        }

        private static SelectList FindSelectList(IElementContainer elementContainer, TableRow tableRow)
        {
            SelectList field = elementContainer.SelectList(Find.ById(t => t.Contains(tableRow["Name"])));

            return field;
        }
    }
}