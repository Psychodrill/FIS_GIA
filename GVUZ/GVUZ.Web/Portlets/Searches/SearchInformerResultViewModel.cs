using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.Portlets.Searches
{
	public class SearchInformerResultViewModel
	{
		[DisplayName("Всего найдено")]
		public int SearchResultCount { get; set; }

		[DisplayName("Образовательные учреждения")]
		public List<string> SearchResultInstitutions { get; set; }

		[DisplayName("Количество мест")]
		public List<string> SearchResultPlaces { get; set; }

		[DisplayName("Количество заявлений")]
		public List<string> SearchResultRequests { get; set; }

		[DisplayName("Конкурс")]
		public List<string> SearchResultContest { get; set; }

		[DisplayName("Олимпиады")]
		public List<string> SearchResultOlympics { get; set; }

		[DisplayName("Подготовительные курсы")]
		public List<string> SearchResultCourses { get; set; }

		[DisplayName("Военная кафедра")]
		public List<string> SearchResultWarDepartment { get; set; }
	}
}