<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ImportPackageInfoViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<%@ Register TagPrefix="gv" TagName="EntrantCard" Src="~/Views/Shared/Entrant/EntrantCard.ascx" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
	Просмотр результатов обработки пакетов
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Результат обработки пакета: 
</asp:Content>

<asp:Content ID="PageSubtitle" ContentPlaceHolderID="PageSubtitle" runat="server">
	<%: Model.PackageID %>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
		<div id="content">
	<% ViewData["MenuItemID"] = 4; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
			
			<table id="cardTable" class="tableAdmin" >
				<tbody>
				<tr>
                   <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                      { %>
					<td class="caption big" style="width:170px;">
						<%= Html.TableLabelFor(x => x.InstitutionName)%>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.InstitutionName)%>
					</td>
                    <% }
                      else
                      { %>
                    <td class="caption big" style="width:170px;">
						<%= Html.TableLabelFor(x => x.Login)%>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.Login)%>
					</td> <% } %>
				</tr>
				<tr>
					<td class="caption big">
						<%= Html.TableLabelFor(x => x.DateSent) %>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.DateSent)%>
					</td>
				</tr>
				<tr>
					<td class="caption big">
						<%= Html.TableLabelFor(x => x.DateProcessing) %>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.DateProcessing)%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%= Html.TableLabelFor(x => x.Type) %>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.Type)%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">
						<%= Html.TableLabelFor(x => x.Status) %>
					</td>
					<td colspan="2">
						<%= Html.CommonInputReadOnly(Model.Status)%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%= Html.TableLabelFor(x => x.GetPackageString) %>
					</td>
					<td colspan="2">
						<a href="<%= Url.Generate<ImportController>(x => x.GetRawImportPackage(Model.PackageID)) %>">Скачать</a>
					</td>
				</tr>
				<tr>
					<td class="caption big" rowspan="5" style="text-align: left">
						Результат обработки:
					</td>
				</tr>
				<tr>
					<td class="caption" style="width: 200px">
						<%= Html.TableLabelFor(x => x.CountProcessed)%>
					</td>
					<td id="CountProcessed">
						<%= Html.CommonInputReadOnly(Model.CountProcessed.ToString())%>
					</td>
				</tr>
				<tr>
					<td id="CountNotProcessed" class="caption">
						<%= Html.TableLabelFor(x => x.CountNotProcessed)%>
					</td>
					<td>
						<%= Html.CommonInputReadOnly(Model.CountNotProcessed.ToString())%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%= Html.TableLabelFor(x => x.ResultError)%>
					</td>
					<td>
						<%= Html.CommonInputReadOnly(Model.ResultError)%>
					</td>
				</tr>
                <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>
				    <tr>
					    <td class="caption">
						    <%= Html.TableLabelFor(x => x.Comment)%>
					    </td>
					    <td>
						    <%= Html.CommonTextAreaReadOnly(Model.Comment)%>
					    </td>
				    </tr>
                <% } %>
				</tbody>
			</table>
		</div>
	</div>
	<br/>

    

	<% if (Model.ErrorObjects != null && Model.ErrorObjects.Length > 0)
	{
		var idx = 1;%>   
	<div id="tblErrorProcessedObjects">
        <%--<div class="tableHeader2l tableHeaderCollapsed">	
			<div id="divFilterPlace">
				<div class="hideTable" onclick="toggleFilter()" style="float:left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
				<div id="spAppCount" class="appCount">Количество заявок: <span id="spAppCountFieldF"></span></div>
			</div>
			<div id="divFilter" style="display:none;clear:both;">
			<div class="nameTable" style="display:none">Фильтр по списку заявок</div>	
			<table class="tableForm">
				<tbody>
					<tr>
                        <% { %>
						<td>
							<%= Html.TableLabelFor(x => x.ErrorObjectsDataDescr.ObjectType)%>
						</td>
						<td>
							<input type="text" id="errorTypeFilter" value='' />
						</td>
						<td>
							<%= Html.TableLabelFor(x => x.ErrorObjectsDataDescr.ObjectDetails)%>
						</td>
						<td>
							<input type="text" id="errorDetailsFilter" value='' />
						</td>		
                        <td>
							<%= Html.TableLabelFor(x => x.ErrorObjectsDataDescr.ErrorCode)%>
						</td>
						<td>
							<input type="text" id="errorCodeFilter" value='' />
						</td>
                        <td>
							<%= Html.TableLabelFor(x => x.ErrorObjectsDataDescr.ErrorText)%>
						</td>
						<td>
							<input type="text" id="errorTextFilter" value='' />
						</td>			
                        <% } %>
					</tr>
                    <tr>
                        <td colspan="8" align="center">
							<input type="button" id="btnApplyFilterF" class="button" onclick="applyFilter()"
								value="Найти" style="color: black; font-weight: bold;" />
							<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()"
								value="Сбросить фильтр" style="width: auto" />
                        </td>
                    </tr>
				</tbody>
				</table>
			</div>
		</div>--%>
		<h3>Необработанные объекты</h3>
        <%-- Фильтр для необработанных объектов--%>        
		<table class="gvuzDataGrid tableStatement2" cellpadding="3">
			<thead>
				<tr>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 1)"><%= Html.LabelFor(x => x.ErrorObjectsDataDescr.ObjectType) %></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.ErrorObjectsDataDescr.ObjectDetails) %></span>
					</th>
                    <th>
						<span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.ErrorObjectsDataDescr.ErrorCode) %></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 4)"><%= Html.LabelFor(x => x.ErrorObjectsDataDescr.ErrorText) %></span>
					</th>
				</tr>
			</thead>
			<tbody>
                <tr id="trAddNew" style="display:none">
				</tr>
				<% foreach (var errorObjectsData in Model.ErrorObjects)
				   { %>
				  <tr class="<%= (idx++%2 == 0 ? "trline2" : "trline1") %>">
				  	<td id ="ObjectType"><%: errorObjectsData.ObjectType %></td>
				  	<td id ="ObjectDetails"><%: errorObjectsData.ObjectDetails %></td>
                    <td id ="ErrorCode"><%: errorObjectsData.ErrorCode %></td>
					<td id ="ErrorText"><%: errorObjectsData.ErrorText %></td>
				  </tr> 	
				<% } %>
			</tbody>
		</table>
	</div>	
		<br/>
	<% } %>
	<% if (Model.SuccessObjects != null && Model.SuccessObjects.Length > 0)
	   {
	   	var idx = 1; %>
	<div id="tblSuccessProcessedObjects">
		<h3>Обработанные объекты</h3>
		<table class="gvuzDataGrid tableStatement2" cellpadding="3">
			<thead>
				<tr>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 5)"><%= Html.LabelFor(x => x.SuccessObjectsDataDescr.ObjectType) %></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 6)"><%= Html.LabelFor(x => x.SuccessObjectsDataDescr.ObjectDetails) %></span>
					</th>
					<th>
						<span class="linkSumulator" onclick="doSort(this, 7)"><%= Html.LabelFor(x => x.SuccessObjectsDataDescr.LinkString) %></span>
					</th>
				</tr>
			</thead>
			<tbody>
                <tr id="trAddNew2" style="display:none"></tr>
				<% foreach (var successObjectsData in Model.SuccessObjects)
				   { %>
				  <tr class="<%= (idx++%2 == 0 ? "trline2" : "trline1") %>">
				  	<td id ="ObjectType2"><%: successObjectsData.ObjectType %></td>
				  	<td id ="ObjectDetails2"><%: successObjectsData.ObjectDetails %></td>
					<td id ="HasLink2"><% if(successObjectsData.HasLink) { %><a href="<%= successObjectsData.GetLink(Url) %>" class="btnMove">&nbsp;</a> <%} %></td>
				  </tr> 	
				<% } %>
			</tbody>
		</table>
	</div>
	<% } %>
		<input type="button" value="Вернуться" id="btnBack" class="button3" style="margin-top: 5px"/>

    <script language="javascript" type="text/javascript">
    var gridItems = null;
    var currentSorting = null;	

	jQuery(document).ready(function () {
		jQuery("#btnBack").click(function () {
			window.location = '<%= Url.Generate<ImportController>(x => x.ImportPackageList()) %>?back=1';
		})
	});

	function addItem($trBefore, item)
	{
		var className = $trBefore.prev().attr('class');
		if(className == 'trline2') className = 'trline1'; else className = 'trline2';
		if ($trBefore.selector === '#trAddNew') {
		$trBefore.before('<tr class="' + className + '">'
			+ '<td>' + escapeHtml(item.ObjectType) + '</td>'
			+ '<td>' + escapeHtml(item.ObjectDetails) + '</td>'
			+ '<td>' + escapeHtml(item.ErrorCode) + '</td>'
			+ '<td>' + escapeHtml(item.ErrorText) + '</td>'
			+ '</tr>');
		}
		else if ($trBefore.selector == '#trAddNew2'){
		$trBefore.before('<tr class="' + className + '">'
			+ '<td>' + escapeHtml(item.ObjectType) + '</td>'
			+ '<td>' + escapeHtml(item.ObjectDetails) + '</td>'
			+ '<td>' + '<a href="' + item.LinkString + '" class="btnMove">&nbsp;</a>' + '</td>'
			+ '</tr>');
		}
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
		Math.abs(sortID) <= 4 ? updateData('#trAddNew') : updateData('#trAddNew2');
	}

    function updateData(cl)
	{
		clearValidationErrors(jQuery('#content'));

		doPostAjax('<%= Url.Generate<ImportController>(x => x.GetImportInfo(Model.PackageID, Model.SortID)) %>', JSON.stringify(prepareModel()), function(data) {
			if (!addValidationErrorsFromServerResponse(data, false)) {		        
		        cl == '#trAddNew' ? gridItems = data.Data.ErrorObjects : gridItems = data.Data.SuccessObjects;
		        fillGrid(cl);
			    //setFilterCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount);
			    //fillPager(data.Data.TotalPageCount, pageNumber);
		        //saveFilter();
            }
		});
    }

    function fillGrid(cl)
    {
        if (cl == '#trAddNew') {
            jQuery('#tblErrorProcessedObjects').find('tbody tr:not(' + cl + ')').remove().detach();
            jQuery.each(gridItems, function (idx, n) { addItem(jQuery(cl), n); });
        }
        if (cl == '#trAddNew2') {
            jQuery('#tblSuccessProcessedObjects').find('tbody tr:not(' + cl + ')').remove().detach();
            jQuery.each(gridItems, function (idx, n) { addItem(jQuery(cl), n); });
        }
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
	
	//function setFilterCount(filteredCount, totalCount)
	//{
	//	var res = totalCount;
	//	if (filteredCount < totalCount)
	//		res = filteredCount + ' из ' + res;
	//	jQuery('#spAppCountFieldF').html(res);
	//}

	function prepareModel() {
	    var model =
            {
                SortID: currentSorting,
                PageNumber: pageNumber,
                ObjectType: jQuery("#ObjectType").val(),
                ObjectDetails: jQuery("#ObjectDetails").val(),
                ErrorCode: jQuery('#ErrorCode').val(),
                ErrorText: jQuery('#ErrorText').val(),
            };

	    return model;
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
		setCookie('impPackageListFilter', JSON.stringify(prepareModel()), 1)
	}

	function restoreFilter()
	{
		var appListFilterString = getCookie('impPackageListFilter')
		if(typeof appListFilterString != "undefined")
		{
			setCookie('impPackageListFilter', '', -1)
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

	jQuery("#btnLoadImportPackageList").click(function() {

		clearValidationErrors(jQuery('#content'));

		doPostAjax('<%= Url.Generate<ImportController>(x => x.GenerateXmlListForExtended(null)) %>', JSON.stringify(prepareModel()), function(data) {
			if (!addValidationErrorsFromServerResponse(data, false)) {
				location.href = '<%= Url.Generate<ImportController>(x => x.GetXmlList(null, null)) %>?xmlListName=' + encodeURI(data.Data);
			}
		});
	});
    


	
</script>
</asp:Content>
