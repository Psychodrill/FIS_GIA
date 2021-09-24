using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GVUZ.Helper;

namespace GVUZ.Web.ViewModels
{
	public class EntrantInfoViewModelC
	{
		public int EntrantID { get; set; }

		[DisplayName(@"Имя")]
		public string FirstName { get; set; }
		
		[DisplayName(@"Отчество")]
		public string MiddleName { get; set; }
		
		[DisplayName(@"Фамилия")]
		public string LastName { get; set; }

		[DisplayName(@"Ф.И.О.")]
		public string FullName
		{
			get { return NameHelper.GetFullName(FirstName, LastName, MiddleName); }
		}
		
		public DateTime BirthDateTime { get; set; }

		[DisplayName(@"Дата рождения")]
		public string BirthDate { get { return BirthDateTime.ToString("dd.MM.yyyy"); } }

		[DisplayName(@"Пол")]
		public string Gender { get; set; }

		[DisplayName(@"Место рождения")]
		public string BirthPlace { get; set; }

		[DisplayName(@"Вид")]
		public string DocumentType { get; set; }

		[DisplayName(@"Серия / №")]
		public string DocumentSeriesNumber { get; set; }		

		[DisplayName(@"Поданные заявления")]
		public ApplicationData[] Applications { get; set; }

		public EntrantInfoViewModelC()
		{
			Applications = new ApplicationData[0];
		}

		public ApplicationData ApplicationDataNull
		{
			get { return null; }
		}

		[DisplayName("Идентификатор в БД ОО (UID)")]
		[StringLength(200)]
		public string Uid { get; set; }

		//[DisplayName(@"")]
	}
}