<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.CompetitiveGroup.CompetitiveGroupViewModel>" %>
<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Конкурсы
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement">
<gv:tabcontrol runat="server" id="tabControl" />
<style type="text/css">
	.dataTable
	{
        margin-top: auto; /*IE bug*/
		/*overflow-x: auto;
		overflow-y: auto;*/
	}
</style>

<script type="text/javascript">
    function doDelete(groupID) {
        confirmDialog('Вы действительно хотите удалить конкурс?', function() {
            doPostAjax('<%= Url.Generate<CompetitiveGroupController>(c => c.CompetitiveGroupDelete(null)) %>', 'competitiveGroupID=' + groupID, function(data) {
                if (data.IsError) {
                    infoDialog(data.Message);
                    return;
                }
                if (!addValidationErrorsFromServerResponse(data))
                    location.reload(true)
            }, "application/x-www-form-urlencoded")
        });
    }

    jQuery(function ()
    {
        jQuery('#btnAdd').click(function ()
        {
            doPostAjax('<%= Url.Generate<CompetitiveGroupController>(c => c.CompetitiveGroupAdd()) %>', null, function (data)
            {
                jQuery('#addGroupDialog').html(data);
                jQuery('#addGroupDialog').dialog({
                    modal: true,
                    width: 800,
                    title: 'Добавление конкурса',
                    buttons:
                        {
                            "Сохранить": function () { cg_save(function () { $(this).dialog('close'); }.bind(this) ); },
                            "Отмена": function () { $(this).dialog('close'); }
                        }
                }).dialog('open');
            }, "application/x-www-form-urlencoded", "html")

        })
    })

    jQuery(document).ready(function()
    {
        if(currentSortID != null)
        {
            if(currentSortID > 0)
                jQuery('#spSort' + currentSortID).after('<span class="sortUp"></span>')
            else
                jQuery('#spSort' + (-currentSortID)).after('<span class="sortDown"></span>')
        }
        fillPager(<%= Model.TotalPageCount %>, <%= Model.PageNumber %>)

        var CGListFilterString = getCookie('CGListFilter');
		if(typeof CGListFilterString != "undefined")
		{
            setCookie('CGListFilter',CGListFilterString,-1);
    	    window.location = '<%= Url.Generate<CompetitiveGroupController>(x => x.GetCompetitiveGroupList(null))  %>' + CGListFilterString;
        }
        else 
        {
            var urlPart1 = []
            if(currentSortID && currentSortID != 0) urlPart1.push('sortID=' + currentSortID);
            if(currentPageNumber && currentPageNumber != 0) urlPart1.push('pageNumber=' + currentPageNumber);
            if(!filterModel) filterModel = { };
            if(filterModel.Name) urlPart1.push('fName=' + filterModel.Name);
            if(filterModel.Course && filterModel.Course != 0) urlPart1.push('fCourse=' + filterModel.Course);
            if(filterModel.CampaignID && filterModel.CampaignID != 0) urlPart1.push('fCampaignID=' + filterModel.CampaignID);
            if (filterModel.EducationLevelID && filterModel.EducationLevelID != 0) urlPart1.push('fEducationLevelID=' + filterModel.EducationLevelID);
            if (filterModel.UID) urlPart1.push('fuid=' + filterModel.UID)

            var str = '?' + urlPart1.join('&');
            setCookie('CGListFilter', str);
        }
    })

    var currentSortID = <%= Model.SortID %>
    function doSort(el, sortID)
    {
        if (sortID == currentSortID)
            sortID= -sortID
        currentSortID = sortID;
        doNavigate();
    }

    var currentPageNumber = <%= Model.PageNumber %>

    function movePager(pageNumber)
    {
        currentPageNumber = pageNumber;
        doNavigate();
    }

</script>

