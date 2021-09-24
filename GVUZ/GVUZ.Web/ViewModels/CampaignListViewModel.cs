using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class CampaignListViewModel
	{
		public int? SortID { get; set; }
		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemCount { get; set; }

		public class CampaignData
		{
			[DisplayName("Действие")]
			public int CampaignID { get; set; }
			[DisplayName("Название")]
			public string Name { get; set; }
			[DisplayName("Сроки проведения")]
			public string CampaignYearRange { get; set; }
			[DisplayName("Формы обучения")]
			public string EducationForms { get; set; }
			[DisplayName("Курсы")]
			public string Courses { get; set; }
			[DisplayName("Уровни образования")]
			public string EducationLevels { get; set; }

			[DisplayName("Статус")]
			public string StatusName { get; set; }

			public int StatusID { get; set; }

            [DisplayName("Дата завершения")]
            public string DateOfEnd { get; set; }
		}

		public CampaignData CampaignDescr
		{
			get { return null; }
		}

		public CampaignData[] Campaigns { get; set; }

		public bool CanCreateNewCampaign { get; set; }
	}
}