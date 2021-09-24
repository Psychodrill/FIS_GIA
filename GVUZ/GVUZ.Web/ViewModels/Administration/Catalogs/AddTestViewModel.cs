using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddTestViewModel
	{		
		public int TestID { get; set; }

		public bool IsEditMode
		{
			get { return TestID > 0; }
		}

		public AddTestViewModel()
		{
		}

		public AddTestViewModel(int testId)
		{
			TestID = testId;			
		}		

		public void FillData(EntrantsEntities dbContext)
		{										
		}
		
		public string Validate()
		{
			return string.Empty;
		}
	}
}