using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using GVUZ.Helper.MVC;

namespace GVUZ.Web.ViewModels
{
	public class AddPreparatoryCourseViewModel : BaseEditViewModel
	{
		[DisplayName("Название курса")]
		//[LocalRequired]
		public int CourseTypeID { get; set; }

		[DisplayName("Название курса")]
		[LocalRequired]
		[StringLength(250)]
		public string CourseName { get; set; }

		[ScriptIgnore]
		public IEnumerable CourseTypeList { get; set; }

		[DisplayName("Информация о курсе")]
		public string Information { get; set; }

		[DisplayName("Предметы")]
		public string[] SubjectData { get; set; }

		public int InstitutionID { get; set; }

		[DisplayName("Дополнительные сведения")]
		public Guid? FileID { get; set; }

		public bool FileDeleted { get; set; }

		public string FileName { get; set; }

		public int CourseID { get; set; }

		public string[] PredefinedSubjects;

		public AddPreparatoryCourseViewModel() {}
		public AddPreparatoryCourseViewModel(int institutionID)
		{
			InstitutionID = institutionID;
		}

		public AddPreparatoryCourseViewModel(int institutionID, int courseID)
		{
			InstitutionID = institutionID;
			CourseID = courseID;
		}

	}
}