<%if (true) // (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection)) && Model.HasCompaigns)
  { %>
<div>    
	<input type="button" value="Добавить конкурс" id="btnAdd" class="button3 w250px" />
</div>
<%} %>
<div>&nbsp;</div>
<div id="addGroupDialog"></div>
<div class="content">
    <div class="tableHeader2l tableHeaderCollapsed">	
	<div id="divFilterPlace">
		<div class="hideTable" onclick="toggleFilter()" style="float:left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
        <div id="spAppCount" class="appCount">Количество конкурсных групп: <%= Model.TotalFilteredCount %> <%= Model.TotalFilteredCount < Model.TotalItemCount ? "из " + Model.TotalItemCount : "" %></div>
	</div>
	<div id="divFilter" style="display:none;clear:both;">
	<div class="nameTable" style="display:none">Фильтр по списку конкурсных групп</div>	
	<table class="tableForm" >
		<tbody>
			<tr>
				<td class="labelsInside" width="10%"><%= Html.TableLabelFor(x => x.Filter.Name) %></td>
				<td width="23%"><%= Html.TextBoxExFor(x => x.Filter.Name) %></td>

				<td class="labelsInside" width="10%"><%= Html.TableLabelFor(x => x.Filter.Course) %></td>
				<td width="23%"><%= Html.DropDownListExFor(x => x.Filter.Course, Model.Courses, new {  })%></td>
                
				<td class="labelsInside" width="10%"><%= Html.TableLabelFor(x => x.Filter.UID) %></td>
				<td width="23%"><%= Html.TextBoxExFor(x => x.Filter.UID) %></td>
			</tr>
			<tr>	
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.CampaignID) %></td>
				<td><%= Html.DropDownListExFor(x => x.Filter.CampaignID, Model.Campaigns, new {  })%></td>
				
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EducationLevelID) %></td>
				<td><%= Html.DropDownListExFor(x => x.Filter.EducationLevelID, Model.EducationLevels, new {  })%></td>

                <td></td>
                <td>
				    <input type="button" id="btnApplyFilterF" class="button" onclick="applyFilter()" value="Найти" style="color: black; font-weight: bold;" />
					<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()" value="Сбросить фильтр" style="width: auto" />
				</td>
			</tr>
		</tbody>
	</table>
	</div>
	</div>
    
