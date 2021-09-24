using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupListViewModel
	{
		public CompetitiveGroupViewModel DisplayData { get { return null; } }

		public List<List<CompetitiveGroupViewModel>> TreeItems { get; set; }

		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }
		public int TotalFilteredCount { get; set; }

		public bool HasCompaigns { get; set; }

		public class FilterData
		{
			[DisplayName("Наименование")]
			public string Name { get; set; }
			
			[DisplayName("Курс")]
			public int Course { get; set; }

			[DisplayName("Приемная кампания")]
			public int CampaignID { get; set; }

			[DisplayName("Уровень образования")]
			public int EducationLevelID { get; set; }

            [DisplayName("UID")]
            public string UID { get; set; }

            public override string ToString()
            {
                return string.Format("{0}_{1}_{2}_{3}_{4}",
                    Name, Course, CampaignID, EducationLevelID, UID);
            }
		}

		public FilterData Filter { get; set; }

		public IEnumerable Campaigns { get; set; }
		public IEnumerable Courses { get; set; }
		public IEnumerable EducationLevels { get; set; }
	}
}