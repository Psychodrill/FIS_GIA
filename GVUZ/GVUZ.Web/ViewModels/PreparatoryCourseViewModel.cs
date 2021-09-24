using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.ViewModels
{
	public class PreparatoryCourseViewModel : BaseEditViewModel
	{
		public int InstitutionID { get; set; }

		[DisplayName("Название курса")]
		[LocalRequired]
		public string CourseName { get; set; }

		[DisplayName("Предметы")]
		[LocalRequired]
		public string Subjects { get; set; }

		[DisplayName("Информация о курсе")]
		[LocalRequired]
		public string Information { get; set; }

		[DisplayName("Дополнительные сведения")]
		[LocalRequired]
		public string AdditionalInfo { get; set; }

		public class CourseData
		{
			public class FileData
			{
				public Guid FileID { get; set; }
				public string FileName { get; set; }
			}
			public int CourseID { get; set; }
			public string Name { get; set; }
			public string InformationRaw { get; set; }

			private static readonly Regex _hrefTargetFix = new Regex("<a\\s");
			public string Information
			{
				get
				{
					if (InformationRaw != null)
						return _hrefTargetFix.Replace(InformationRaw, "<a target=\"_blank\"");
					return InformationRaw;
				}
				set { InformationRaw = value; }
			}
			[ScriptIgnore]
			public IEnumerable<string> SubjectsList { get; set; }
			public Guid? FileID;
			public string FileName;

			public string Subjects
			{
				get
				{
					StringBuilder b = new StringBuilder();
					foreach (string s in SubjectsList)
					{
						b.Append(s).Append(";");
					}
					return b.ToString();
				}
			}
		}

		public CourseData[] Courses { get; set; }

		public PreparatoryCourseViewModel()
		{
			
		}

		public PreparatoryCourseViewModel(int institutionID)
		{
			InstitutionID = institutionID;
		}
	}
}