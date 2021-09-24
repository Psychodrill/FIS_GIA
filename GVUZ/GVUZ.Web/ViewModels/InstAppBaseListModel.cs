using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace GVUZ.Web.ViewModels {

	public class InstAppBaseListModel {


		public class DataFileds {

			public DataFileds() { }
		}
		public const string EmptyText="[Не важно]";

		public class FilterFields {	// Фильтры	
			public int InstitutionID { get; set; }
			public string OrderField { get; set; }
			public string OrderDirection { get; set; }
			public int PageSize { get; set; }
			public int CurrentPage { get; set; }
			public int ListType { get; set; } // 0 - Новые, 1 - Не прошедшие проверку, 2 - Отозванные, 3 - Принятые, 4 - Рекомендованные к зачислению

			public FilterFields() { }
		}

		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public int Count { get; set; }
		public int Total { get; set; }

		public List<string> Errors { get; set; }

		public List<DataFileds> Items { get; set; }
		public DataFileds			Data { get { return null; } }
		public FilterFields		Filter {	get { return null; }		}

		public InstAppBaseListModel(){
				Items=new List<DataFileds>();
				Errors=new List<string>();		
		}


	}
}