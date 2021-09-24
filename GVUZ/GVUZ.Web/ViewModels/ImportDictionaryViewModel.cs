using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
	public class ImportDictionaryViewModel
	{
		public ImportDictionaryViewModel(IEnumerable<ImportDictionaryItem> dictionaries)
		{
			Dictionaries = dictionaries;
		}

		public IEnumerable<ImportDictionaryItem> Dictionaries { get; set; }

		public ImportDictionaryItem Description { get { return null; } }
	}

	public class ImportDictionaryItem
	{
		[DisplayName("Действие")]
		public int DictionaryID { get; set; }
		[DisplayName("Наименование")]
		public string DictionaryName { get; set; }
	}
}