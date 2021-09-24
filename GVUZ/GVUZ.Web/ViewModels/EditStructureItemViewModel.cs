using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.ViewModels
{
	public class EditStructureItemViewModel : AddStructureItemViewModel
	{
		public override IEnumerable StructureTypeList
		{
			get
			{
				return new[]
				       	{new {ID = StructureType, Description = ((InstitutionItemType) StructureType).GetName()}};
			}
		}

		public EditStructureItemViewModel()
		{
			IsAdd = false;
		}

		public EditStructureItemViewModel(int structureItemID) : base(structureItemID)
		{
			IsAdd = false;
		}
	}
}