namespace FBS.Spec.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using FBS.Spec.BaseSteps;
    using FBS.Spec.Controls;

    using WatiN.Core;

    /// <summary>
    /// Класс для тестирования гридов
    /// </summary>
    public class Grid : Control<Table>, IGrid
    {
        /// <summary>
        /// Футер таблицы
        /// </summary>
        public TableRow Footer
        {
            get
            {
                return this.Table.TableRow(TableRow.IsFooterRow());
            }
        }

        /// <summary>
        /// Хидер таблицы
        /// </summary>
        public TableRow Header
        {
            get
            {
                return this.Table.TableRows[0];
            }
        }

        /// <summary>
        /// Строки таблицы с данными
        /// </summary>
        public IEnumerable<TableRow> DataRows
        {
            get
            {
                return this.Table.TableBodies[0].TableRows.Where(p => p.ClassName != "empty");
            }
        }

        /// <summary>
        /// Показывает, есть ли в гриде колонка с чекбоксами
        /// </summary>
        public bool HasCheckboxColumn
        {
            get
            {
                return this.Header.CheckBoxes.Count > 0;
            }
        }

        /// <summary>
        /// Gets a constraint that is used to help locate the element that belongs to the control.
        /// </summary>
        public override WatiN.Core.Constraints.Constraint ElementConstraint
        {
            get
            {
                return Find.BySelector("table.grid");
            }
        }

        /// <summary>
        /// Таблица
        /// </summary>
        protected virtual Table Table
        {
            get
            {
                return new Table(this.Element.DomContainer, this.Element.FindNativeElement());
            }
        }

        /// <summary>
        /// Получение первого найденного грида на странице
        /// </summary>
        /// <returns>Грид</returns>
        public static Grid GetFirstGrid()
        {
            var divForGrid = WebBrowser.Current.Div(Find.ByClass("content-in"));
            var grid = divForGrid.Control<Grid>(Find.ByClass("result-list-table"));
            Assert.IsTrue(grid.Element.Exists, "На странице не найдено ни 1 грида");
            return grid;
        }

        /// <summary>
        /// Получение таблицы по идентификатору
        /// </summary>
        /// <param name="tableId">
        /// Идентификатор таблицы.
        /// </param>
        /// <returns>
        /// таблица
        /// </returns>
        public static Grid GetGridByid(string tableId)
        {
            var divForContentGrid = WebBrowser.Current.Div(Find.ByClass("content-in"));
            var grid = divForContentGrid.Control<Grid>(Find.ByClass(tableId));
            Assert.IsTrue(grid.Element.Exists, "Таблица с идентификатором '" + tableId + "' не найдена!");
            return grid;
        }

        /// <summary>
        /// Убирает выделение в строке
        /// </summary>
        /// <param name="row">Строка</param>
        public void UnSelectRow(TableRow row)
        {
            row.GetSelectCheckBox().Checked = false;
        }

        /// <summary>
        /// Убирает выделение в строке
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        public void UnSelectRow(int rowIndex)
        {
            var row = this.DataRows.ToArray()[rowIndex - 1];
            row.GetSelectCheckBox().Checked = false;
        }
    }
}
