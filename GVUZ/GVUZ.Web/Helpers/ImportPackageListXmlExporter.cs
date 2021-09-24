using System;
using System.IO;
using System.Linq;
using CarlosAg.ExcelXmlWriter;
using GVUZ.ServiceModel.Import;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.Helpers
{	
	/// <summary>
	/// Генератор в Excel списка пактов импорта
	/// </summary>
	public class ImportPackageListXmlExporter
	{
		private const string HeaderStyle = "HeaderStyle";
		private const string NotSet = "Не установлены";

		private readonly Workbook _book = new Workbook();
		private readonly Worksheet _applicationsSheet;

		private readonly string[] _titles = new[]
		{
			"ID", "Название ОО", "Дата отправки",
			"Дата обработки", "Тип", "Содержание", "Статус"
		};

		public ImportPackageListXmlExporter()
		{
			_applicationsSheet = _book.Worksheets.Add("Список запросов");
			var style = _book.Styles.Add(HeaderStyle);
			style.Font.Bold = true;
			style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
		}

		public void AddFilterRow(string filtersString)
		{
			if (string.IsNullOrEmpty(filtersString))
				filtersString = NotSet;

			var row = _applicationsSheet.Table.Rows.Add();
			row.Cells.Add("Фильтры:");
			row.Cells.Add(filtersString);
		}

		internal void AddFilterRow(ImportListViewModel model)
		{
			var filterString = string.Empty;

			if (!String.IsNullOrWhiteSpace(model.SelectedInstitution))
				filterString += string.Format("Название ОО: {0};", model.SelectedInstitution);

			using (var dbContext = new ImportEntities())
			{
				if (model.SelectedType != 0)
				{
					var importPackageType = dbContext.ImportPackageType.Where(x => x.TypeID == model.SelectedType).Select(x => new { x.Name }).FirstOrDefault();
					if (importPackageType != null) filterString += string.Format("Тип: {0};", importPackageType.Name);
				}
			}
				
			if (model.DateBegin.HasValue)
				filterString += string.Format("Дата отправки с: {0};", model.DateBegin.Value.ToString("dd.MM.yyyy"));
			if (model.DateEnd.HasValue)
				filterString += string.Format("Дата отправки до: {0};", model.DateEnd.Value.ToString("dd.MM.yyyy"));
			
			AddFilterRow(filterString);
		}

		public void AddHeader()
		{
			var row = _applicationsSheet.Table.Rows.Add();
			foreach (var title in _titles)
			{
				row.Cells.Add(new WorksheetCell(title, HeaderStyle));
			}
		}

		public void AddRow(RowData data)
		{
			var row = _applicationsSheet.Table.Rows.Add();
			row.Cells.Add(data.ID.ToString());
			row.Cells.Add(data.InctitutionName);
			row.Cells.Add(data.DateSend);
			row.Cells.Add(data.DateProcessing);
			row.Cells.Add(data.Type);
			row.Cells.Add(data.Content);
			row.Cells.Add(data.Status);
		}

		public string SaveToTempFile()
		{
			var fileName = Guid.NewGuid().ToString();
			var path = Path.Combine(Path.GetTempPath(), fileName + ".xls");
			Save(path);
			return fileName;
		}

		public void Save(string fileName)
		{
			// sucks
			_applicationsSheet.Table.Columns.Add().Width = 40;
			_applicationsSheet.Table.Columns.Add().Width = 100;
			_applicationsSheet.Table.Columns.Add().Width = 100;
			_applicationsSheet.Table.Columns.Add().Width = 100;
			_applicationsSheet.Table.Columns.Add().Width = 300;
			_applicationsSheet.Table.Columns.Add().Width = 400;
			_applicationsSheet.Table.Columns.Add().Width = 100;

			_book.Save(fileName);
		}

		public class RowData
		{
			public int ID { get; set; }
			public string InctitutionName { get; set; }
			public string DateSend { get; set; }
			public string DateProcessing { get; set; }
			public string Type { get; set; }
			public string Content { get; set; }
			public string Status { get; set; }
		}		
	}
}