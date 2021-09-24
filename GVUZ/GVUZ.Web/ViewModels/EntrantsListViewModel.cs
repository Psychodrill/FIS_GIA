using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FogSoft.Web.Mvc;
using GVUZ.Model.Entrants;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels
{
	public class EntrantsListViewModel : IOrderable, IPageable
	{
		public class EntrantData
		{
			public int EntrantID { get; set; }

			[DisplayName(@"Ф.И.О.")]
			public string FullName { get; set; }

			[DisplayName(@"Док-т, удостоверяющий личность")]
			public string IdentityDocument { get; set; }

			public ApplicationData[] Applications { get; set; }

			public string EntrantUID { get; set; }
		}

		public FilterDetails Filter { get; set; }
		public IEnumerable CompetitiveGroups { get; set; }
		public IEnumerable ApplicationStatuses { get; set; }
        public IEnumerable Campaigns { get; set; }

		public EntrantData EntrantDataNull
		{
			get { return null; }
		}

		public ApplicationData ApplicationDataNull
		{
			get { return null; }
		}

		public EntrantData[] Entrants { get; set; }

		public int? SortID { get; set; }

		public int? PageNumber { get; set; }
		public int TotalPageCount { get; set; }
		public int TotalItemFilteredCount { get; set; }
		public int TotalItemCount { get; set; }

        public class FilterDetails
        {
            [DisplayName("Номер заявления")]
            public string ApplicationNumber { get; set; }

            [DisplayName("Дата регистрации с")]
            [Date(">today-100y")]
            [Date("<=today")]
            public DateTime? DateBegin { get; set; }
            [DisplayName("по")]
            [Date(">today-100y")]
            [Date("<=today")]
            public DateTime? DateEnd { get; set; }

            [DisplayName("Конкурс")]
            public string CompetitiveGroupName { get; set; }

            [DisplayName("Статус заявления")]
            public int[] ApplicationStatusID { get; set; }

            [DisplayName("Фамилия")]
            public string EntrantLastName { get; set; }
            [DisplayName("Имя")]
            public string EntrantFirstName { get; set; }
            [DisplayName("Отчество")]
            public string EntrantMiddleName { get; set; }
            [DisplayName("Серия паспорта")]
            public string EntrantDocSeries { get; set; }
            [DisplayName("№ паспорта")]
            public string EntrantDocNumber { get; set; }

            [DisplayName("Приёмная кампания")]
            public string CampaignName { get; set; }
        }
	}


}