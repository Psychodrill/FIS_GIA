<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Searches.SearchInstitutionViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Portlets.Searches" %>

<script type="text/javascript">
    jQuery(document).ready(
        function() {

                        			<%--
				Ниже используется доработанный контрол "jQuery Autocomplete Mod"
				http://www.pengoworks.com/workshop/jquery/autocomplete.htm
				Доработка: поддержка подгрузки данных с сервера через Ajax методом Post, контрол поддерживал только Get.
				Для включения Post использовать флаг usePost : true.
				Скрипт находится в проекте по адресу: Resources/Scripts/libs/jquery.autocomplete.js				
				Пришлось дорабатывать "jQuery Autocomplete Mod", т.к. на портале уже применялся выше описанный контрол и сейчас 
				туда выложена доработанная версия с поддержкой Ajax методом Post.

				В MVC приложении используем jQuery UI Autocomplete.
			--%>

            // fill autocompletes
            jQuery("#InstitutionName").autocomplete('<%= PortletLinkHelper.InstitutionSearchByNameAjaxLink() %>',
                {
                    delay: 10,
                    minChars: 1,
                    matchSubset: 1,
                    autoFill: false,
                    cacheLength: 10,
                    matchContains: 1,
                    usePost: true,
                    lineSeparator: ',',
                    maxItemsToShow: 20,
                    onItemSelect: selectInstitutionItem
                });

                        <%--
			jQuery("#InstitutionName").autocomplete('<%= PortletLinkHelper.InstitutionSearchByNameAjaxLink() %>',
				source: function(request, response) {
						$.ajax({
							url: "url",
							data: request,
							dataType: "json",
							method: "post",
							success: response
						}
					});
								{source: predefinedSubjects, minLength: 0, delay: 0}
--%>

            jQuery("#DirectionName").autocomplete('<%= PortletLinkHelper.InstitutionDirectionSearchAjaxLink() %>',
                {
                    delay: 10,
                    minChars: 1,
                    matchSubset: 1,
                    autoFill: false,
                    cacheLength: 10,
                    matchContains: 1,
                    usePost: true,
                    lineSeparator: ',',
                    maxItemsToShow: 20,
                    onItemSelect: selectDirectionNameItem
                });


            jQuery("#DirectionCode").autocomplete('<%= PortletLinkHelper.InstitutionDirectionCodeSearchAjaxLink() %>',
                {
                    delay: 10,
                    minChars: 1,
                    matchSubset: 1,
                    autoFill: false,
                    cacheLength: 10,
                    matchContains: 1,
                    usePost: true,
                    lineSeparator: ',',
                    maxItemsToShow: 20,
                    onItemSelect: selectDirectionCodeItem
                });
            jQuery("#Region").autocomplete('<%= PortletLinkHelper.InstitutionRegionSearchAjaxLink() %>',
                {
                    delay: 10,
                    minChars: 1,
                    matchSubset: 1,
                    autoFill: false,
                    cacheLength: 10,
                    matchContains: 1,
                    usePost: true,
                    lineSeparator: ',',
                    maxItemsToShow: 20,
                    onItemSelect: selectRegionItem
                });

            // --- TriState CheckBoxes ---
            // установки при загрузке страницы
            setAdvancedSearch('<%= Model.AdvancedSearch %>');
            setCheckboxes();
            //jQuery('#btnSubmit').button();
        });

    function selectInstitutionItem(li) {
        if (li != null)
            document.getElementById('InstitutionNameIsFull').value = 'true';
    }

    function selectDirectionNameItem(li) {
        if (li != null)
            document.getElementById('DirectionNameIsFull').value = 'true';
    }

    function selectDirectionCodeItem(li) {
        if (li != null)
            document.getElementById('DirectionCodeIsFull').value = 'true';
    }

    function selectRegionItem(li) {
        if (li != null)
            document.getElementById('RegionIsFull').value = 'true';
    }

    // очистка значений 

    function clearIt(obj) {
        if (obj.name == 'InstitutionName') {
            document.getElementById('InstitutionNameIsFull').value = 'false';
            if (obj.value == '<%= Search.NamePart %>')
                obj.value = '';
        }

        if (obj.name == 'DirectionName') {
            document.getElementById('DirectionNameIsFull').value = 'false';
            if (obj.value == '<%= Search.DirectionName %>')
                obj.value = '';
        }

        if (obj.name == 'DirectionCode') {
            document.getElementById('DirectionCodeIsFull').value = 'false';
            if (obj.value == '<%= Search.DirectionCode %>')
                obj.value = '';
        }

        if (obj.name == 'Region') {
            document.getElementById('RegionIsFull').value = 'false';
            if (obj.value == '<%= Search.RegionName %>')
                obj.value = '';
        }

        if ((obj.name == 'City' && obj.value == '<%= Search.City %>')) {
            obj.value = '';
        }
        return false;
    }

    // открыть/закрыть расширенный поиск

    function chandgeAdvancedSearch() {
        var advancedBlock = document.getElementById('advancedSearchBlock');
        var advancedInput = document.getElementById('AdvancedSearch');
        var advancedValue;
        advancedValue = advancedBlock.style.display;
        if (advancedBlock.style.display == 'none')
            advancedValue = 'block';
        else
            advancedValue = 'none';

        advancedBlock.style.display = advancedValue;
        advancedInput.value = advancedValue;

        return false;
    }

    function setAdvancedSearch(style) {
        document.getElementById('advancedSearchBlock').style.display = style;
        return false;
    }


    // --- TriState CheckBoxes ---

    function getCheckValueId(id) {
        return id + 'Value';
    }

    function getjQueryId(id) {
        return "input#" + id;
    }

    function check(obj) {
        var jQueryId = getCheckValueId(obj.id);
        var checkValueId = getCheckValueId(obj.id);

        var checkValueObj = document.getElementById(checkValueId);
        var checkValue = checkValueObj.value;
        var newCheckValue;
        //"checked" -> "unchecked" -> "unselected"

        if (checkValue == "checked")
            newCheckValue = "unchecked";
        else if (checkValue == "unchecked")
            newCheckValue = "unselected";
        else
            newCheckValue = "checked";

        checkValueObj.value = newCheckValue;
        setCheck(obj, newCheckValue);

        return false;
    }

    function setCheck(checkObj, valueToSet) {
        var jQueryId = getjQueryId(checkObj.id);
        if (valueToSet == "checked") {
            checkObj.checked = "checked";
            jQuery(jQueryId).css('opacity', 1);
        } else if (valueToSet == "unchecked") {
            checkObj.checked = "";
            jQuery(jQueryId).css('opacity', 1);
        } else {
            checkObj.checked = "checked";
            jQuery(jQueryId).css('opacity', 0.35);
        }
    }

    function setCheckboxes() {
        setCheck(document.getElementById('MilitaryCheck'), '<%= Model.MilitaryCheckValue %>');
        setCheck(document.getElementById('CoursesCheck'), '<%= Model.CoursesCheckValue %>');
        setCheck(document.getElementById('OlympicsCheck'), '<%= Model.OlympicsCheckValue %>');
    }

</script>
<%
    if (Model.ValidationMessage != "")
    {
%>
    <div class="validation"><%= Html.LabelFor(m => m.ValidationMessage) %>: <%= Html.DisplayTextFor(m => m.ValidationMessage) %><br/></div>
<%
    }
%>

<form method="post" name="institutionsSearchForm" action="<%= PortletLinkHelper.SearchAction() %>" target="_self">
    <table  class="searchForm">
        <tr>
            <td><%= Html.LabelFor(m => m.InstitutionName) %>:</td>
            <td>
                <%= Html.TextBoxFor(m => m.InstitutionName, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchText"}}) %>
                <%= Html.HiddenFor(m => m.InstitutionNameIsFull) %>
            </td>
            <td><input class="button" type="submit" value="Найти" name="<%= SearchType.Normal %>"/></td>
        </tr>
        <tr>
            <td colspan="3">

                <%= Html.PortletCheckBoxFor(m => m.VuzCheck) %>&nbsp;<%= Html.LabelFor(m => m.VuzCheck) %>&nbsp;
                <%= Html.PortletCheckBoxFor(m => m.SsuzCheck) %>&nbsp;<%= Html.LabelFor(m => m.SsuzCheck) %>&nbsp;
            </td>
        </tr>
    </table>

    <a onclick=" chandgeAdvancedSearch(); "><%= Html.LabelFor(m => m.AdvancedSearch) %></a><br />
    <div id="advancedSearchBlock" style="display: none;">
        <%= Html.HiddenFor(m => m.AdvancedSearch) %>

        <table  class="searchForm">
            <tr>
                <td><%= Html.LabelFor(m => m.DirectionName) %>:</td>
                <td>
                    <%= Html.TextBoxFor(m => m.DirectionName, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchField"}}) %>
                    <%= Html.HiddenFor(m => m.DirectionNameIsFull) %>
                </td>
            </tr>
            <tr>
                <td><%= Html.LabelFor(m => m.DirectionCode) %>:</td>
                <td>
                    <%= Html.TextBoxFor(m => m.DirectionCode, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchField"}}) %>
                    <%= Html.HiddenFor(m => m.DirectionCodeIsFull) %>
                </td>
            </tr>
            <%--<tr><td><%= Html.LabelFor(m=>m.City) %>:</td><td><%= Html.TextBoxFor(m => m.City, new Dictionary<string, object> { { "onclick", "clearIt(this);" } })%></td></tr>--%>
            <tr>
                <td><%= Html.LabelFor(m => m.Region) %>:</td>
                <td>
                    <%= Html.TextBoxFor(m => m.Region, new Dictionary<string, object> {{"onclick", "clearIt(this);"}, {"class", "searchField"}}) %>
                    <%= Html.HiddenFor(m => m.RegionIsFull) %>
                </td>
            </tr>
            <tr><td><%= Html.LabelFor(m => m.StudyFormId) %>:</td><td><%= Html.DropDownListFor(m => m.StudyFormId,
                                                                                               new SelectList(Model.StudyFormList, "ItemTypeID", "Name"), new Dictionary<string, object> {{"class", "searchField"}}) %></td></tr>
            <tr><td><%= Html.LabelFor(m => m.FormOfLawID) %>:</td><td><%= Html.DropDownListFor(m => m.FormOfLawID,
                                                                                               new SelectList(Model.FormOfLawList, "FormOfLawID", "Name"), new Dictionary<string, object> {{"class", "searchField"}}) %></td></tr>
            <tr><td><%= Html.LabelFor(m => m.EducationLevelId) %>:</td><td><%= Html.DropDownListFor(m => m.EducationLevelId,
                                                                                                    new SelectList(Model.EducationLevelList, "ItemTypeID", "Name"), new Dictionary<string, object> {{"class", "searchField"}}) %></td></tr>
            <tr><td><%= Html.LabelFor(m => m.AdmissionTypeId) %>:</td><td><%= Html.DropDownListFor(m => m.AdmissionTypeId,
                                                                                                   new SelectList(Model.AdmissionTypeList, "ItemTypeID", "Name"), new Dictionary<string, object> {{"class", "searchField"}}) %></td></tr>
        </table>

        <%= Html.PortletTriStateCheckBoxFor(m => m.MilitaryCheckValue, "check(this);") %>&nbsp;<%= Html.LabelFor(m => m.MilitaryCheckValue) %>&nbsp;
        <%= Html.PortletTriStateCheckBoxFor(m => m.CoursesCheckValue, "check(this);") %>&nbsp;<%= Html.LabelFor(m => m.CoursesCheckValue) %>&nbsp;
        <%= Html.PortletTriStateCheckBoxFor(m => m.OlympicsCheckValue, "check(this);") %>&nbsp;<%= Html.LabelFor(m => m.OlympicsCheckValue) %>&nbsp;<br />

        <%= Html.HiddenFor(m => m.PageNumber) %>

        <input type="submit" value="Найти" class="button" name="<%= SearchType.Advanced %>"/><br />
    </div>
</form>