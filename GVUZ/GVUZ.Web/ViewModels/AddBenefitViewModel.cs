using System.Collections.Generic;
using System.ComponentModel;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.ViewModels
{
	public class AddBenefitViewModel : BaseEditViewModel
	{
		public int InstitutionID { get; set; }
		public int BenefitItemID { get; set; }

		[DisplayName("Тип диплома")]
		public int DiplomaTypeID { get; set; }
		[DisplayName("Уровень олимпиады")]
		public int ComptetitionLevelID { get; set; }
	    [DisplayName("Вид льготы")]
		public int BenefitTypeID { get; set; }

		public class IDName
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		public IEnumerable<IDName> DiplomaTypes { get; set; }
		public IEnumerable<IDName> CompetitionLevels { get; set; }
		public IEnumerable<IDName> BenefitTypes { get; set; }

		public AddBenefitViewModel() {}
		public AddBenefitViewModel(int institutionID)
		{
			InstitutionID = institutionID;
		}

		public AddBenefitViewModel(int institutionID, int benefitID)
		{
			InstitutionID = institutionID;
			BenefitItemID = benefitID;
		}
	}
}