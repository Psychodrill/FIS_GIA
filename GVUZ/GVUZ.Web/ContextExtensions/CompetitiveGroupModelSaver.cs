using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Сохранение КГ 
	/// </summary>
	internal class CompetitiveGroupModelSaver
	{
		private readonly EntrantsEntities _dbContext;
		private readonly int _institutionID;
		private readonly CompetitiveGroupEditViewModel _model;

		public CompetitiveGroupModelSaver(EntrantsEntities context, CompetitiveGroupEditViewModel model, int institutionID)
		{
			_dbContext = context;
			_institutionID = institutionID;
			_model = model;
			model.Organizations = model.Organizations ?? new CompetitiveGroupEditViewModel.OrganizationData[0];
			model.Rows = model.Rows ?? new CompetitiveGroupEditViewModel.DirectionInfo[0];
		}

		private CompetitiveGroup LoadGroup()
		{
			return _dbContext.CompetitiveGroup
				.Include(x => x.Campaign)
				.FirstOrDefault(x => x.InstitutionID == _institutionID && x.CompetitiveGroupID == _model.GroupID);
		}

		/// <summary>
		/// Проверяем базовые параметры КГ
		/// </summary>
		private AjaxResultModel ValidateGroupBaseProperties(CompetitiveGroup dbGroup)
		{
			if (dbGroup == null)
				return new AjaxResultModel("Не найден конкурс");
			if (dbGroup.Campaign != null && dbGroup.Campaign.StatusID == CampaignStatusType.Finished)
				return new AjaxResultModel("Невозможно редактировать конкурс для завершенной кампании");
			if (_model.Uid != null && _dbContext.CompetitiveGroup
										 .Where(x => x.InstitutionID == _institutionID
											 && x.CompetitiveGroupID != _model.GroupID && x.UID == _model.Uid).Any())
				return new AjaxResultModel("").SetIsError("Uid", "Уже существует конкурс с данным UID'ом");
			return null;
		}

		/// <summary>
		/// Сохранение КГ
		/// </summary>
		public AjaxResultModel SaveCompetitiveGroupEditViewModel()
		{
			var dbGroup = LoadGroup();
			// проверяем базовые параметры
			var res = ValidateGroupBaseProperties(dbGroup);
			if (res != null) return res;
			
			// проверяем дублирование организаций ЦП
			if (_model.Organizations.Select(x => x.Name.Trim()).Distinct().Count() < _model.Organizations.Length)
				return new AjaxResultModel("Названия организаций целевого приема дублируются");
			if (_model.Organizations.Where(x => !String.IsNullOrEmpty(x.UID)).Select(x => x.UID.Trim()).Distinct().Count() < 
				_model.Organizations.Where(x => !String.IsNullOrEmpty(x.UID)).Count())
				return new AjaxResultModel("UID'ы организаций целевого приема дублируются");

			dbGroup.Name = _model.Name;
			dbGroup.UID = _model.Uid;
			dbGroup.DirectionFilterType = _model.DirectionFilterType;
			
			var existingGroupItems = _dbContext.CompetitiveGroupItem
				.Include(x => x.CompetitiveGroup.Direction)
				.Where(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID)
				.ToList();
			//удаляем целевой прием
            _dbContext.CompetitiveGroupTargetItem
                .Where(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID)
                .ToList().ForEach(_dbContext.CompetitiveGroupTargetItem.DeleteObject);

			var directionsToInsert = _model.Rows.Select(x => x.DirectionID).Distinct().ToArray();
			// проверяем правильность направлений в КГ
			res = ValidateDirectionFilterType(directionsToInsert);
			if (res != null) return res;

			// если нет направлений, но есть ВИ 0 ошибка
			if (_model.Rows.Length == 0)
			{
				int entranceTests = _dbContext.EntranceTestItemC.Count(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID);
				if (entranceTests > 0)
					return new AjaxResultModel("Невозможно сохранить конкурс, так как существуют вступительные испытания, но отсутствуют направления");
			}

			//удаляем все удалённые
			var remainedOrgs = _model.Organizations.Select(x => x.ID).ToArray();
		    var targetsToDel = _dbContext.CompetitiveGroupTarget
		                                 .Where(x =>
		                                        !x.CompetitiveGroupTargetItem.Any() &&
		                                        !x.ApplicationSelectedCompetitiveGroupTarget.Any() &&
		                                        !x.Application.Any() &&
		                                        !remainedOrgs.Contains(x.CompetitiveGroupTargetID))
		                                 .ToList();

		    targetsToDel.ForEach(_dbContext.CompetitiveGroupTarget.DeleteObject);

			var targetItemsUIDs = _dbContext.CompetitiveGroupTargetItem
				.Where(x => x.CompetitiveGroup.InstitutionID == _institutionID && x.CompetitiveGroupTarget.UID != null
				                && x.CompetitiveGroupID != _model.GroupID)
				.Select(x => x.CompetitiveGroupTarget.UID).ToList();

			// создаём организации ЦП
			foreach (var organization in _model.Organizations)
			{
				res = CreateTargetOrg(organization);
				if (res != null) return res;
			}

			Func<CompetitiveGroupItem, string> getErrorKey = (item) => item.CompetitiveGroup.DirectionID + "@" + item.CompetitiveGroup.EducationLevelID;
			Dictionary<string, string> errors = new Dictionary<string, string>();
			Dictionary<string, List<int>> errorIdxes = new Dictionary<string, List<int>>();
			Dictionary<string, string> targetUIDErrors = new Dictionary<string, string>();
			Dictionary<string, List<int>> targetUIDErrorsIdxes = new Dictionary<string, List<int>>();

			var groupItemsUIDs =
				_dbContext.CompetitiveGroupItem.Where(x => x.CompetitiveGroup.InstitutionID == _institutionID && x.CompetitiveGroup.UID != null && _model.GroupID != x.CompetitiveGroupID)
				.Select(x => new { x.CompetitiveGroup.DirectionID, x.CompetitiveGroup.UID, x.CompetitiveGroup.EducationLevelID, x.CompetitiveGroupID })
				.ToArray()
				.Where(x => !_model.Rows.Where(y => y.DirectionID == x.DirectionID && y.EducationLevelID == x.EducationLevelID && _model.GroupID == x.CompetitiveGroupID).Any())
				.Select(x => x.UID).ToList();
		    res = ValidateEdLevelsFilterType(_model.Rows.Select(x => x.EducationLevelID).ToArray());
            if (res != null) return res;
			// заполняем направления и направления ЦП или ошибки
			foreach (var row in _model.Rows)
			{
				if (row.Data.Length != 9 + (_createdTargets.Count * 3))
					return new AjaxResultModel(AjaxResultModel.DataError);
				CompetitiveGroupItem item = existingGroupItems
					.FirstOrDefault(x => x.CompetitiveGroup.DirectionID == row.DirectionID && x.CompetitiveGroup.EducationLevelID == row.EducationLevelID);
				bool isItemFound = item != null;
				if (!isItemFound)
				{
					item = new CompetitiveGroupItem
					{
						CompetitiveGroup = dbGroup,
						//DirectionID = row.DirectionID,
						//EducationLevelID = (short)row.EducationLevelID,
						//Direction = _dbContext.Direction.Single(x => x.DirectionID == row.DirectionID)
					};
				}
					
				item.NumberBudgetO = row.Data[0];
				item.NumberBudgetOZ = row.Data[1];
				item.NumberBudgetZ = row.Data[2];
                item.NumberQuotaO = row.Data[3];
                item.NumberQuotaOZ = row.Data[4];
                item.NumberQuotaZ = row.Data[5];
                item.NumberPaidO = row.Data[6];
				item.NumberPaidOZ = row.Data[7];
				item.NumberPaidZ = row.Data[8];
				item.CompetitiveGroup.UID = String.IsNullOrEmpty(row.UID) ? null : row.UID;
				if (item.CompetitiveGroup.UID != null)
				{
					if (groupItemsUIDs.Contains(item.CompetitiveGroup.UID))
					{
						errors[getErrorKey(item)] = "UID уже существует";
						errorIdxes[getErrorKey(item)] = new List<int> { -1 };
					}
					else groupItemsUIDs.Add(item.CompetitiveGroup.UID);
				}

				if (!isItemFound)
					_dbContext.CompetitiveGroupItem.AddObject(item);
				else
					existingGroupItems.Remove(item); //удаляем из списка, больше не пригодится
				int targetSummO = 0;
				int targetSummOZ = 0;
				int targetSummZ = 0;
				for (int i = 9; i < row.Data.Length; i += 3)
				{
                    // в CompetitiveGroupTargetItem нет поля UID и нет связи с CompetitiveGroupItem
                    CompetitiveGroupTargetItem targetItem = new CompetitiveGroupTargetItem
					{
						CompetitiveGroupTarget = _createdTargets[(i - 9) / 3],
						//CompetitiveGroupItem = item,
						NumberTargetO = row.Data[i],
						NumberTargetOZ = row.Data[i + 1],
						NumberTargetZ = row.Data[i + 2],
						//UID = String.IsNullOrEmpty(row.DataTargetUIDs[(i - 9) / 3]) ? null : row.DataTargetUIDs[(i - 9) / 3]
					};
					//if (targetItem.UID != null)
					//{
					//	if (targetItemsUIDs.Contains(targetItem.UID))
					//	{
					//		targetUIDErrors[getErrorKey(item)] = "UID дублируется";
					//		if (!targetUIDErrorsIdxes.ContainsKey(getErrorKey(item)))
					//			targetUIDErrorsIdxes[getErrorKey(item)] = new List<int>();
					//		targetUIDErrorsIdxes[getErrorKey(item)].Add((i - 9) / 3);
					//	}
					//	else
					//		targetItemsUIDs.Add(targetItem.UID);
					//}

					targetSummO += row.Data[i];
					targetSummOZ += row.Data[i + 1];
					targetSummZ += row.Data[i + 2];
					_dbContext.CompetitiveGroupTargetItem.AddObject(targetItem);
				}

				List<int> errorIdx = new List<int>();
				// проверяем количество
				string error = ValidateCompetitiveGroupDirectionCount(_dbContext, _institutionID, item, targetSummO, targetSummOZ, targetSummZ, ref errorIdx);
				if (error != null)
				{
					errors[getErrorKey(item)] = error;
					errorIdxes[getErrorKey(item)] = errorIdx;
				}
			}
			//удаляем оставшиеся элементы, которые есть в базе но удалил клиент
			existingGroupItems.ForEach(_dbContext.CompetitiveGroupItem.DeleteObject);
			if (errors.Count > 0 || targetUIDErrors.Count > 0)
				return new AjaxResultModel("df") 
				{
					Data = new
					{
						Errors = errors.Select(x => new { DirectionID = x.Key.Split('@')[0], EducationLevelID = x.Key.Split('@')[1], Error = x.Value, ErrorIdx = errorIdxes[x.Key].ToArray() }).ToArray(),
						TargetUIDErrors = targetUIDErrors.Select(x => new { DirectionID = x.Key.Split('@')[0], EducationLevelID = x.Key.Split('@')[1], Error = x.Value, ErrorIdx = targetUIDErrorsIdxes[x.Key].ToArray() }).ToArray()
					}
				};
			try
			{
				_dbContext.SaveChanges();

                targetsToDel = _dbContext.CompetitiveGroupTarget
                                         .Where(x =>
                                                !x.CompetitiveGroupTargetItem.Any() &&
                                                !x.ApplicationSelectedCompetitiveGroupTarget.Any() &&
                                                !x.Application.Any() &&
                                                !remainedOrgs.Contains(x.CompetitiveGroupTargetID))
                                         .ToList();

                targetsToDel.ForEach(_dbContext.CompetitiveGroupTarget.DeleteObject);

			    _dbContext.SaveChanges();
				// для первого курса изначально добавляем ВИ
				if (dbGroup.Course == 1) 
					_dbContext.InitialFillEntranceTest(dbGroup.CompetitiveGroupID, _institutionID);
			}
			catch (Exception ex) // если не сохранилось, пишем соответствующие известные ошибки из базы
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("FK_Application_CompetitiveGroupItem"))
					return new AjaxResultModel("Невозможно сохранить данные, т.к. есть заявления на удалённом направлении");

                if (inner != null && inner.Message.Contains("FK_ApplicationCompetitiveGroupItem_CompetitiveGroupItem"))
                    return new AjaxResultModel("Невозможно удалить конкурс, так как есть привязанные к ней заявления");

				if (inner != null && inner.Message.Contains("UK_CompetitiveGroup_UniqueInstitutionName"))
					return new AjaxResultModel("Невозможно переименовать конкурс. Уже существует конкурс с данным названием")
					{
						Data = new[]
						{
							new
							{
								ControlID = "Name",
								ErrorMessage = "Уже существует группа с данным названием"
							}
						}
					};
				throw;
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// Проверяем корректность выбранного типа направлений
		/// </summary>
		private AjaxResultModel ValidateDirectionFilterType(int[] directionsToInsert)
		{
			if (_model.DirectionFilterType == 0 || _model.DirectionFilterType == 1) //not exact or exact
			{
				if (directionsToInsert.Length > 0 &&
				    _dbContext.GetAvailableLinksWithSameEntranceTests(directionsToInsert, _model.DirectionFilterType == 1).Length == 0)
				{
					return new AjaxResultModel("При текущей стратегии выбора направлений подготовки невозможность использовать данную комбинацию направлений подготовки. Необходимо установить переключатель в положение \"Любые направления подготовки\". ");
				}
			}

			if (_model.DirectionFilterType == 0) //not exact
			{
				if (
					_dbContext.EntranceTestItemC.Where(
						x => x.CompetitiveGroupID == _model.GroupID && x.EntranceTestTypeID == EntranceTestType.MainType).Count() > 4)
				{
						return new AjaxResultModel(
							"При текущей стратегии выбора специальностей разрешено только 4 основных вступительных испытания. Перед сохранением необходимо удалить лишние.");
				}
			}

			if (_model.DirectionFilterType == 2)
			{
				//теперь можно любые профильные, проверка не нужна
				//if(context.EntranceTestItemC.Where(x => x.CompetitiveGroupID == model.GroupID && x.EntranceTestTypeID == EntranceTestType.ProfileType).Any())
				//	return new AjaxResultModel("Существует испытание профильной направленности, удалите его перед использованием стратегии \"Любые специальности\".");
			}

			if (_model.DirectionFilterType == 1) //exact
			{
				if (_dbContext.EntranceTestItemC.Where(x => x.EntranceTestTypeID == EntranceTestType.ProfileType 
							   && x.CompetitiveGroupID == _model.GroupID).Count() > 1)
					return new AjaxResultModel(
							"При текущей стратегии выбора специальностей разрешено только 1 испытание профильной направленности. Перед сохранением необходимо удалить лишние.");
			}

			return null;
		}
        /// <summary>
        /// Проверяем корректность выбранных уровней образования
        /// </summary>
        private AjaxResultModel ValidateEdLevelsFilterType(int[] edLevelsToInsert)
        {
            //Если в КГ уже есть какой-нибудь уровень образования, то добавлять можно только такой же
                //исключение - бакалавриат и бакалавриат сокращенный
                //http://redmine.armd.ru/issues/20835
            if (edLevelsToInsert.Distinct().Count() > 1 &&
                edLevelsToInsert.Any(x => !(new List<int>{2,3}).Contains(x)))
//                return new AjaxResultModel("Прием в высшие учебные заведения должен осуществляться отдельно по программам бакалавриата, программам подготовки специалиста, программам магистратуры и программам СПО.");
                return new AjaxResultModel("Прием на обучение должен осуществляться раздельно по программам бакалавриата, прикладного бакалавриата, специалитета, магистратуры, программам СПО и программам подготовки кадров высшей квалификации");
            return null;
        }
		private readonly List<CompetitiveGroupTarget> _createdTargets = new List<CompetitiveGroupTarget>();

		/// <summary>
		/// Создаём организации ЦП
		/// </summary>
		private AjaxResultModel CreateTargetOrg(CompetitiveGroupEditViewModel.OrganizationData organization)
		{
			//если по каким-то причинам на клиенте не отработало
			if (organization == null || organization.Name == null || organization.Name.Length > 250)
				return new AjaxResultModel("Некорректное название организации целевого приема: " + (organization != null ? organization.Name ?? "" : ""));

			CompetitiveGroupTarget targetByID =
				_dbContext.CompetitiveGroupTarget.Where(
					x => x.InstitutionID == _institutionID && x.CompetitiveGroupTargetID == organization.ID)
					.FirstOrDefault();
			CompetitiveGroupTarget targetByName =
				_dbContext.CompetitiveGroupTarget.Where(
					x => x.InstitutionID == _institutionID && x.Name == organization.Name)
					.FirstOrDefault();

			CompetitiveGroupTarget targetByUID =
				_dbContext.CompetitiveGroupTarget.Where(
					x => x.InstitutionID == _institutionID && x.UID == organization.UID)
					.FirstOrDefault();

			if (targetByName != null)
			{
				if (targetByName.UID != organization.UID && targetByName.CompetitiveGroupTargetID != organization.ID)
					return new AjaxResultModel("Уже существует организация целевого приема с наименованием " + organization.Name + " но другим UID'ом: " + (targetByName.UID ?? string.Empty));
			}

			if (targetByUID != null)
			{
				if (targetByUID.Name != organization.Name && targetByUID.CompetitiveGroupTargetID != organization.ID)
					return new AjaxResultModel("Уже существует организация целевого приема с UID " + organization.UID + " но другим наименованием: " + (targetByUID.Name ?? string.Empty));
			}
			//сменили организацию
			if (targetByID != null && targetByName != null && targetByID.CompetitiveGroupTargetID != targetByName.CompetitiveGroupTargetID)
			{
				bool hasAnyApp = _dbContext.Application
					.Where(y => y.ApplicationSelectedCompetitiveGroupTarget.Any(z => z.CompetitiveGroupTargetID == targetByID.CompetitiveGroupTargetID)
					&& y.OrderCompetitiveGroupTargetID == _model.GroupID)
					.Any();
				if (hasAnyApp)
				{
					return new AjaxResultModel("Данные организации целевого приема были изменены на другую существующую организацию, но к данной организации уже прикреплены заявления. Изменение невозможно.");
				}

				targetByID = null;
			}

			CompetitiveGroupTarget target = targetByID ?? targetByName;
			if (target == null)
			{
				target = new CompetitiveGroupTarget();
				target.InstitutionID = _institutionID;
				_dbContext.CompetitiveGroupTarget.AddObject(target);
			}

			target.Name = organization.Name;
			target.UID = organization.UID;
			_createdTargets.Add(target);
			return null;
		}

		private static readonly string[] CompetitiveGroupsErrorNames = new[]
		                                                     {
		                                                     	"Очное обучение", "Очно-заочное обучение", "Заочное обучение",
		                                                     	"Очное обучение", "Очно-заочное обучение", "Заочное обучение",
		                                                     	"Очное обучение", "Очно-заочное обучение", "Заочное обучение",
		                                                     	"Очное обучение", "Очно-заочное обучение", "Заочное обучение",
		                                                     };

		/// <summary>
		/// Проверяем количество мест в КГ на минимум и максимум
		/// </summary>
		private static string ValidateCompetitiveGroupDirectionCount(EntrantsEntities dbContext, int institutionID, CompetitiveGroupItem item, int targetSummO, int targetSummOZ, int targetSummZ, ref List<int> errorIdx)
		{
			int[] counts = dbContext.GetCompetitiveGroupAvailableDirectionCountInternal(institutionID, item.CompetitiveGroup.CompetitiveGroupID, item.CompetitiveGroup.DirectionID.Value, item.CompetitiveGroup.EducationLevelID.Value);
			//int[] counts2 = dbContext.GetCompetitiveGroupApplicationInOrderCountInternal(institutionID, item.CompetitiveGroup.CompetitiveGroupID, item.CompetitiveGroupItemID);
			if (counts != null)// && counts2 != null)
			{
				var errorIdxLocal = errorIdx;
				List<string> partErrors = new List<string>();
				Action<int, int> getError = (number, idx) =>
				{
					if (number > counts[idx])
					{
						partErrors.Add(CompetitiveGroupsErrorNames[idx] + ": введено " + number + " доступно " + counts[idx]);
						errorIdxLocal.Add(idx);
					}

/*                  Исключено sgerasimov во исполнение #26373
                    if (number < counts2[idx])
                    {
                        partErrors.Add(CompetitiveGroupsErrorNames[idx] + ": введено " + number + " минимально " + counts2[idx]);
                        errorIdxLocal.Add(idx);
                    }*/
				};
				getError(item.NumberBudgetO.Value, 0);
				getError(item.NumberBudgetOZ.Value, 1);
				getError(item.NumberBudgetZ.Value, 2);
                getError(item.NumberQuotaO.HasValue ? item.NumberQuotaO.Value : 0, 3);
                getError(item.NumberQuotaOZ.HasValue ? item.NumberQuotaOZ.Value : 0, 4);
                getError(item.NumberQuotaZ.HasValue ? item.NumberQuotaZ.Value : 0, 5);
                getError(item.NumberPaidO.Value, 6);
				getError(item.NumberPaidOZ.Value, 7);
				getError(item.NumberPaidZ.Value, 8);
				getError(targetSummO, 9);
				getError(targetSummOZ, 10);
				getError(targetSummZ, 11);
				if (partErrors.Count > 0)
					return String.Join("\r\n  ", partErrors.ToArray());
			}
			else return AjaxResultModel.DataError;
			return null;
		}
	}
}