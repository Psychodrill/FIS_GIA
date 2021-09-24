using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddQualificationViewModel
	{
		[DisplayName("Наименование квалификации/степени")]
		public int QualificationID { get; set; }

		[DisplayName("Наименование квалификации/степени")]
		[LocalRequired]
		[StringLength(255)]
		public string Name { get; set; }

		[DisplayName("Код")]
		[LocalRequired]
		[StringLength(10)]
		public string Code { get; set; }

		public AddQualificationViewModel(){}

		public AddQualificationViewModel(int qualificationId)
		{
			QualificationID = qualificationId;
		}
	}
}