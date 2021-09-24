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
	/// ���������� �� 
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
		/// ��������� ������� ��������� ��
		/// </summary>
		private AjaxResultModel ValidateGroupBaseProperties(CompetitiveGroup dbGroup)
		{
			if (dbGroup == null)
				return new AjaxResultModel("�� ������ �������");
			if (dbGroup.Campaign != null && dbGroup.Campaign.StatusID == CampaignStatusType.Finished)
				return new AjaxResultModel("���������� ������������� ������� ��� ����������� ��������");
			if (_model.Uid != null && _dbContext.CompetitiveGroup
										 .Where(x => x.InstitutionID == _institutionID
											 && x.CompetitiveGroupID != _model.GroupID && x.UID == _model.Uid).Any())
				return new AjaxResultModel("").SetIsError("Uid", "��� ���������� ������� � ������ UID'��");
			return null;
		}

		/// <summary>
		/// ���������� ��
		/// </summary>
		public AjaxResultModel SaveCompetitiveGroupEditViewModel()
		{
			var dbGroup = LoadGroup();
			// ��������� ������� ���������
			var res = ValidateGroupBaseProperties(dbGroup);
			if (res != null) return res;
			
			// ��������� ������������ ����������� ��
			if (_model.Organizations.Select(x => x.Name.Trim()).Distinct().Count() < _model.Organizations.Length)
				return new AjaxResultModel("�������� ����������� �������� ������ �����������");
			if (_model.Organizations.Where(x => !String.IsNullOrEmpty(x.UID)).Select(x => x.UID.Trim()).Distinct().Count() < 
				_model.Organizations.Where(x => !String.IsNullOrEmpty(x.UID)).Count())
				return new AjaxResultModel("UID'� ����������� �������� ������ �����������");

			dbGroup.Name = _model.Name;
			dbGroup.UID = _model.Uid;
			dbGroup.DirectionFilterType = _model.DirectionFilterType;
			
			var existingGroupItems = _dbContext.CompetitiveGroupItem
				.Include(x => x.CompetitiveGroup.Direction)
				.Where(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID)
				.ToList();
			//������� ������� �����
            _dbContext.CompetitiveGroupTargetItem
                .Where(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID)
                .ToList().ForEach(_dbContext.CompetitiveGroupTargetItem.DeleteObject);

			var directionsToInsert = _model.Rows.Select(x => x.DirectionID).Distinct().ToArray();
			// ��������� ������������ ����������� � ��
			res = ValidateDirectionFilterType(directionsToInsert);
			if (res != null) return res;

			// ���� ��� �����������, �� ���� �� 0 ������
			if (_model.Rows.Length == 0)
			{
				int entranceTests = _dbContext.EntranceTestItemC.Count(x => x.CompetitiveGroupID == dbGroup.CompetitiveGroupID);
				if (entranceTests > 0)
					return new AjaxResultModel("���������� ��������� �������, ��� ��� ���������� ������������� ���������, �� ����������� �����������");
			}

			//������� ��� ��������
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

			// ������ ����������� ��
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
			// ��������� ����������� � ����������� �� ��� ������
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
						errors[getErrorKey(item)] = "UID ��� ����������";
						errorIdxes[getErrorKey(item)] = new List<int> { -1 };
					}
					else groupItemsUIDs.Add(item.CompetitiveGroup.UID);
				}

				if (!isItemFound)
					_dbContext.CompetitiveGroupItem.AddObject(item);
				else
					existingGroupItems.Remove(item); //������� �� ������, ������ �� ����������
				int targetSummO = 0;
				int targetSummOZ = 0;
				int targetSummZ = 0;
				for (int i = 9; i < row.Data.Length; i += 3)
				{
                    // � CompetitiveGroupTargetItem ��� ���� UID � ��� ����� � CompetitiveGroupItem
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
					//		targetUIDErrors[getErrorKey(item)] = "UID �����������";
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
				// ��������� ����������
				string error = ValidateCompetitiveGroupDirectionCount(_dbContext, _institutionID, item, targetSummO, targetSummOZ, targetSummZ, ref errorIdx);
				if (error != null)
				{
					errors[getErrorKey(item)] = error;
					errorIdxes[getErrorKey(item)] = errorIdx;
				}
			}
			//������� ���������� ��������, ������� ���� � ���� �� ������ ������
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
				// ��� ������� ����� ���������� ��������� ��
				if (dbGroup.Course == 1) 
					_dbContext.InitialFillEntranceTest(dbGroup.CompetitiveGroupID, _institutionID);
			}
			catch (Exception ex) // ���� �� �����������, ����� ��������������� ��������� ������ �� ����
			{
				var inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("FK_Application_CompetitiveGroupItem"))
					return new AjaxResultModel("���������� ��������� ������, �.�. ���� ��������� �� �������� �����������");

                if (inner != null && inner.Message.Contains("FK_ApplicationCompetitiveGroupItem_CompetitiveGroupItem"))
                    return new AjaxResultModel("���������� ������� �������, ��� ��� ���� ����������� � ��� ���������");

				if (inner != null && inner.Message.Contains("UK_CompetitiveGroup_UniqueInstitutionName"))
					return new AjaxResultModel("���������� ������������� �������. ��� ���������� ������� � ������ ���������")
					{
						Data = new[]
						{
							new
							{
								ControlID = "Name",
								ErrorMessage = "��� ���������� ������ � ������ ���������"
							}
						}
					};
				throw;
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// ��������� ������������ ���������� ���� �����������
		/// </summary>
		private AjaxResultModel ValidateDirectionFilterType(int[] directionsToInsert)
		{
			if (_model.DirectionFilterType == 0 || _model.DirectionFilterType == 1) //not exact or exact
			{
				if (directionsToInsert.Length > 0 &&
				    _dbContext.GetAvailableLinksWithSameEntranceTests(directionsToInsert, _model.DirectionFilterType == 1).Length == 0)
				{
					return new AjaxResultModel("��� ������� ��������� ������ ����������� ���������� ������������� ������������ ������ ���������� ����������� ����������. ���������� ���������� ������������� � ��������� \"����� ����������� ����������\". ");
				}
			}

			if (_model.DirectionFilterType == 0) //not exact
			{
				if (
					_dbContext.EntranceTestItemC.Where(
						x => x.CompetitiveGroupID == _model.GroupID && x.EntranceTestTypeID == EntranceTestType.MainType).Count() > 4)
				{
						return new AjaxResultModel(
							"��� ������� ��������� ������ �������������� ��������� ������ 4 �������� ������������� ���������. ����� ����������� ���������� ������� ������.");
				}
			}

			if (_model.DirectionFilterType == 2)
			{
				//������ ����� ����� ����������, �������� �� �����
				//if(context.EntranceTestItemC.Where(x => x.CompetitiveGroupID == model.GroupID && x.EntranceTestTypeID == EntranceTestType.ProfileType).Any())
				//	return new AjaxResultModel("���������� ��������� ���������� ��������������, ������� ��� ����� �������������� ��������� \"����� �������������\".");
			}

			if (_model.DirectionFilterType == 1) //exact
			{
				if (_dbContext.EntranceTestItemC.Where(x => x.EntranceTestTypeID == EntranceTestType.ProfileType 
							   && x.CompetitiveGroupID == _model.GroupID).Count() > 1)
					return new AjaxResultModel(
							"��� ������� ��������� ������ �������������� ��������� ������ 1 ��������� ���������� ��������������. ����� ����������� ���������� ������� ������.");
			}

			return null;
		}
        /// <summary>
        /// ��������� ������������ ��������� ������� �����������
        /// </summary>
        private AjaxResultModel ValidateEdLevelsFilterType(int[] edLevelsToInsert)
        {
            //���� � �� ��� ���� �����-������ ������� �����������, �� ��������� ����� ������ ����� ��
                //���������� - ����������� � ����������� �����������
                //http://redmine.armd.ru/issues/20835
            if (edLevelsToInsert.Distinct().Count() > 1 &&
                edLevelsToInsert.Any(x => !(new List<int>{2,3}).Contains(x)))
//                return new AjaxResultModel("����� � ������ ������� ��������� ������ �������������� �������� �� ���������� ������������, ���������� ���������� �����������, ���������� ������������ � ���������� ���.");
                return new AjaxResultModel("����� �� �������� ������ �������������� ��������� �� ���������� ������������, ����������� ������������, ������������, ������������, ���������� ��� � ���������� ���������� ������ ������ ������������");
            return null;
        }
		private readonly List<CompetitiveGroupTarget> _createdTargets = new List<CompetitiveGroupTarget>();

		/// <summary>
		/// ������ ����������� ��
		/// </summary>
		private AjaxResultModel CreateTargetOrg(CompetitiveGroupEditViewModel.OrganizationData organization)
		{
			//���� �� �����-�� �������� �� ������� �� ����������
			if (organization == null || organization.Name == null || organization.Name.Length > 250)
				return new AjaxResultModel("������������ �������� ����������� �������� ������: " + (organization != null ? organization.Name ?? "" : ""));

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
					return new AjaxResultModel("��� ���������� ����������� �������� ������ � ������������� " + organization.Name + " �� ������ UID'��: " + (targetByName.UID ?? string.Empty));
			}

			if (targetByUID != null)
			{
				if (targetByUID.Name != organization.Name && targetByUID.CompetitiveGroupTargetID != organization.ID)
					return new AjaxResultModel("��� ���������� ����������� �������� ������ � UID " + organization.UID + " �� ������ �������������: " + (targetByUID.Name ?? string.Empty));
			}
			//������� �����������
			if (targetByID != null && targetByName != null && targetByID.CompetitiveGroupTargetID != targetByName.CompetitiveGroupTargetID)
			{
				bool hasAnyApp = _dbContext.Application
					.Where(y => y.ApplicationSelectedCompetitiveGroupTarget.Any(z => z.CompetitiveGroupTargetID == targetByID.CompetitiveGroupTargetID)
					&& y.OrderCompetitiveGroupTargetID == _model.GroupID)
					.Any();
				if (hasAnyApp)
				{
					return new AjaxResultModel("������ ����������� �������� ������ ���� �������� �� ������ ������������ �����������, �� � ������ ����������� ��� ����������� ���������. ��������� ����������.");
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
		                                                     	"����� ��������", "����-������� ��������", "������� ��������",
		                                                     	"����� ��������", "����-������� ��������", "������� ��������",
		                                                     	"����� ��������", "����-������� ��������", "������� ��������",
		                                                     	"����� ��������", "����-������� ��������", "������� ��������",
		                                                     };

		/// <summary>
		/// ��������� ���������� ���� � �� �� ������� � ��������
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
						partErrors.Add(CompetitiveGroupsErrorNames[idx] + ": ������� " + number + " �������� " + counts[idx]);
						errorIdxLocal.Add(idx);
					}

/*                  ��������� sgerasimov �� ���������� #26373
                    if (number < counts2[idx])
                    {
                        partErrors.Add(CompetitiveGroupsErrorNames[idx] + ": ������� " + number + " ���������� " + counts2[idx]);
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