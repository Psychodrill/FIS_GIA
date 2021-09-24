namespace FBS.Spec.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using WatiN.Core;

    internal static class GridHelper
    {
        public static CheckBox GetSelectCheckBox(this TableRow row)
        {
            return row.TableCells[0].CheckBoxes[0];
        }

        /// <summary>
        /// Делает выделение строки в таблице
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="rowIndex">Индекс строки</param>
        public static void SelectRow(this IGrid grid, int rowIndex)
        {
            var row = grid.DataRows.ToArray()[rowIndex - 1];
            row.GetSelectCheckBox().Checked = true;
        }

        /// <summary>
        /// Получение выбранных строк
        /// </summary>
        /// <param name="grid">Грид.</param>
        /// <returns>Выбранные строки</returns>
        public static IEnumerable<TableRow> GetSelectedRows(this IGrid grid)
        {
            return grid.DataRows.Where(p => p.GetSelectCheckBox().Checked);
        }
    }
}