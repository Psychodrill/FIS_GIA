using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;

namespace GVUZ.Web.ViewModels
{
	public class EditAdmissionItemViewModel : AddAdmissionItemViewModel
	{
		public EditAdmissionItemViewModel()
		{
			IsAdd = false;
		}
		public EditAdmissionItemViewModel(int admissionStructureItemID) : base(admissionStructureItemID)
		{
			IsAdd = false;
		}
	}
}