using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FogSoft.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;

namespace GVUZ.Web.ViewModels
{
	public class AddAdmissionItemViewModel : BaseEditViewModel
	{
		public int AdmissionStructureItemID { get; set; }
		public string AdmissionItemName { get; set; }

		public bool IsAdd { get; set; }

		[DisplayName("Название")]
		[LocalRequired]
		[StringLength(255)]
		public string FullName { get; set; }

		[DisplayName("Краткое название")]
		[StringLength(50)]
		public string BriefName { get; set; }

		[DisplayName("Тип")]
		[LocalRequired]
		public short AdmissionType { get; set; }

		public virtual IEnumerable AdmissionTypeList {get; set; }
		public virtual IEnumerable AdmissionTypeListAutoCopy { get; set; }

		public int CourseSpecificNameID
		{
			get
			{
				if (!string.IsNullOrEmpty(FullName) && Char.IsDigit(FullName[0]))
					return Convert.ToInt32(FullName.Substring(0, 1));
				return 0;
			}
		}
		public IEnumerable CourseNameDictionary
		{
			get
			{
				for (int i = 1; i <= 6; i++)
					yield return new { ID = i, Name = CompetitiveGroupExtensions.GetCourseName(i) };
			}
		}

		public IEnumerable Directions
		{
			get
			{
				using (var dbContext = new InstitutionsEntities())
				{
					return null;
					//return dbContext.GetAllowedAdmissionDirections(AdmissionStructureItemID).OrderBy(x => x.Code).Select(x => new { x.Code, Name = x.Code + " " + x.Name }).ToList();
				}
			}
		}

		[DisplayName("Код направления")]
		public string DirectionCode { get; set; }

		[DisplayName(@"Общее количество мест по направлению")]
		[LocalRange(1, 99999)]
		public int? TotalDirectionPlaceCount { get; set; }

		[DisplayName("Количество мест")]
		[LocalRange(1, 99999)]
		public int? PlaceCount { get; set; }

		[DisplayName("Условия, определяющие выбор направления")]
		public Guid? FileID { get; set; }

		public string FileName { get; set; }
		public bool FileDeleted { get; set; }

		public AddAdmissionItemViewModel(int admissionStructureItemID)
		{
			IsAdd = true;
			AdmissionStructureItemID = admissionStructureItemID;
		}

		public AddAdmissionItemViewModel()
		{
			IsAdd = true;
		}

		public static string CorrectItemFullName(int itemTypeID, string name, int? placeCount)
		{
			string resName = name;
			if (itemTypeID == AdmissionItemTypeConstants.Faculty && resName.IndexOf(AdmissionItemTypeConstants.FacultyName, StringComparison.CurrentCultureIgnoreCase) < 0)
				resName = AdmissionItemTypeConstants.FacultyName + " " + name;
			if (itemTypeID == AdmissionItemTypeConstants.Cathedra && resName.IndexOf(AdmissionItemTypeConstants.CathedraName, StringComparison.CurrentCultureIgnoreCase) < 0)
				resName = AdmissionItemTypeConstants.CathedraName + " " + name;
			return resName + (placeCount.HasValue ? " (" + placeCount.Value + ")" : "");
		}
	}
}