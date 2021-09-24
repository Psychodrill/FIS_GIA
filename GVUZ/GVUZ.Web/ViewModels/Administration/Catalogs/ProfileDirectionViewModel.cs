using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class ProfileDirectionViewModel : IPageable, IOrderable
	{
	
		public class InstitutionProfileDirectionData
		{
			[DisplayName("Действие")]
			public int InstitutionID { get; set; }						

			[DisplayName("ВУЗ")]
			public string Name { get; set; }

			public DirectionData[] Directions { get; set; }

			[DisplayName("Направление подготовки (специальность)")]
			public string DirectionsString
			{
				get
				{
					return Directions != null
					       	? Directions.Aggregate(string.Empty, (current, directionData) => current + (directionData.Name + ";"))
					       	  	.TrimEnd(new char[] {';'})
					       	: "";
				}
			}			
		}		

		public InstitutionProfileDirectionData[] Directions { get; set; }

		public InstitutionProfileDirectionData DataDescr { get { return null; } }

		public int? PageNumber { get; set; }

		public int TotalPageCount { get; set; }

		public int? SortID { get; set; }
	}
}