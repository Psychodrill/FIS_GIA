using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GVUZ.Model;

namespace GVUZ.Web.ViewModels
{
	public class CampaignDateEditViewModel
	{
		public int CampaignID { get; set; }
		public int StatusID { get; set; }
		public bool CanEdit { get; set; }

		public class CampaignDateDetails : IComparable<CampaignDateDetails>
		{
			public int EducationFormID { get; set; }
			public int EducationLevelID { get; set; }
			public int EducationSourceID { get; set; }
			public int Course { get; set; }
			public int Stage { get; set; }
			[DisplayName("Начало приема документов")]
			public DateTime DateStart { get; set; }
			[DisplayName("Окончание приема документов")]
			public DateTime DateEnd { get; set; }
			[DisplayName("Издание приказа о зачислении")]
			public DateTime DateOrder { get; set; }

			[DisplayName("UID")]
			[StringLength(200)]
			public string UID { get; set; }

			[DisplayName("Активно")]
			public bool IsActive { get; set; }

			public bool CanRemoveDate { get; set; }

			public int CompareTo(CampaignDateDetails other)
			{
				var cmp = EducationFormID.CompareTo(other.EducationFormID);
				if (cmp != 0) return cmp;
				cmp = EducationLevelID.CompareTo(other.EducationLevelID);
				if (cmp != 0) return cmp;
				cmp = Course.CompareTo(other.Course);
				if (cmp != 0) return cmp;
				cmp = EducationSourceID.CompareTo(other.EducationSourceID);
				if (cmp != 0) return cmp;
				return Stage.CompareTo(other.Stage);
			}

			public string NameString
			{
				get
				{
					var namePart = DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study, EducationFormID) + ", "
						+ DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, EducationLevelID) + ", "
					+ Course + " курс, ";
					if (Stage != 0)
						namePart += DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.AdmissionType, EducationSourceID) +", "
							+ Stage + " этап";
					else namePart += DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.AdmissionType, EducationSourceID);
					return namePart;
				}
			}

			public string CombinedID
			{
				get
				{
					return EducationFormID + "_" + EducationLevelID + "_" + Course + "_" + EducationSourceID + "_" + Stage;
				}
			}
		}

		public CampaignDateDetails DisplayInfo
		{
			get { return null; }
		}

		public CampaignDateDetails[] CampaignDates { get; set; }

		public int YearStart { get; set; }
		public int YearEnd { get; set; }
	}
}