using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class DirectionData
	{
		[DisplayName("Действие")]
		public int DirectionID { get; set; }

		[DisplayName("Код")]
		public string Code { get; set; }

		[DisplayName("Направление подготовки (специальность)")]
		public string Name{ get; set; }		
	}
}