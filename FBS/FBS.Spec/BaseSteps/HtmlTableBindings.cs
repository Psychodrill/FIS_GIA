namespace FBS.Spec.BaseSteps
{
    using System.Linq;

    using NUnit.Framework;

    using FBS.Spec.Controls;

    using TechTalk.SpecFlow;

    using Table = TechTalk.SpecFlow.Table;

    /// <summary>
    /// Описание шагов, связанных с Html таблицами
    /// </summary>
    [Binding]
    public class HtmlTableBindings : Steps
    {
        /// <summary>
        /// Выбирает таблицу на странице
        /// </summary>
        /// <param name="gridId">
        /// идентификатор таблицы.
        /// </param>
        [Given(@"Я выбираю таблицу с идентификатором: (.*)")]
        public void ДопустимЯВыбираюТаблицуСИдентификатором(string gridId)
        {
            var tableInfo = new TableInfo(gridId);
            TableInfo.SavedState = tableInfo;
        }

        /// <summary>
        /// Проверяет, что на экране в таблице есть нужное кол-во колонок
        /// </summary>
        /// <param name="colcount">Таблица с надписями</param>
        [Then("в таблице есть колонок: (.*)")]
        public void CheckColCountInHtmlTable(int colcount)
        {
            var table = Grid.GetFirstGrid();
            Assert.AreEqual(colcount, table.Header.ElementsWithTag("th").Count, "В таблице нет нужных колонок.");
        }

        /// <summary>
        /// Проверяет, кол-во записей в таблице
        /// </summary>
        /// <param name="rowCount">Ожидаемое кол-во записей.</param>
        [Then("в таблице есть записей: (.*)")]
        public void CheckHtmlTableRowCount(int rowCount)
        {
            this.Given(@"Я выбираю таблицу с идентификатором: result-list-table");
            var table = Grid.GetFirstGrid();
            var totalСount = table.DataRows.Count() - 1;
            Assert.AreEqual(rowCount, totalСount, "Количество записей в таблице не совпадает с ожидаемым.");
        }

        /// <summary>
        /// Проверяет, что в таблице присутствуют следующие значения
        /// </summary>
        /// <param name="table">Таблица значений </param>
        [Then(@"в таблице есть строки:")]
        public void ВТаблицеЕстьСтроки(Table table)
        {
            var grid = Grid.GetFirstGrid();

            int rowindex = 0;
            var gridRows = grid.DataRows.ToArray();

            foreach (var tableRow in table.Rows)
            {
                var row = gridRows[rowindex];
                for (int i = 0; i < table.Header.Count(); i++)
                {
                    var cell = row.TableCells[i];
                    var cellText = cell.Text;
                    if (cell.TextFields.Count > 0)
                    {
                        cellText = cell.TextFields[0].Value;
                    }

                    if (cellText == null)
                    {
                        cellText = string.Empty;
                    }

                    Assert.AreEqual(tableRow[i], cellText);
                }

                rowindex++;
            }
        }

        /// <summary>
        /// Проверяет, что кол-во строк в таблице увеличилось на
        /// </summary>
        /// <param name="count">На сколько увеличилось.</param>
        [Then(@"количество строк в таблице увеличилось на (.*)")]
        public void ТоКоличествоСтрокВТаблицеУвеличилось(int count)
        {
            this.Given(@"Я выбираю таблицу с идентификатором: result-list-table");
            var old = TableInfo.SavedState.DataRowsCount;
            var current = TableInfo.SavedState.CurrentGrid.DataRows.Count();

            Assert.AreEqual(old, current, "Не увеличилось количество строк в таблице.");
        }

        /// <summary>
        /// Информация о таблице с данными
        /// </summary>
        internal class TableInfo
        {
            /// <summary>
            /// Создает экземпляр класса <see cref="TableInfo"/>.
            /// </summary>
            /// <param name="gridId">Идентификатор таблицы с даными.</param>
            public TableInfo(string gridId)
            {
                this.CurrentGridId = gridId;
                var table = Grid.GetGridByid(gridId);
                this.DataRowsCount = table.DataRows.Count();
            }

            /// <summary>
            /// Информация о таблице с данными в контексте текущего сценария
            /// </summary>
            public static TableInfo SavedState
            {
                get
                {
                    if (ScenarioContext.Current.ContainsKey("TableInfo"))
                    {
                        return (TableInfo)ScenarioContext.Current["TableInfo"];
                    }

                    return null;
                }

                set
                {
                    ScenarioContext.Current["TableInfo"] = value;
                }
            }

            /// <summary>
            /// Кол-во строк с данными
            /// </summary>
            public int DataRowsCount { get; internal set; }

            /// <summary>
            /// общее кол-во строк с данными
            /// </summary>
            public int TotalCount { get; internal set; }

            /// <summary>
            /// Текущая таблица
            /// </summary>
            public Grid CurrentGrid
            {
                get
                {
                    return Grid.GetGridByid(this.CurrentGridId);
                }
            }

            private string CurrentGridId { get; set; }
        }
    }
}