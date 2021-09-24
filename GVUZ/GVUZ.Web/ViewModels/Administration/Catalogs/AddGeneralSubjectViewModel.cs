using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddGeneralSubjectViewModel
	{
		[DisplayName("Наименование предмета")]
		public int SubjectID { get; set; }

		[DisplayName("Наименование предмета")]
		[LocalRequired]
		[StringLength(50)]
		public string Name { get; set; }

		public AddGeneralSubjectViewModel()
		{
		}

		public AddGeneralSubjectViewModel(int subjectId)
		{
			SubjectID = subjectId;
		}
	}
}