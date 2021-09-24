using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddProfileDirectionViewModel
	{		
		[DisplayName("Вуз")]
		public int InstitutionID { get; set; }

		[DisplayName("Вуз")]
		[LocalRequired]
		public string Name { get; set; }

		public bool IsEditMode
		{
			get { return InstitutionID > 0; }
		}

		public AddProfileDirectionViewModel()
		{
		}

		public AddProfileDirectionViewModel(int institutionId)
		{
			InstitutionID = institutionId;			
		}

		public DirectionData DirectionBriefDataDescr
		{
			get { return null; }
		}

		[DisplayName("Направления подготовки")]
		public DirectionData[] Directions { get; set; }

		[ScriptIgnore]
		public string[] DirectionNameList;

		[ScriptIgnore]
		public string[] InstitutionNameList;

		public void FillData(EntrantsEntities dbContext)
		{
			// full institutions list
			if(!IsEditMode)
				InstitutionNameList = dbContext.Institution.OrderBy(x => x.FullName).Select(x => x.FullName).ToArray();
			// full directions list
			DirectionNameList = dbContext.Direction.OrderBy(x => x.Name).Select(x => x.Code + " " + x.Name).ToArray();			
			// current institute has directions
			//Directions = dbContext.EntranceTestProfileDirection.Where(x => x.InstitutionID == InstitutionID).Select(x => new DirectionData()
			//{
			//	DirectionID = x.Direction.DirectionID,
			//	Name = x.Direction.Name,
			//	Code = x.Direction.Code
			//}).ToArray();										
		}
		
		public string Validate()
		{
			var errors = new StringBuilder();
			var directions = new HashSet<string>();
			if (Directions == null) Directions = new DirectionData[0];
			
			foreach (var data in Directions)
			{
				if (directions.Contains(data.Name))
					errors.AppendLine("Дублируется название направления: " + data.Name);
				else
					directions.Add(data.Name);
			}

			if (Directions.Length == 0)
				errors.AppendLine("Хотя бы одно направление обязательно");

			return errors.ToString();
		}
	}
}