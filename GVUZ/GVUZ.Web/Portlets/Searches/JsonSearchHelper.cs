using System.Collections.Generic;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;
using System.Linq;

namespace GVUZ.Web.Portlets.Searches
{
	[AutoMapping(Source = typeof(InstitutionSearchFields), Destination = typeof(JsonInstitutionSearchParameters))]
	public class JsonInstitutionSearchParameters : InstitutionSearchParameters<JsonInstitutionSearchResult>, IAutoMapping
	{

		public override ConvertResults Convert
		{
			get
			{
				if (base.Convert == null)
					return (items, parameters) =>
						JsonSearchHelper.GetSearchDictionaries(items, (JsonInstitutionSearchParameters)parameters);
				return base.Convert;
			}
			set { base.Convert = value; }
		}

		public void CreateMap(IConfiguration config)
		{
			config.CreateMap<InstitutionSearchFields, JsonInstitutionSearchParameters>()
				.ForMember(x => x.ParentStructureID, o => o.Ignore())
				.ForMember(x => x.DepthLimit, o => o.Ignore());
		}
	}

	public static class JsonSearchHelper
	{
		public static JsonInstitutionSearchResult GetSearchDictionaries
			(IEnumerable<InstitutionSearchResult> items,
			JsonInstitutionSearchParameters parameters)
		{
			JsonInstitutionSearchResult result = new JsonInstitutionSearchResult();

			List<InstitutionSearchTreeItemViewModel> childrenList = new List<InstitutionSearchTreeItemViewModel>();
			int currentParentId = 0;
			
			result.Children.Add("0", childrenList);

			foreach (var item in items)
			{
				int itemId = item.AdmissionStructureID;
				InstitutionSearchTreeItemViewModel viewModel =
					new InstitutionSearchTreeItemViewModel
						{
							ItemID = itemId,				
							Name = item.Name,
							PlaceCount = item.PlaceCount,
							ApplicationCount = item.ApplicationCount,
							Competition = item.Competition,
							HasMilitaryDepartment = item.HasMilitaryDepartment,
							HasOlympics = item.HasOlympics,
							HasPreparatoryCourses = item.HasPreparatoryCourses,
							Applicable = item.Applicable,
							CanBeChecked = item.CanBeChecked,
							ApplicationStatus = item.ApplicationStatus,
							ApplicationStatusID = item.ApplicationStatusID,
							ApplicationID = item.ApplicationID,
							EntrantApplicationCount = item.EntrantApplicationCount,
							IsLeaf = item.IsLeaf ?? false,
							CanClick = item.ItemLevel == (short)AdmissionItemLevel.Institution,
							NodeUrl = PortletLinkHelper.InstitutionLink(item.InstitutionID.ToString())
						};
				
				result.Objects.Add(itemId.ToString(), viewModel.GetObject());

				int parentId = item.ParentID ?? 0;

				if (currentParentId != parentId)
				{
					// TODO: попробовать передавать в процедуру параметр, чтобы сортировало сразу там и сделать List<int> childrenList (Eventually)
					ConvertAndSortChildren(result, currentParentId, childrenList);
					
					currentParentId = parentId;
					string parent = parentId.ToString();
						
					childrenList = new List<InstitutionSearchTreeItemViewModel>();
					result.Children.Add(parent, childrenList);
					// если считываем все - нужно проанализировать возможность раскрытия
					if (!parameters.DepthLimit.HasValue)
					{
						((InstitutionSearchTreeItemViewModel.ObjectWithArray)
							result.Objects[parent]).IsLeaf = false;
					}
				}

// ReSharper disable PossibleNullReferenceException
				childrenList.Add(viewModel);
// ReSharper restore PossibleNullReferenceException
			}
			
			ConvertAndSortChildren(result, currentParentId, childrenList);
			return result;
		}

		private static void ConvertAndSortChildren(JsonInstitutionSearchResult result, int currentParentId,
			IEnumerable<InstitutionSearchTreeItemViewModel> childrenList)
		{
			result.Children[currentParentId.ToString()] =
				childrenList.OrderBy(x => x.Name).Select(x => x.ItemID).ToArray();
		}
	}
}