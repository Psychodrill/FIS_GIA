using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class IncludeInOrderListViewModel
	{
		public int InstitutionID { get; set; }

		public class OrderData
		{
			[DisplayName("Действие")]
			public int OrderID { get; set; }
			[DisplayName("Приемная кампания")]
			public string CampaignName { get; set; }
		 
			[DisplayName("Тип приказа")]
			public string OrderTypeName { get; set; }

			[DisplayName("Дата приказа")]
			public string OrderDate { get; set; }
			[DisplayName("Количество абитуриентов")]
			public int ApplicationCount { get; set; }
			[DisplayName("Статус")]
			public string StatusName { get; set; }

			public int StatusID { get; set; }

			[StringLength(200)]
			public string UID { get; set; }
		}

		public OrderData OrderDescr
		{
			get { return null; }
		}

		public OrderData[] Orders { get; set; }

		[DisplayName("Приемная кампания")]
		public int SelectedCampaignID { get; set; }
		public IEnumerable Campaigns { get; set; } 

		[DisplayName("Тип приказа")]
		public string SelectedOrderType { get; set; }
		public IEnumerable OrderTypes { get; set; }
	}
}