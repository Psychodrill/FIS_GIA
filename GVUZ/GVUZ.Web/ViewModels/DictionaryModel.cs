using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels {

	public class DictionaryModel {
		public string Key { get; set; }
		public string Title { get; set; }
		// Допольнительыне поля 
		public Dictionary<string,string> Fields { get; set; }
	}
	public class DictionaryFilterModel {
		public string Dictionary { get; set; }
		public string Key { get; set; }
		public string Title { get; set; }
		// Допольнительыне поля фильтра типа
		public Dictionary<string,string> Fields { get; set; }
	}

	public class IDName {
		public int ID { get; set; }
		public string Name { get; set; }
        public int Year { get; set; }
	}
}