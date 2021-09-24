using System.ComponentModel;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.ViewModels
{
	public class BenefitViewModel : BaseEditViewModel
	{
		public int InstitutionID { get; set; }

		[DisplayName("Тип диплома")]
		public string DiplomaType { get; set; }
		[DisplayName("Уровень")]
		public string ComptetitionLevel { get; set; }
		[DisplayName("Вид льготы")]
		public string BenefitType { get; set; }


		public BenefitViewModel(int institutionID)
		{
			InstitutionID = institutionID;
		}

		public class BenefitData
		{
			public int BenefitItemID { get; set; }
			public int DiplomaTypeID { get; set; }
			public string DiplomaType { get; set; }
			public int CompetitionLevelID { get; set; }
			public string CompetitionLevel { get; set; }
			public int BenefitTypeID { get; set; }
			public string BenefitType { get; set; }
		}

		public BenefitData[] Benefits { get; set; }
	}
}