<div class="dataTable">
<table class="tableStatement2">
	<thead>
		<tr>
			<th rowspan="2"><span class="linkSumulator" onclick="doSort(this, 1)" id="spSort1"><%: Html.LabelFor(x => x.DisplayData.GroupName) %></span></th>
			<th rowspan="2" nowrap="nowrap"><span class="linkSumulator" onclick="doSort(this, 2)" id="spSort2"><%: Html.LabelFor(x => x.DisplayData.CourseName) %></span></th>
			<th rowspan="2" nowrap="nowrap"><span class="linkSumulator" onclick="doSort(this, 5)" id="spSort5"><%: Html.LabelFor(x => x.DisplayData.CampaignName) %></span></th>
			<th rowspan="2"><span id="spSort3"><%: Html.LabelFor(x => x.DisplayData.EducationalLevelID) %></span></th>
			<th rowspan="2"><%: Html.LabelFor(x => x.DisplayData.DirectionID) %></th>
			<th colspan="3"><%: Html.LabelFor(x => x.DisplayData.BudgetName) %></th>
			<th colspan="3"><%: Html.LabelFor(x => x.DisplayData.QuotaName) %></th>
			<th colspan="3"><%: Html.LabelFor(x => x.DisplayData.PaidName) %></th>
			<th colspan="3"><%: Html.LabelFor(x => x.DisplayData.TargetName) %></th>
			<%if (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection))
     { %>
			<th rowspan="2">Действие</th>
			<%} %>
		</tr>
		<tr>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetOZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaOZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberPaidO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberTargetO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberTargetOZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberTargetZ) %></th>
		</tr>
	</thead>
	<tbody>
		<%
			var globalCounter = 1;
    var str4sign = "Наименование КГ\tКурс\tПК\tУровень образования\tСпециальность\t\t\t\t\tБюджет\tКвота\tПлатные\tЦелевые\n\n";
    str4sign += "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\n\n";
	foreach (var groupItem in Model.TreeItems)
	{
		int rowSpan = groupItem.Count;
		
		for (int i = 0; i < groupItem.Count; i++ )
		{
			globalCounter ++;
			var direction = groupItem[i];

            str4sign += direction.GroupName == null ? "" : direction.GroupName + "\t";
            if ((direction.GroupName == null) || (direction.GroupName.Length < 9))
             str4sign += "\t";
            
            str4sign += direction.CourseName == null ? "" : direction.CourseName + "\t";
            str4sign += direction.CampaignName + "\t";
            str4sign += direction.EducationalLevelName == null ? "" : direction.EducationalLevelName + "\t";
            str4sign += direction.DirectionName + "\t";
            if (direction.DirectionName.Length < 11) str4sign += "\t\t\t\t\t\t";
             else if (direction.DirectionName.Length < 21) str4sign += "\t\t\t\t\t";
              else if (direction.DirectionName.Length < 31) str4sign += "\t\t\t\t";
               else if (direction.DirectionName.Length < 41) str4sign += "\t\t\t";
                else if (direction.DirectionName.Length < 52) str4sign += "\t\t";
                 else if (direction.DirectionName.Length < 62) str4sign += "\t";
                   //else if (direction.DirectionName.Length < 73) str4sign += "\t";            
                

            str4sign += direction.NumberBudgetO == null ? "" : direction.NumberBudgetO.ToString() + "\t";
            str4sign += direction.NumberBudgetOZ == null ? "" : direction.NumberBudgetOZ.ToString() + "\t";
            str4sign += direction.NumberBudgetZ == null ? "" : direction.NumberBudgetZ.ToString() + "\t";
            str4sign += direction.NumberPaidO == null ? "" : direction.NumberPaidO.ToString() + "\t";
            str4sign += direction.NumberPaidOZ == null ? "" : direction.NumberPaidOZ.ToString() + "\t";
            str4sign += direction.NumberPaidZ == null ? "" : direction.NumberPaidZ.ToString() + "\t";
            str4sign += direction.NumberTargetO == null ? "" : direction.NumberTargetO.ToString() + "\t";
            str4sign += direction.NumberTargetOZ == null ? "" : direction.NumberTargetOZ.ToString() + "\t";
            str4sign += direction.NumberTargetZ == null ? "" : direction.NumberTargetZ.ToString();

            str4sign += "\n\n";
            
            
            
			var borderClass = i == groupItem.Count - 1 ? "" : "noBottomBorder";
			%>
			<tr class="trline2">
			<% if (i == 0)
			  {%>
				<td isName="1" rowspan="<%= rowSpan %>" >
				<%if ((!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection)) && direction.CanEdit)
      { %>
				<a href="<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupEdit(direction.GroupID, Model.Filter.CampaignID)) %>" title="Редактировать конкурсную группу">
					<%: direction.GroupName%></a>
				<%} else {%>
					<a href="<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupEdit(direction.GroupID, Model.Filter.CampaignID)) %>" title="Просмотр конкурсной группы">
					<%: direction.GroupName%></a>
				<%} %>
				</td>
				<td rowspan="<%= rowSpan %>" ><%: direction.CourseName%></td>
				<td rowspan="<%= rowSpan %>" ><%: direction.CampaignName%></td>
				
			<%} %>
				<td class="<%= borderClass %>"><%: direction.EducationalLevelName.To("")%></td>
				<td class="<%= borderClass %>"><%: direction.DirectionName.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberBudgetO.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberBudgetOZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberBudgetZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberQuotaO.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberQuotaOZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberQuotaZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberPaidO.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberPaidOZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberPaidZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberTargetO.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberTargetOZ.To("")%></td>
				<td class="<%= borderClass %>" align="center"><%: direction.NumberTargetZ.To("")%></td>
			<% if (i == 0 && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection)))
			  {%>
				<td rowspan="<%= rowSpan %>" align="center" >
					<% if (direction.CanEdit)
		{ %>
				<a href="<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupEdit(direction.GroupID, Model.Filter.CampaignID)) %>" class="btnEdit" title="Редактировать конкурсную группу"></a>
				<a href="javascript:doDelete(<%= direction.GroupID %>)" class="btnDelete" title="Удалить конкурсную группу"></a>
				<% } else { %>
					   <span class="btnEditGray" title="Невозможно редактировать конкурсную группу"></span>
						<span  class="btnDeleteGray" title="Невозможно удалить конкурсную группу"></span>
				<% } %>
				</td>
			<%} %>
			</tr>

		<%}
        if (groupItem != Model.TreeItems.Last())
          { %><tr><td colspan="18"><hr width="100%" size="2" color="dddddd"></td></tr><% }

	}%>
	</tbody>
