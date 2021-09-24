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
	public class CampaignEditViewModel
	{
		public int CampaignID { get; set; }

		[DisplayName("Название")]
		[LocalRequired]
		[StringLength(50)]
		public string Name { get; set; }

        [DisplayName("Дополнительный набор")]
        public bool Additional { get; set; }

		[LocalRequired]
		public int YearStart { get; set; }
		[LocalRequired]
		public int YearEnd { get; set; }

		public IEnumerable YearRange { get; set; }

		[DisplayName("Формы обучения")]
		public int EducationFormFlags { get; set; }

		public class EducationLevelData
		{
			[DisplayName("Курс")]
			public int Course { get; set; }
			public int EducationLevelID { get; set; }
			public bool IsAvailable { get; set; }
			public bool IsSelected { get; set; }
		}
        
		[DisplayName("Уровень образования")]
		public EducationLevelData[] EducationLevels { get; set; }

		public class EducationLevelName
		{
			public int ID { get; set; }
			public string Name { get; set; }
		}

		public EducationLevelName[] EducationLevelNames { get; set; }

		public int[] Courses { get; set; }

		public int StatusID { get; set; }

		public bool CanEdit { get; set; }

		public bool HasGroups { get; set; }

        public bool HasODates { get; set; }
        public bool HasZDates { get; set; }
        public bool HasOZDates { get; set; }

		[DisplayName("UID")]
		[StringLength(200)]
		public string UID { get; set; }

     //   [DisplayName("Прием жителей Республики Крым и города Севастополя")]
	    //public bool IsFromKrym { get; set; }
	}
}