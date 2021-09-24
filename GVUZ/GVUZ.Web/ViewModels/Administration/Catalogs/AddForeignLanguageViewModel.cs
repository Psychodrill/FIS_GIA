using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddForeignLanguageViewModel
	{
		[DisplayName("Название языка")]		
		public int LanguageID { get; set; }

		[DisplayName("Название языка")]
		[LocalRequired]
		[StringLength(255)]
		public string Name { get; set; }

		public AddForeignLanguageViewModel() { }

		public AddForeignLanguageViewModel(int languageId)
		{
			LanguageID = languageId;
		}
	}
}