using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class SubjectsAndEgeMinScoreViewModel : IPageable, IOrderable
	{
		public class ScoreData
		{
			[DisplayName(@"Действие")]
			public int SubjectID { get; set; }

			[DisplayName(@"Общеобразовательный предмет")]
			public string SubjectName { get; set; }
						
			public int? MinValue { get; set; }

			[DisplayName(@"Балл")]
			[LocalRange(0, 100)]
			public int MinScore{ get { return MinValue.HasValue ? MinValue.Value : 0; } }
		}

		public ScoreData[] SubjectsAndScores { get; set; }

		public ScoreData ScoreDescr { get { return null; } }

		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int? SortID { get; set; }
	}
}