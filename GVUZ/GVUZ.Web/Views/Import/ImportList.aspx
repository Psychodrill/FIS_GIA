<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ImportListViewModel>" %>
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
	<% ViewData["MenuItemID"] = 4; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	

	<div id="tabControl" class="submenu"></div>
	<div>&nbsp;</div>
	<div id="content">
	    
        <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>
            <%--<input class="button3" type="button" value="Выгрузить список" id="btnLoadImportPackageList" style="margin-bottom:5px"/>--%>
        <span style="margin-left: 15px;">Поиск по ID Запроса: </span><input type="text" id="Text1" value='' style="width: auto" onkeypress="entergo(event)" /> 
        <% } %>

		<div class="tableHeader2l tableHeaderCollapsed">	
			<div id="divFilterPlace">
				<div class="hideTable" onclick="toggleFilter()" style="float:left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
				<div id="spAppCount" class="appCount">Заявок: <span id="spAppCountFieldF"></span></div>
			</div>
			<div id="divFilter" style="display:none;clear:both;">
			<div class="nameTable" style="display:none">Фильтр по списку заявок</div>	
			<table class="tableForm">
				<tbody>
					<tr>
                         <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                            { %>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedInstitution)%>
						</td>
						<td>
							<input type="text" id="selectInst" value='' />
						</td>
                        <% } %>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedType)%>
						</td>
						<td>
							<%= Html.DropDownListExFor(x => x.SelectedType, Model.Types, new {})%>
						</td>
                        <% if (UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff) && !UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                            { %>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedLogin)%>
						</td>
						<td id="selectedtd">
							<%= Html.TextBoxFor(x => x.SelectedLogin)%>
						</td>
                        <% } %>					

						<td>
							<%= Html.TableLabelFor(x => x.DateBegin) %>
						</td>
						<td>
							<%= Html.DatePickerFor(x => x.DateBegin) %>&nbsp;
							<%= Html.TableLabelFor(x => x.DateEnd) %>
							<%= Html.DatePickerFor(x => x.DateEnd) %>
						</td>
                         <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                            { %>
						<td>
							<%= Html.TableLabelFor(x => x.SelectedLogin)%>
						</td>
						<td>
							<%= Html.TextBoxFor(x => x.SelectedLogin)%>
						</td>
                        <% } %>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: center;">
							<input type="button" id="btnApplyFilterF" class="button primary" onclick="applyFilter()" value="Найти" />
							<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()" value="Сбросить фильтр" />
                        </td>
                    </tr>
				</tbody>
				</table>
			</div>
		</div>
<%--		<div class="temporary content">
			<table class="tableForm">
				<tbody>
		<tr>
                        <td colspan="6" style="text-align: center;">
							<font size="4" color="red" face="Arial">
Уважаемые пользователи, в связи с возросшей нагрузкой на Систему возможно возникновение очереди загрузки пакетов. Пакеты могут обрабатываться до 8 часов согласно очереди.
							</font>
                        </td>
                    </tr>
					</tbody>
				</table>
			</div>--%>
		<table class="gvuzDataGrid tableStatement2" cellpadding="3">
			<thead>
				<tr>
					<th>
						<span><%= Html.LabelFor(x => x.ImportPackageDescr.PackageID)%></span>
					</th>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 1)"><%= Html.LabelFor(x => x.ImportPackageDescr.Login)%></span>
					</th>
                    <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                       { %>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.ImportPackageDescr.InstitutionName)%></span>
					</th>
                    <% } %>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.ImportPackageDescr.DateSent)%></span>
					</th>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 4)"><%= Html.LabelFor(x => x.ImportPackageDescr.DateProcessing)%></span>
					</th>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 5)"><%= Html.LabelFor(x => x.ImportPackageDescr.Type)%></span>
					</th>
					<th>
						<span><%= Html.LabelFor(x => x.ImportPackageDescr.Content)%></span>
					</th>
					<th>
						<span class="linkSumulator sortable" onclick="doSort(this, 7)"><%= Html.LabelFor(x => x.ImportPackageDescr.Status)%></span>
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

        function entergo(evt) 
        {
            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode == 13)
            {
                var str = parseInt(document.getElementById("Text1").value);
                str1 = "ImportInfo?packageID="+str;
                if (str1 != "ImportInfo?packageID=NaN")
                {
                window.location = str1;
                }
                else
                {
                alert("Введите корректный номер запроса");
                }           
            }
        }

	jQuery(document).ready(function()
	{
		jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
<% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                       { %>		
		var institutionNames = jQuery.map(institutions, function(n, i) { return n.Name; });
		autocompleteDropdown(jQuery("#selectInst"), {
			source: institutionNames,
			delay: 200
		});
        <% } else { %>
		autocompleteDropdown(jQuery("#SelectedLogin"), {
			source: logins,
			delay: 200
        });
        <% } %>
		restoreFilter()
		updateData();
	});
    <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                       { %>
	var institutions = JSON.parse('<%= Html.Serialize(Model.Institutions) %>');

	<% } else {%>     var logins = JSON.parse('<%= Html.Serialize(Model.Logins) %>'); <% } %>

	function addItem($trBefore, item)
	{
		var className = $trBefore.prev().attr('class');
		if(className == 'trline2') className = 'trline1'; else className = 'trline2';
		$trBefore.before('<tr itemID="' + item.ID + '" class="' + className + '">'
			+ '<td width="5%">' + getImportCardAnchor(item.PackageID) + '</td>'
			+ '<td width="10%">' + escapeHtml(item.Login ? item.Login : "") + '</td>'
            <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin))
                       { %>
			+ '<td width="10%"><a href="#" onclick="doViewInstitution(' + item.InstitutionID + ');return false;">' + escapeHtml(item.InstitutionName) + '</span></td>'
			<% } %>
            + '<td width="20%">' + escapeHtml(item.DateSent) + '</td>'
			+ '<td width="20%">' + escapeHtml(item.DateProcessing) + '</td>'
			+ '<td width="15%">' + escapeHtml(item.Type) + '</td>'
			+ '<td width="20%">' + escapeHtml(item.Content) + '</td>'
			+ '<td width="10%">' + escapeHtml(item.Status) + '</td>'
			+ '</tr>');
	}
	
	function getImportCardAnchor(packageID) 
	{
		return '<a href="<%=Url.Generate<ImportController>(c=>c.ImportInfo(null)) %>?packageID='
			+ packageID + '" title="Просмотр результатов обработки пакета">'
				+ packageID + '</span>';
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

		doPostAjax('<%= Url.Generate<ImportController>(x => x.GetImportList(null)) %>', JSON.stringify(prepareModel()), function(data) {
			if (!addValidationErrorsFromServerResponse(data, false)) {
				gridItems = data.Data.ImportPackages;
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
