<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Список ОО
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Список ОО
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
	<% ViewData["MenuItemID"] = 2; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
		
		<div id="divPopupApplication"></div>
		
		<div class="tableHeader" id="divFilterRegionF">
			<div class="hideTable nonHideTable" style="float:left;"><span style="cursor:default;">Быстрый поиск</span></div>
			<div id="spAppCountF" class="appCount">Записей:&nbsp;<span id="spAppCountFieldF"></span></div>
			<div id="divFilterF">
           
			<table class="tableForm">
				<tbody>
					<tr>
						<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.FullName) %></td>
						<td><input type="text" id="Filter_ShortNameF" onkeypress="entergo(event)" /></td>
						<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.InstitutionTypeID) %></td>
						<td><%= Html.DropDownListExFor(x => x.Filter.InstitutionTypeIDF, Model.InstitututionTypes, new { @class = "ss" })%></td>
						<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.RegionID) %></td>
						<td><%= Html.TextBoxExFor(x => x.Filter.RegionIDF)%></td>
						<td nowrap="nowrap">
							<input type="submit" id="btnApplyFilterF" class="button primary" onclick="applyFilter()" value="Найти"/>
							<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()" value="Сбросить фильтр" />
							<input type="button" id="btnSwitchFilterF" class="button" onclick="toggleFilter(1)" value="Расширенный поиск"/>
						</td>
					</tr>
				</tbody>
				</table>
			</div>
		</div>
		
		<div class="tableHeader5l" id="divFilterRegion" style="display:none">	
	<div id="divFilterPlace">
		<div class="hideTable nonHideTable" style="float:left"><span style="cursor:default;">Расширенный поиск</span></div>
		<div id="spAppCount" class="appCount">Количество заявлений: <span id="spAppCountField"></span></div>
	</div>
	<div id="divFilter">
	<table class="tableForm">
    <br>
		<tbody>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.ShortName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.ShortName) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.InstitutionTypeID) %></td>
				<td><%= Html.DropDownListExFor(x => x.Filter.InstitutionTypeID, Model.InstitututionTypes, null) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.RegionID) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.RegionID) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.Owner) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.Owner) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.FullName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.FullName) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.INN) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.INN) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.OGRN) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.OGRN) %></td>
				<td class="labelsInside"></td>
				<td></td>
			</tr>
			<tr>	
				<td colspan="4" style="text-align: center;">
                    <input type="submit" id="btnApplyFilter" class="button primary" onclick="applyFilter()" value="Найти" />
				    <input type="button" id="btnClearFilter" class="button" onclick="clearFilter()" value="Сбросить фильтр" />
				    <input type="button" id="btnSwitchFilter" class="button" onclick="toggleFilter(0)" value="Сбросить и скрыть область" />
				</td>
			</tr>
		</tbody>
	</table>
	</div>
	</div>

		<div id="content">
			<table class="gvuzDataGrid tableStatement2">
				<thead>
					<tr>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 1)"><%= Html.LabelFor(x => x.InstitutionDataNull.FullName) %></span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.InstitutionDataNull.InstitutionTypeName)%></span>
						</th>
						<th style="text-align:center">
							<span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.InstitutionDataNull.RegionName)%></span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 4)"><%= Html.LabelFor(x => x.InstitutionDataNull.Owner)%></span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 5)"><%= Html.LabelFor(x => x.InstitutionDataNull.UserCount)%></span>
						</th>
					</tr>					
				</thead>
				<tbody>
					<tr id="firstRow" style="display:none">	
					</tr>
				</tbody>
			</table>
		</div>
	</div>

	<script language="javascript" type="text/javascript">

		var gridItems = null
		var currentSorting = null

        $(document).ready(function()
        {
            document.getElementById("Filter_ShortName").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_RegionID").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_RegionIDF").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_Owner").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_FullName").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_INN").onkeypress=function(event) {entergo(event)}; 
            document.getElementById("Filter_OGRN").onkeypress=function(event) {entergo(event)};
            jQuery('#Filter_RegionID').val('')
            jQuery('#Filter_RegionIDF   ').val('')
	    });
		
        function entergo(evt) //Событие - нажатие на кнопку Enter для начала поиска.
        {
            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode == 13)
            {
                applyFilter();
            }
        }

		function addItem($trBefore, item) {							

				var className = jQuery('[itemID]').last().attr('class')
				if (className == 'trline2') className = 'trline1'; else className = 'trline2';
				var resultTableContent = ''

				var selectedStyle = ''

				if(item.InstitutionID == <%= Model.CurrentInstitutionID %>)
					selectedStyle = 'style="background-color:#ffffe0;"'

				resultTableContent += '<tr itemID="' + item.InstitutionID + '" class="' + className + '" ' + selectedStyle + '>'
										+ '<td width="65%" title="' + escapeHtml(item.FullName) + '">'
											+'<a href="<%= Url.Generate<InstitutionAdminController>(x => x.SwitchToInstitution(null)) %>?institutionID=' + item.InstitutionID
											+ '">'+ escapeHtml(item.FullName) + '</a></td>'
										+ '<td width="5%" >' + escapeHtml(item.InstitutionTypeName) + '</td>'
										+ '<td width="10%" >' + escapeHtml(item.RegionName) + '</td>'
										+ '<td width="10%" >' + escapeHtml(item.Owner) + '</td>'
										+ '<td width="10%" >' + (item.UserCount) + '</td>'
										+'<tr>'
				
				$trBefore.before(resultTableContent)
		}	


		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()			
			
			if(gridItems.length > 0)
				jQuery.each(gridItems, function (index, object) { addItem(jQuery('#firstRow'), object) })
			else
				jQuery('#firstRow').before('<tr><td colspan="5" align="center">Не найдено ни одного ОО</td></tr>')
		}

		function doSort(el, sortID) {			
			var isSortedUp = jQuery(el).hasClass('sortedUp')
			jQuery('.sortUp,.sortDown').remove().detach()
			if (isSortedUp)
				jQuery(el).after('<span class="sortDown"></span>')
			else
				jQuery(el).after('<span class="sortUp"></span>')
			jQuery(el).removeClass('sortedUp')
			if (isSortedUp)
				sortID = -sortID;
			else
				jQuery(el).addClass('sortedUp')
			currentSorting = sortID
			updateData()
		}

		var pageNumber = 0

		function movePager(pageID) {
			pageNumber = pageID
			updateData()
		}

		function prepareModel()
		{
			var model =
			{
				SortID: currentSorting,
				PageNumber: pageNumber
			}
			if (filterModel != null)
				model.Filter = filterModel
			return model
		}

		function updateData()
		{
			doPostAjax('<%= Url.Generate<InstitutionAdminController>(x => x.GetInstitutionList(null)) %>', JSON.stringify(prepareModel()), function (data) {
				if (!addValidationErrorsFromServerResponse(data, false)) {
					gridItems = data.Data.Institutions
					fillGrid()
					setFilterItemCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount)
					fillPager(data.Data.TotalPageCount, pageNumber)
				}
			})
		}

		function setFilterItemCount(filteredCount, totalCount)
		{
			var res = totalCount;
			if (filteredCount < totalCount)
				res = filteredCount + ' из ' + res;
			jQuery('#spAppCountField,#spAppCountFieldF').html(res)
		}


		jQuery(document).ready(function () {			
			//jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' })
		
			var regionNamesS = jQuery.map(regions, function(n,i) {return n.Name})
			autocompleteDropdown(jQuery("#Filter_RegionID"), {
				source: regionNamesS,
				delay : 200
			})
			autocompleteDropdown(jQuery("#Filter_RegionIDF"), {
				source: regionNamesS,
				delay : 200
			})
			autocompleteDropdown(jQuery("#Filter_Owner"), {
				source: ownerDepartments,
				delay : 200
			})
			jQuery('#Filter_ShortNameF').change(function() { jQuery('#Filter_FullName').val(jQuery(this).val()) }) //DO NOT TOUCH!!! IT'S MAGICK!
			jQuery('#Filter_RegionIDF').blur(function() { jQuery('#Filter_RegionID').val(jQuery(this).val()) })
			jQuery('#Filter_InstitutionTypeIDF').change(function() { jQuery('#Filter_InstitutionTypeID').val(jQuery(this).val()) })

			restoreFilter()
			updateData()
		})

		var regions = JSON.parse('<%= Html.Serialize(Model.Regions) %>')
		var ownerDepartments = JSON.parse('<%= Html.Serialize(Model.OwnerDepartments) %>')

		function toggleFilter(v)
		{
			//divFilterRegionF
			if (v == 1)
			{
                clearFilter();
				jQuery('#divFilterRegionF').hide()
				jQuery('#divFilterRegion').show()
			}
			else
			{
				clearFilter();
				jQuery('#divFilterRegionF').show()
				jQuery('#divFilterRegion').hide()
			}
		}

		var filterModel = null

		function applyFilter()
		{
			setTimeout(applyFilter1, 0)
		}

		function applyFilter1()
		{
			var regionID = 0
			//jQuery('#Filter_RegionIDF').blur()
            if (jQuery('#Filter_RegionID').val()!="")
			var selRegion = jQuery('#Filter_RegionID').val()
            if (jQuery('#Filter_RegionIDF').val()!="")
			var selRegion = jQuery('#Filter_RegionIDF').val() 
			jQuery.each(regions, function ()
			{
				if (this.Name == selRegion)
				{
					regionID = this.ID
					return false
				}
			})
			filterModel = {
				ShortName: jQuery('#Filter_ShortName').val(),
				Fullname: jQuery('#Filter_FullName').val(),
				INN: jQuery('#Filter_INN').val(),
				OGRN: jQuery('#Filter_OGRN').val(),
				InstitutionTypeID: jQuery('#Filter_InstitutionTypeID').val(),
				Owner: jQuery('#Filter_Owner').val(),
				RegionID: regionID
			}
			pageNumber = 0
			updateData()
		}

		function clearFilter()
		{
			filterModel = null
			jQuery('#Filter_ShortName').val('')
			jQuery('#Filter_ShortNameF').val('')
			jQuery('#Filter_FullName').val('')
			jQuery('#Filter_INN').val('')
			jQuery('#Filter_ORGN').val('')
			jQuery('#Filter_Owner').val('')
			jQuery('#Filter_InstitutionTypeID').val('0')
			jQuery('#Filter_InstitutionTypeIDF').val('0')
			jQuery('#Filter_RegionID').val('')
			jQuery('#Filter_RegionIDF').val('')

			applyFilter()
		}

		function saveFilter()
		{
			setCookie('instListFilter', JSON.stringify(prepareModel()), 1)
		}

		function restoreFilter()
		{
			var appListFilterString = getCookie('instListFilter')
			if (typeof appListFilterString != "undefined")
			{
				setCookie('instListFilter', '', -1)
				if (document.location.toString().indexOf('back=1') > 0)
				{
					var model = JSON.parse(appListFilterString)
					currentSorting = model.SortID
					pageNumber = model.PageNumber
					if (model.Filter != null)
					{
						jQuery('#Filter_ShortName').val(model.Filter.ShortName)
						jQuery('#Filter_ShortNameF').val(model.Filter.ShortNameF)
						jQuery('#Filter_FullName').val(model.Filter.FullName)
						jQuery('#Filter_INN').val(model.Filter.INN)
						jQuery('#Filter_Owner').val(model.Filter.Owner)
						jQuery('#Filter_ORGN').val(model.Filter.OGRN)
						jQuery('#Filter_InstitutionTypeID').val(model.Filter.InstitutionTypeID)
						jQuery('#Filter_InstitutionTypeIDF').val(model.Filter.InstitutionTypeID)
						
						var regName = ''
						jQuery.each(regions, function ()
						{
							if (this.ID == model.Filter.RegionID)
							{
								regName = this.Name
								return false
							}
						})

						jQuery('#Filter_RegionID').val(regName)
						jQuery('#Filter_RegionIDF').val(regName)

						filterModel = model.Filter
					}
					return true
				}
			}
			return false
		}
	</script>

</asp:Content>