</table>
</div>
</div>
</div>
<script type="text/javascript">
    menuItems[4].selected = true;
</script>
<div id="divPopupDialog"></div>
<div id="divBenefitListDialog"></div>
<div id="divAddBenefit"></div>
<div id="divViewOlympic" style="padding: 5px;display:none;position:absolute" class="ui-widget ui-widget-content ui-corner-all"></div>
<div id="divAddOlympicMultiple"></div>
    
<script type="text/javascript">
    function toggleFilter()
    {
        if (jQuery('#btnShowFilter').hasClass('filterDisplayed'))
        {
            jQuery('#btnShowFilter').removeClass('filterDisplayed')
            jQuery('#btnShowFilter').html('Отобразить фильтр')
            jQuery('#btnShowFilter').parent().removeClass('nonHideTable')
            jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed')
            jQuery('#divFilter').hide()
        }
        else
        {
            jQuery('#btnShowFilter').addClass('filterDisplayed')
            jQuery('#btnShowFilter').html('Скрыть фильтр')
            jQuery('#btnShowFilter').parent().addClass('nonHideTable')
            jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed')
            jQuery('#divFilter').show()
        }
    }

    var filterModel = JSON.parse('<%= Html.Serialize(Model.Filter) %>');

    function doNavigate() {
        var urlPart = []
        if(currentSortID && currentSortID != 0) urlPart.push('sortID=' + currentSortID);
        if(currentPageNumber && currentPageNumber != 0) urlPart.push('pageNumber=' + currentPageNumber);
        if(!filterModel) filterModel = { };
        if(filterModel.Name) urlPart.push('fName=' + filterModel.Name);
        if(filterModel.Course && filterModel.Course != 0) urlPart.push('fCourse=' + filterModel.Course);
        if(filterModel.CampaignID && filterModel.CampaignID != 0) urlPart.push('fCampaignID=' + filterModel.CampaignID);
        if (filterModel.EducationLevelID && filterModel.EducationLevelID != 0) urlPart.push('fEducationLevelID=' + filterModel.EducationLevelID);
        if (filterModel.UID) urlPart.push('fuid=' + filterModel.UID)

        var j = '?' + urlPart.join('&');
        setCookie('CGListFilter', j);
        window.location = '<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupList(null, null, null, null, null, null, null))  %>' + j;
    }

    function applyFilter() {
        filterModel = {
            Name: jQuery("#Filter_Name").val(),
            Course: jQuery("#Filter_Course").val(),
            CampaignID: jQuery("#Filter_CampaignID").val(),
            EducationLevelID: jQuery("#Filter_EducationLevelID").val(),
            UID: jQuery("#Filter_UID").val()
        }
        
        currentPageNumber = 0;
        doNavigate()
    }
    
    function clearFilter() {
        filterModel = {
            Name: '',
            Course: 0,
            CampaignID: 0,
            EducationLevelID: 0
        }
        currentPageNumber = 0;
        doNavigate()
    }

</script>
</asp:Content>
