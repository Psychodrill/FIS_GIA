using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы со структурой вуза
	/// </summary>
	public static class InstitutionStructureExtensions
	{
		/// <summary>
		/// Добавляем новый элемент структуры
		/// </summary>
		public static AjaxResultModel AddStructure(this InstitutionsEntities dbContext, AddStructureItemViewModel model)
		{
			object result;
			if (model.StructureType == 0)
				return new AjaxResultModel("Некорректный тип");
			if (model.FullName == null || model.FullName.Trim() == "")
				return new AjaxResultModel("Не введено наименование");
			InstitutionItem inst = new InstitutionItem();
			inst.BriefName = model.BriefName;
			inst.Name = model.FullName.Trim();
			inst.Site = model.Site;
			if (model.DirectionCode != null)
				model.DirectionCode = model.DirectionCode.Trim();
			if (!String.IsNullOrEmpty(model.DirectionCode))
			{
                Direction foundDir = null;

                if (model.DirectionCode.StartsWith("/"))
                {
                    //Обработка только нового кода, старого нет
                    string dirNewCode = model.DirectionCode.Replace("/", "");

                    foundDir = dbContext.Direction.Where(d => d.NewCode.Trim() == dirNewCode.Trim()).FirstOrDefault();
                }
                else if (model.DirectionCode.EndsWith("/"))
                {
                    // Обработка только старого кода, нового нет
                    string[] dirCodes = model.DirectionCode.Replace("/","").Split('.');
                    string s1, s2;
                    if (dirCodes.Length > 1)
                    {
                        s1 = dirCodes[0];
                        s2 = dirCodes[1];
                        foundDir = dbContext.Direction.Where(d => d.Code == s1 && d.QUALIFICATIONCODE == s2).FirstOrDefault();
                    }
                }
                else
                {
                    // Есть и старый, и новый код
                    string[] codes = model.DirectionCode.Split('/');
                    if (codes.Length == 2)
                    {
                        string[] parts = codes[0].Split('.');
                        if (parts.Length > 1)
                        {
                            string s1, s2, s3;
                            s1 = parts[0];
                            s2 = parts[1];
                            s3 = codes[1];
                            foundDir = dbContext.Direction.Where(d => d.Code == s1 && d.QUALIFICATIONCODE == s2 && d.NewCode.Trim() == s3.Trim()).FirstOrDefault();
                        }
                    }
                }
                
                if (foundDir != null)                
                  inst.DirectionID = foundDir.DirectionID;                
                else 
                  return new AjaxResultModel("Не найден код подразделения");
			}

			inst.ItemTypeID = model.StructureType;
			var parent = GetParentInstitutionInfo(dbContext, model.StructureItemID);

			if (parent != null)
			{
				inst.InstitutionID = parent.InstitutionID;
				inst.ParentID = parent.InstitutionItemID;

				InstitutionStructure structure = new InstitutionStructure
				{
					InstitutionItem = inst,
					ParentID = parent.StructureID,
					Lineage = "",
					Depth = 1
				};
				dbContext.InstitutionStructure.AddObject(structure);
				try
				{
					dbContext.SaveChanges();
				}
				catch (Exception ex)
				{
					SqlException inner = ex.InnerException as SqlException;
					if (inner != null && inner.Message.Contains("UK_InstitutionItem_Name"))
						return new AjaxResultModel("Уже существует " + "элемент структуры"
							/*((InstitutionItemType)model.StructureType).GetName()*/ + " с данным наименованием");
					throw;
				}
				//result = new { ItemID = structure.InstitutionStructureID, inst.Name };
				result = new TreeItemViewModel
				         {
				         	ItemID = structure.InstitutionStructureID,
				         	Name = inst.Name,
				         	CanAdd = inst.ItemType != InstitutionItemType.Direction,
				         	CanEdit = true,
				         	CanDelete = true,
				         	IsLeaf = true
				         };
			}
			else
				return new AjaxResultModel("Не найдена структура");
			return new AjaxResultModel { Data = result };
		}

		/// <summary>
		/// Обновить элемент структуры
		/// </summary>
		public static AjaxResultModel UpdateStructure(this InstitutionsEntities dbContext, EditStructureItemViewModel model)
		{
			var inst = GetInstitutionInfo(dbContext, model.StructureItemID);
			if (model.FullName == null || model.FullName.Trim() == "")
				return new AjaxResultModel("Не введено наименование");
			inst.BriefName = model.BriefName;
			inst.Name = model.FullName.Trim();
			inst.Site = model.Site;

			if (model.DirectionCode != null)
				model.DirectionCode = model.DirectionCode.Trim();
			if (!String.IsNullOrEmpty(model.DirectionCode))
			{
                Direction foundDir = null;
                string[] dirCodes = model.DirectionCode.Split('.');
                string s1, s2;
                if (dirCodes.Length > 1)
                {
                    s1 = dirCodes[0];
                    s2 = dirCodes[1];
                    foundDir = dbContext.Direction.Where(d => d.Code == s1 && d.QUALIFICATIONCODE == s2).FirstOrDefault();
                }
                if (foundDir != null)                
                 inst.DirectionID = foundDir.DirectionID;                
                else 
                 return new AjaxResultModel("Не найден код подразделения");
			}

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				SqlException inner = ex.InnerException as SqlException;
				if (inner != null)
				{
					if (inner.Message.Contains("UK_InstitutionItem_Name"))
					{
						return
							new AjaxResultModel("Уже существует " + 
								"элемент структуры" +
								//((InstitutionItemType)model.StructureType).GetName() +
												" с данным наименованием");
					}
				}

				throw;
			}

			bool isLeaf = !dbContext.InstitutionStructure.Where(x => x.ParentID == model.StructureItemID).Any();

			return new AjaxResultModel 
			{
				Data = new TreeItemViewModel
				         {
				         	ItemID = model.StructureItemID,
				         	Name = inst.Name,
				         	CanAdd = inst.ItemType != InstitutionItemType.Direction,
				         	CanEdit = true,				         	CanDelete = true,
				         	IsLeaf = isLeaf
				         }
			};
		}

		/// <summary>
		/// Загрузить элемент структуры
		/// </summary>
		public static void LoadStructure(this InstitutionsEntities dbContext, EditStructureItemViewModel model)
		{
			InstitutionItem item = GetInstitutionInfo(dbContext, model.StructureItemID);
			model.FullName = item.Name;
			model.BriefName = item.BriefName;
			model.StructureType = (short)item.ItemType;
			model.Site = item.Site;
			if (item.Direction == null)
				model.DirectionCode = dbContext.Direction
										.Where(d => d.DirectionID == item.DirectionID)
										.Select(d => d.Code + "." + d.QUALIFICATIONCODE)
										.FirstOrDefault();
			else
                model.DirectionCode = item.Direction.Code + "." + item.Direction.QUALIFICATIONCODE;
			model.StructureItemName = item.Name;
		}

		/// <summary>
		/// Загрузить начальную структуру вуза
		/// </summary>
		public static AddStructureItemViewModel FillInitialStructure(this InstitutionsEntities dbContext, AddStructureItemViewModel model)
		{
			int depth = dbContext.InstitutionStructure.Where(x => x.InstitutionStructureID == model.StructureItemID).Select(x => x.Depth).First();
			if (depth == 1)
				model.StructureItemName = dbContext
					.InstitutionStructure
						.Where(x => x.InstitutionStructureID == model.StructureItemID)
						.Select(x => x.InstitutionItem.Institution.FullName).FirstOrDefault();
			else
				model.StructureItemName = dbContext
					.InstitutionStructure.Where(x => x.InstitutionStructureID == model.StructureItemID).Select(x => x.InstitutionItem.Name).FirstOrDefault();
			return model;
		}

		/// <summary>
		/// Загрузить информацию об элементе структуры
		/// </summary>
		private static InstitutionItem GetInstitutionInfo(InstitutionsEntities dbContext, int structureItemID)
		{
			return (from s in dbContext.InstitutionStructure
					join ii in dbContext.InstitutionItem on s.InstitutionItemID equals ii.InstitutionItemID
					where s.InstitutionStructureID == structureItemID
					select ii)
				.FirstOrDefault();
		}

		private class ParentInstitutionInfo
		{
			public int StructureID { get; set; }
			public int InstitutionID { get; set; }
			public int InstitutionItemID { get; set; }
			public InstitutionItemType ItemType { get; set; }
		}

		/// <summary>
		/// Получить родительскую структуру вуза
		/// </summary>
		private static ParentInstitutionInfo GetParentInstitutionInfo(InstitutionsEntities dbContext, int structureItemID)
		{
			return (from s in dbContext.InstitutionStructure
					join ii in dbContext.InstitutionItem on s.InstitutionItemID equals ii.InstitutionItemID
					where s.InstitutionStructureID == structureItemID
					select new ParentInstitutionInfo
					       {
					       		StructureID = s.InstitutionStructureID,
								InstitutionID = ii.InstitutionID,
								InstitutionItemID = ii.InstitutionItemID,
								ItemType = (InstitutionItemType)ii.ItemTypeID
					       })
				.FirstOrDefault();
		}

		/// <summary>
		/// Получить возможные дочерние элементы структуры
		/// </summary>
		public static List<InstitutionItemType> GetAvailableDescendantStructure(this InstitutionsEntities dbContext, int parentStrucutreItemID)
		{
			List<InstitutionItemType> types = new List<InstitutionItemType> { InstitutionItemType.Faculty, InstitutionItemType.Department, InstitutionItemType.Direction };
			var parent = GetParentInstitutionInfo(dbContext, parentStrucutreItemID);
			if (parent != null)
			{
				if (parent.ItemType == InstitutionItemType.Faculty)
					types.RemoveRange(0, 1);
				else if (parent.ItemType == InstitutionItemType.Department)
					types.RemoveRange(0, 2);
				//incorrect situation, should no call with such type
				else if (parent.ItemType == InstitutionItemType.Direction)
					types.RemoveRange(0, 3);
			}

			return types;
		}

		/// <summary>
		/// Удалить элемент структуры вуза
		/// </summary>
		public static AjaxResultModel DeleteStructure(this InstitutionsEntities context, int? structureItemID)
		{
			if (!structureItemID.HasValue)
				return new AjaxResultModel("Не найдена структура");
			var instItem = (from s in context.InstitutionStructure
			                join ii in context.InstitutionItem on s.InstitutionItemID equals ii.InstitutionItemID
			                where s.InstitutionStructureID == structureItemID
			                select new { ii, s, s.Children })
											.FirstOrDefault();

			if (instItem != null)
			{
				// удаляем только листы
				if (instItem.Children.Count == 0)
				{
					context.InstitutionItem.DeleteObject(instItem.ii);
					context.InstitutionStructure.DeleteObject(instItem.s);
					context.SaveChanges();
				}
				else
					return new AjaxResultModel(Messages.DeleteChildrenElementsFirst);
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// Получить возможные разрешённые направления для структуры
		/// </summary>
		public static List<Direction> GetAllowedInstitutionItemDirections(this InstitutionsEntities dbContext, int parentStrucutreItemID)
		{
			var q = (from s in dbContext.InstitutionStructure
					join ii in dbContext.InstitutionItem on s.InstitutionItemID equals ii.InstitutionItemID
					join i in dbContext.Institution on ii.InstitutionID equals i.InstitutionID
					join ad in dbContext.AllowedDirections on ii.InstitutionID equals ad.InstitutionID
					where s.InstitutionStructureID == parentStrucutreItemID
					select ad.Direction).Distinct();
			return q.ToList();
		}
	}
}