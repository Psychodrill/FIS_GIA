<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.AccessListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Очередь запросов
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
	Очередь запросов
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement">
	<% ViewData["MenuItemID"] = 5; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
	
	<div id="tabControl" class="submenu"></div>
	<div>&nbsp;</div>
	<div id="content">
		<div class="tableHeader2l tableHeaderCollapsed">	
			<div id="divFilterPlace">
				<div class="hideTable" onclick="toggleFilter()" style="float:left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
				<div id="spAppCount" class="appCount">Количество элементов: <span id="spAppCountFieldF"></span></div>
			</div>
			<div id="divFilter" style="display:none;clear:both;">
			<div class="nameTable" style="display:none">Фильтр по списку доступа</div>	
			<table class="tableForm">
				<tbody>
					<tr>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedInstitution)%>
						</td>
						<td>
							<input type="text" id="selectInst" value='' />
						</td>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedType)%>
						</td>
						<td>
							<%= Html.DropDownListExFor(x => x.SelectedType, Model.Types, new {})%>
						</td>
						<td rowspan="2">
							<input type="button" id="btnApplyFilterF" class="button primary" onclick="applyFilter()" value="Найти"/>
						</td>
						<td rowspan="2">
							<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()" value="Сбросить фильтр" />
						</td>
					</tr>
					<tr>
						<td>
							<%= Html.TableLabelFor(x => x.DateBegin) %>
						</td>
						<td>
							<%= Html.DatePickerFor(x => x.DateBegin) %>&nbsp;
							<%= Html.TableLabelFor(x => x.DateEnd) %>
							<%= Html.DatePickerFor(x => x.DateEnd) %>
						</td>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedLogin)%>
						</td>
						<td>
							<%= Html.TextBoxFor(x => x.SelectedLogin)%>
						</td>
					</tr>
				</tbody>
				</table>
			</div>
		</div>
	
		<table class="gvuzDataGrid tableStatement2" cellpadding="3">
			<thead>
				<tr>
					<th style="display: none">
						<span><%= Html.LabelFor(x => x.AccessLogDescr.ID)%></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 1)"><%= Html.LabelFor(x => x.AccessLogDescr.Login)%></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.AccessLogDescr.InstitutionName)%></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.AccessLogDescr.DateCreated)%></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 4)"><%= Html.LabelFor(x => x.AccessLogDescr.Type)%></span>
					</th>
                    <th>
						<span><%= Html.LabelFor(x => x.AccessLogDescr.ObjectName)%></span>
					</th>
					<th>
						<span><%= Html.LabelFor(x => x.AccessLogDescr.ObjectMethod)%></span>
					</th>
					<th>
						<span><%= Html.LabelFor(x => x.AccessLogDescr.Content)%></span>
					</th>
				</tr>
			</thead>
			<tbody>
				<tr id="trAddNew" style="display:none">
				</tr>
			</tbody>
		</table>
	</div>
