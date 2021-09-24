using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class CreativeDirectionViewModel : IPageable, IOrderable
	{
		public DirectionData[] Directions { get; set; }

		public DirectionData DataDescr { get { return null; } }

		public int? PageNumber { get; set; }

		public int TotalPageCount { get; set; }

		public int? SortID { get; set; }
	}
}