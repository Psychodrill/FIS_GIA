using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddCreativeDirectionViewModel
	{
		
		[DisplayName("Направление подготовки (специальность)")]
		public int DirectionID { get; set; }

		[DisplayName("Направление подготовки (специальность)")]
		[LocalRequired]		
		public string Name { get; set; }		

		public AddCreativeDirectionViewModel()
		{
		}

		public AddCreativeDirectionViewModel(int directionId)
		{
			DirectionID = directionId;
		}				

		[ScriptIgnore]
		public string[] DirectionNameList;

		public void FillData(EntrantsEntities dbContext)
		{
			DirectionNameList = dbContext.Direction.OrderBy(x => x.Name).Select(x => x.Code + " " + x.Name).ToArray();
		}
	}
}