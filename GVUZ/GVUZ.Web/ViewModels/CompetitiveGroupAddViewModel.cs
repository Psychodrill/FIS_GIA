using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupAddViewModel
	{
		public int GroupID { get; set; }

		[DisplayName("Наименование конкурса")]
		[StringLength(250)]
		[LocalRequired]
		public string Name { get; set; }

		[DisplayName("Курс")]
		[LocalRequired]
		public short CourseID { get; set; }

		//[DisplayName("Уровень образования")]
		//[LocalRequired]
		//public short EducationLevelID { get; set; }

		[DisplayName("Приемная кампания")]
		[LocalRequired]
		public int CampaignID { get; set; }

		public class BaseData
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		public class CampaignData
		{
			public BaseData Campaign { get; set; }
			public int ID { get { return Campaign.ID; } }

			public string Name { get { return Campaign.Name; } }
			public BaseData[] Courses { get; set; }
			//public BaseData[] EducationLevels { get; set; }
			//public string[] EdLevels_Courses { get; set; }
			public string[] Campaign_Courses { get; set; }
		}

		public CampaignData[] Campaigns;
	}
}