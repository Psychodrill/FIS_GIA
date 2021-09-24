namespace GVUZ.Web.ViewModels
{
	public class TreeViewModel
	{
		public int RootID { get; set; }
		public string RootName { get; set; }
	}

	public class BaseTreeItemViewModel
	{
		public int ItemID { get; set; }
		public string Name { get; set; }
		public bool IsLeaf { get; set; }
	}

	public class ClickableTreeItemViewModel : BaseTreeItemViewModel
	{
		public bool CanClick { get; set; }
	}

	public class InstitutionSearchTreeItemViewModel : ClickableTreeItemViewModel
	{		
		public string NodeUrl { get; set; }
		public int? PlaceCount { get; set; }
		public int? ApplicationCount { get; set; }
		public bool? HasOlympics { get; set; }
		public bool? HasPreparatoryCourses { get; set; }
		public bool? Applicable { get; set; }
		public bool? CanBeChecked { get; set; }
		public string ApplicationStatus { get; set; }
		public int? ApplicationStatusID { get; set; }
		public int? ApplicationID { get; set; }
		public int? EntrantApplicationCount { get; set; }
		public bool? HasMilitaryDepartment { get; set; }
		public decimal? Competition { get; set; }

		public object GetObject()
		{
			return new ObjectWithArray{ 
				ItemID = ItemID, 
				Name = Name,
				IsLeaf = IsLeaf, 
				CanClick = CanClick, 
				NodeUrl = NodeUrl,
				DisplayArray = 
					new object[]
						{
							PlaceCount, Competition, HasOlympics, ApplicationCount, HasPreparatoryCourses, HasMilitaryDepartment, 
							new {
								Applicable,
								CanBeChecked,
								ApplicationStatus,
								ApplicationStatusID,
								ApplicationID
							}
						}};
		}

		public class ObjectWithArray
		{
			public int ItemID { get; set; }
			public string Name { get; set; }
			public bool IsLeaf { get; set; }
			public bool CanClick { get; set; }
			public string NodeUrl { get; set; }
			public object[] DisplayArray { get; set; }
		}
	}

	public class TreeItemViewModel : BaseTreeItemViewModel
	{
		public bool CanAdd { get; set; }
		public bool CanEdit { get; set; }
		public bool CanDelete { get; set; }
		public bool StructCopy { get; set; }
	}

//====================================================================

    public class ApplicationTreeViewModel : TreeViewModel
    {
    }

	/*public class FullStructureTreeItemViewModel 
	{
		public 
		
	}*/
}