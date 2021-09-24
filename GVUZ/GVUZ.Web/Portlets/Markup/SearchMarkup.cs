using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Web.Controllers;
using GVUZ.Web.Portlets.Searches;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Web.Portlets.Markup
{
    public class SearchMarkup : CommonMarkup
    {
        public SearchMarkup(UserInfo currentUser) : base(currentUser)
        {
        }

        public void ClearCheckboxes(string interactionState)
        {
            //TODO: поменять чекбоксы в информере на PortletCheckboxFor и убрать зачистку для них (Eventually)
            var session = ServiceLocator.Current.GetInstance<ISession>();
            if (interactionState == PortletType.Search)
            {
                session.SetValue("VuzCheckDirection", false);
                session.SetValue("SsuzCheckDirection", false);
                session.SetValue("VuzCheckInstitution", false);
                session.SetValue("SsuzCheckInstitution", false);
            }
        }

        public string InstitutionSearchAjax(string interactionState, string searchString)
        {
            var session = ServiceLocator.Current.GetInstance<ISession>();

            if (interactionState == PortletType.InstitutionSearchByName)
                return String.Join(",", SearchService.SearchInstitutionByName(searchString));

            if (interactionState == PortletType.InstitutionDirectionSearch)
                return String.Join(",", SearchService.SearchDirections(searchString));

            if (interactionState == PortletType.InstitutionDirectionCodeSearch)
                return String.Join(",", SearchService.SearchDirectionCodes(searchString));

            if (interactionState == PortletType.InstitutionRegionSearch)
                return String.Join(",", SearchService.SearchRegions(searchString));

            var cwe = new ControllerWrapperExecutor();
            if (interactionState == PortletType.SaveCurrentApplicationID)
                return cwe.DoViewControllerAction<ApplicationController>(x => x.SaveSelectedApplicationID(0),
                                                                         session.GetValue("viewAppID", 0));

            if (interactionState == PortletType.SaveStructureItemID)
                return cwe.DoViewControllerAction<ApplicationController>(x => x.SaveStructureItemID(null),
                                                                         session.GetValue(
                                                                             ApplicationController
                                                                                 .StructureItemSessionKey, ""));

            return "";
        }

        /// <summary>
        ///     Страница поиска образовательных учреждений
        /// </summary>
        /// <param name="informerSearch">html для информера</param>
        /// <param name="search">html для поиска</param>
        public string GetSearchPage(string informerSearch, string search)
        {
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/Searches/SearchPortlet",
                new SearchPortletViewModel
                    {
                        InformerSearch = informerSearch,
                        Search = search
                    });
            return itemString;
        }

        /// <summary>
        ///     Страница поиска образовательных учреждений без информера
        /// </summary>
        /// <param name="search">html для поиска</param>
        public string GetSearchPage(string search)
        {
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/Searches/SearchPortlet",
                new SearchPortletViewModel
                    {
                        Search = search
                    });
            return itemString;
        }

        /// <summary>
        ///     Страница поиска образовательных учреждений без информера
        /// </summary>
        public string SearchPage(string interactionState, ref string mimeType)
        {
            if (interactionState == PortletType.Search || interactionState == PortletType.SearchPage)
            {
                return new ControllerWrapper().RenderView(
                    "Portlets/Searches/SearchPortlet",
                    new SearchPortletViewModel
                        {
                            Search = SearchBlock()
                        });
            }
            mimeType = "application/json";
            var session = ServiceLocator.Current.GetInstance<ISession>();

            return InstitutionSearchAjax(interactionState, session.GetValue("q", ""));
        }

        private static InstitutionSearchFields GetSearchFields(out bool isSearch)
        {
            var session = ServiceLocator.Current.GetInstance<ISession>();

            var searchFields = new InstitutionSearchFields();
            bool isSearchNormal = (session.GetValue(SearchType.Normal, "") != "");
            bool isSearchAdvanced = (session.GetValue(SearchType.Advanced, "") != "");
            bool isSearchCommon = (isSearchNormal || isSearchAdvanced);

            bool isSearchInformerDirection = (session.GetValue(SearchType.InformerDirection, "") != "");
            bool isSearchInformerInstitution = (session.GetValue(SearchType.InformerInstitution, "") != "");
            bool isSearchInformer = (isSearchInformerDirection || isSearchInformerInstitution);

            isSearch = (isSearchCommon || isSearchInformer);

            //определим чекбоксы
            if (isSearchInformer)
            {
                if (isSearchInformerDirection)
                {
                    searchFields.VuzCheckDirection = session.GetValue("VuzCheckDirection", false);
                    searchFields.SsuzCheckDirection = session.GetValue("SsuzCheckDirection", false);
                    searchFields.VuzCheckInstitution = true;
                    searchFields.SsuzCheckInstitution = true;
                    searchFields.IsVUZ = searchFields.VuzCheckDirection;
                    searchFields.IsSSUZ = searchFields.SsuzCheckDirection;
                }
                else
                {
                    searchFields.VuzCheckDirection = true;
                    searchFields.SsuzCheckDirection = true;
                    searchFields.VuzCheckInstitution = session.GetValue("VuzCheckInstitution", false);
                    searchFields.SsuzCheckInstitution = session.GetValue("SsuzCheckInstitution", false);
                    searchFields.IsVUZ = searchFields.VuzCheckInstitution;
                    searchFields.IsSSUZ = searchFields.SsuzCheckInstitution;
                }
                // перезапишем в сессию значения чекбоксов
                session.SetValue("VuzCheckDirection", searchFields.VuzCheckDirection);
                session.SetValue("SsuzCheckDirection", searchFields.SsuzCheckDirection);
                session.SetValue("VuzCheckInstitution", searchFields.VuzCheckInstitution);
                session.SetValue("SsuzCheckInstitution", searchFields.SsuzCheckInstitution);
            }
            else
            {
                searchFields.IsVUZ = session.GetValue("VuzCheck", false);
                searchFields.IsSSUZ = session.GetValue("SsuzCheck", false);
                session.SetValue("VuzCheck", searchFields.IsVUZ);
                session.SetValue("SsuzCheck", searchFields.IsSSUZ);
            }

            searchFields.NamePart = session.GetValue("InstitutionName", Search.NamePart);
            searchFields.DirectionName = session.GetValue("DirectionName", Search.DirectionName);
            searchFields.DirectionCode = session.GetValue("DirectionCode", Search.DirectionCode);
            searchFields.City = session.GetValue("City", Search.City);
            searchFields.RegionName = session.GetValue("Region", Search.RegionName);
            searchFields.StudyID = session.GetValue("StudyFormId", (short) 0);
            searchFields.FormOfLawID = session.GetValue("FormOfLawID", (short) 0);
            searchFields.EducationLevelID = session.GetValue("EducationLevelId", (short) 0);
            searchFields.AdmissionTypeId = session.GetValue("AdmissionTypeId", (short) 0);
            searchFields.Military = session.GetValue("MilitaryCheckValue", "unselected");
            searchFields.Courses = session.GetValue("CoursesCheckValue", "unselected");
            searchFields.Olympics = session.GetValue("OlympicsCheckValue", "unselected");
            searchFields.AdvancedSearch = session.GetValue("AdvancedSearch", "none");

            searchFields.PageNumber = session.GetValue("PageNumber", 1);

            return searchFields;
        }

        /// <summary>
        ///     Поиск образовательных учреждений - расширенная форма
        /// </summary>
        public string SearchBlock()
        {
            bool isSearch;
            string validationMessage;
            InstitutionSearchFields searchFields = GetSearchFields(out isSearch);
            bool isValidSearch = IsValidSearch(isSearch, searchFields, out validationMessage);
            string itemString = GetSearchFormString(searchFields, validationMessage);
            if (isValidSearch)
                itemString += GetSearchResultString(searchFields);
            return itemString;
        }

        [Obsolete]
        private string GetSearchResultString(InstitutionSearchFields searchFields)
        {
            JsonInstitutionSearchParameters searchParameters = GetSearchParameters(searchFields);
            using (var entities = new InstitutionsEntities())
            {
                int? totalPageCount;
                totalPageCount = 0;
                //var treeResult = entities.SearchInstitutions(searchParameters, out totalPageCount);
                return new ControllerWrapper().RenderView(
                    "Portlets/Searches/SearchResultTree",
                    new SearchResultTreeViewModel
                        {
                            ResultCount = totalPageCount.GetValueOrDefault(0),
                            CurrentPage = searchParameters.PageNumber.GetValueOrDefault(0),
                            TreeResult = null
                        });
            }
        }

        private JsonInstitutionSearchParameters GetSearchParameters(InstitutionSearchFields searchFields)
        {
            JsonInstitutionSearchParameters searchParameters =
                Mapper.Map<InstitutionSearchFields, JsonInstitutionSearchParameters>(searchFields);

            var session = ServiceLocator.Current.GetInstance<ISession>();

            if (searchParameters.NamePart == Search.NamePart)
                searchParameters.NamePart = "";

            if (searchParameters.DirectionName == Search.DirectionName)
                searchParameters.DirectionName = "";

            if (searchParameters.DirectionCode == Search.DirectionCode)
                searchParameters.DirectionCode = "";

            if (searchParameters.RegionName == Search.RegionName)
                searchParameters.RegionName = "";

            /*if (searchParameters.City == Search.City)
				searchParameters.City = "";*/

            //TODO: сделать проверки при выборе из автокомплита (Eventually)
            bool namePartIsFull = session.GetValue("InstitutionNameIsFull", false);
            bool directionIsFull = session.GetValue("DirectionNameIsFull", false);
            bool directionCodeIsFull = session.GetValue("DirectionCodeIsFull", false);
            bool regionIsFull = session.GetValue("RegionIsFull", false);

            if (!namePartIsFull && searchParameters.NamePart != "")
                searchParameters.NamePart = "%" + searchParameters.NamePart + "%";
            if (!directionIsFull && searchParameters.DirectionName != "")
                searchParameters.DirectionName = "%" + searchParameters.DirectionName + "%";
            if (!directionCodeIsFull && searchParameters.DirectionCode != "")
                searchParameters.DirectionCode = "%" + searchParameters.DirectionCode + "%";
            if (!regionIsFull && searchParameters.RegionName != "")
                searchParameters.RegionName = "%" + searchParameters.RegionName + "%";

            searchParameters.PageSize = AppSettings.Get("Search.PageSize", 25);
            searchParameters.Snils = CurrentUser.SNILS;
            return searchParameters;
        }

        private static bool IsValidSearch(bool isSearch, InstitutionSearchFields searchFields,
                                          out string validationMessage)
        {
            validationMessage = "";
            if (isSearch)
            {
                if (!searchFields.IsVUZ && !searchFields.IsSSUZ)
                {
                    validationMessage = "Выберите хотя бы один тип образовательного учреждения.";
                }
            }
            else
            {
                searchFields.IsVUZ = true;
                searchFields.IsSSUZ = true;
            }
            return (isSearch && validationMessage == "");
        }

        private static string GetSearchFormString(InstitutionSearchFields searchFields, string validationMessage)
        {
            // списки для dropdown-ов
            List<FormOfLaw> formOfLawList;
            List<AdmissionItemType> educationLevelList;
            List<AdmissionItemType> studyFormList;
            List<AdmissionItemType> admissionTypeList;
            using (var context = new InstitutionsEntities())
            {
                formOfLawList = context.FormOfLaw.ToList();
                formOfLawList.Insert(0, new FormOfLaw {FormOfLawID = 0, Name = "Все"});

                educationLevelList = context.GetAdmissionItemTypes(AdmissionItemLevel.EducationLevel);
                educationLevelList.Insert(0, new AdmissionItemType {ItemTypeID = 0, Name = "Все"});

                studyFormList = context.GetAdmissionItemTypes(AdmissionItemLevel.Study);
                studyFormList.Insert(0, new AdmissionItemType {ItemTypeID = 0, Name = "Все"});

                admissionTypeList = context.GetAdmissionItemTypes(AdmissionItemLevel.AdmissionType);
                admissionTypeList.Insert(0, new AdmissionItemType {ItemTypeID = 0, Name = "Все"});
            }

            return new ControllerWrapper().RenderView(
                "Portlets/Searches/SearchInstitution",
                new SearchInstitutionViewModel
                    {
                        InstitutionName = searchFields.NamePart,
                        VuzCheck = searchFields.IsVUZ,
                        SsuzCheck = searchFields.IsSSUZ,
                        DirectionName = searchFields.DirectionName,
                        DirectionCode = searchFields.DirectionCode,
                        City = searchFields.City,
                        Region = searchFields.RegionName,
                        StudyFormId = searchFields.StudyID,
                        FormOfLawID = searchFields.FormOfLawID,
                        EducationLevelId = searchFields.EducationLevelID,
                        AdmissionTypeId = searchFields.AdmissionTypeId,
                        MilitaryCheckValue = searchFields.Military,
                        CoursesCheckValue = searchFields.Courses,
                        OlympicsCheckValue = searchFields.Olympics,
                        StudyFormList = studyFormList,
                        FormOfLawList = formOfLawList,
                        EducationLevelList = educationLevelList,
                        AdmissionTypeList = admissionTypeList,
                        ValidationMessage = validationMessage,
                        AdvancedSearch = searchFields.AdvancedSearch,
                        PageNumber = 1
                    });
        }

        /// <summary>
        ///     Поиск образовательных учреждений - информер
        /// </summary>
        public string SearchInformerBlock()
        {
            // TODO: перенести метод и все что связано с SearchInformer в отдельный obsolete класс (Eventually)
            string itemString = new ControllerWrapper().RenderView(
                "Portlets/Searches/SearchInformer",
                new SearchInformerViewModel
                    {
                        DirectionName = Search.DirectionName,
                        InstitutionName = Search.NamePart,
                        VuzCheckDirection = true,
                        SsuzCheckDirection = true,
                        VuzCheckInstitution = true,
                        SsuzCheckInstitution = true
                    });

            return itemString;
        }
    }
}