</div>
<div id="divInstitutionViewPopup"></div>
<script language="javascript" type="text/javascript">
	var gridItems = null;
	var currentSorting = null;
	var UserReadonly = <%= UserRole.CurrentUserInRole(UserRole.FBDReadonly) ? "true" : "false" %>;

	jQuery(document).ready(function()
	{
		jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
		
		var institutionNames = jQuery.map(institutions, function(n, i) { return n.Name; });
		autocompleteDropdown(jQuery("#selectInst"), {
			source: institutionNames,
			delay: 200
		});
		restoreFilter()
		updateData();
	});
	var institutions = JSON.parse('<%= Html.Serialize(Model.Institutions) %>');
	
	function addItem($trBefore, item)
	{
		var className = $trBefore.prev().attr('class');
		if(className == 'trline2') className = 'trline1'; else className = 'trline2';
		$trBefore.before('<tr itemID="' + item.ID + '" class="' + className + '">'
			+ '<td style="display:none">' + item.ID + '</td>'
			+ '<td>' + escapeHtml(item.Login ? item.Login : "") + '</td>'
			+ '<td><a href="#" onclick="doViewInstitution(' + item.InstitutionID + ');return false;">' + escapeHtml(item.InstitutionName) + '</span></td>'
			+ '<td>' + escapeHtml(item.DateCreated) + '</td>'
			+ '<td>' + escapeHtml(item.Type) + '</td>'
		    + '<td>' + escapeHtml(item.ObjectName) + '</td>'
		    + '<td>' + escapeHtml(item.ObjectMethod) + '</td>'
			+ '<td>' + escapeHtml(item.Content).replace(/\n/g, '<br/>') + '</td>'
			+ '</tr>');
	}
	
	function fillGrid()
	{
		jQuery('.gvuzDataGrid tbody tr:not(#trAddNew)').remove().detach();
		jQuery.each(gridItems, function (idx, n) { addItem(jQuery('#trAddNew'), n); });
	}

	function doSort(el, sortID)
	{
		var isSortedUp = jQuery(el).hasClass('sortedUp');
		jQuery('.sortUp,.sortDown').remove().detach();
		if (isSortedUp)
			jQuery(el).after('<span class="sortDown"></span>');
		else
			jQuery(el).after('<span class="sortUp"></span>');
		jQuery(el).removeClass('sortedUp');
		if (isSortedUp)
			sortID = -sortID;
		else
			jQuery(el).addClass('sortedUp');
		currentSorting = sortID;;
		updateData();
	}
	
	function toggleFilter()
	{
		if (jQuery('#btnShowFilter').hasClass('filterDisplayed'))
		{
			jQuery('#btnShowFilter').removeClass('filterDisplayed');
			jQuery('#btnShowFilter').html('Отобразить фильтр');
			jQuery('#btnShowFilter').parent().removeClass('nonHideTable');
			jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed');
			jQuery('#divFilter').hide();
		}
		else
		{
			jQuery('#btnShowFilter').addClass('filterDisplayed');
			jQuery('#btnShowFilter').html('Скрыть фильтр');
			jQuery('#btnShowFilter').parent().addClass('nonHideTable');
			jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed');
			jQuery('#divFilter').show();
		}
	}
	
	function setFilterCount(filteredCount, totalCount)
	{
		var res = totalCount;
		if (filteredCount < totalCount)
			res = filteredCount + ' из ' + res;
		jQuery('#spAppCountFieldF').html(res);
	}

	function prepareModel()
	{
		var model =
			{
				SortID: currentSorting,
				PageNumber: pageNumber,
				SelectedInstitution: jQuery("#selectInst").val(),
				SelectedType: jQuery("#SelectedType").val(),
				DateBegin: jQuery('#DateBegin').val(),
				DateEnd: jQuery('#DateEnd').val(),
				SelectedLogin: jQuery('#SelectedLogin').val()
			};
		
		return model;
	}

	function updateData()
	{
		clearValidationErrors(jQuery('#content'));

		doPostAjax('<%= Url.Generate<AccessLogController>(x => x.GetAccessLogList(null)) %>', JSON.stringify(prepareModel()), function(data) {
			if (!addValidationErrorsFromServerResponse(data, false)) {
				gridItems = data.Data.AccessLogs;
				fillGrid();
				setFilterCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount);
				fillPager(data.Data.TotalPageCount, pageNumber);
				saveFilter();
			}
		});
	}

	var pageNumber = <%= ViewBag.ESearchPageNumber ?? 0 %>;

	function movePager(pageID)
	{
		pageNumber = pageID;
		updateData();
	}
	
	function doChangeFilter()
	{
		updateData();
	}
	
	function clearFilter()
	{
		jQuery('#selectInst').val('');
		jQuery('#SelectedType').val('0');
		jQuery('#DateBegin').val('');
		jQuery('#DateEnd').val('');
		jQuery('#SelectedLogin').val('');
		applyFilter();
	}
	
	function applyFilter() 
	{
		setTimeout(applyFilter1, 0);
	}

	function applyFilter1() 
	{
		pageNumber = 0;
		updateData();
	}
	
	function saveFilter()
	{
		setCookie('alListFilter', JSON.stringify(prepareModel()), 1);
	}

    

	function restoreFilter()
	{
		var appListFilterString = getCookie('alListFilter');

		        if(typeof appListFilterString != "undefined")
		        {
			        //setCookie('alListFilter', '', -1)
			        if(document.location.toString().indexOf('back=1') >0)
			        {
				        var model = JSON.parse(appListFilterString)
				        currentSorting = model.SortID
				        pageNumber = model.PageNumber
				        {
					        jQuery('#selectInst').val(model.SelectedInstitution);
					        jQuery('#SelectedType').val(model.SelectedType);
					        jQuery('#DateBegin').val(model.DateBegin);
					        jQuery('#DateEnd').val(model.DateEnd);
					        jQuery('#SelectedLogin').val(model.SelectedLogin);
					        jQuery('#Filter_DateEnd').val(model.DateEnd)
				        }
			        }
		        }
         
         
	}


	function doViewInstitution(instID)
	{
		doPostAjax('<%= Url.Generate<InstitutionController>(x => x.ViewAdmPopup(null)) %>?institutionID=' + instID, '', function (data)
		{
			jQuery('#divInstitutionViewPopup').html(data);
			jQuery('#divInstitutionViewPopup').dialog({
				modal: true,
				width: 800,
				title: 'Информация об ОО'
			}).dialog('open');
		}, "application/x-www-form-urlencoded", "html")
	}
</script>

</asp:Content>
