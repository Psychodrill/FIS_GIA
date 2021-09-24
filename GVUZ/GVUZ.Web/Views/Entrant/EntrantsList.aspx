<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.EntrantsListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Список абитуриентов
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Список абитуриентов
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement notabs">
		<div id="divPopupApplication"></div>
		
		<div class="tableHeader" id="divFilterRegionF">
			<div class="hideTable nonHideTable" style="float:left;"><span style="cursor:default;">Быстрый поиск</span></div>
			<div id="spAppCountF" class="appCount">Количество абитуриентов: <span id="spAppCountFieldF"></span></div>
			<div id="divFilterF">
            <br/>
			<table class="tableForm">
				<tbody>
					<tr>
						<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EntrantLastName) %></td>
						<td><input type="text" id="Filter_EntrantLastNameF" /></td>
						<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.ApplicationNumber) %></td>
						<td><input type="text" id="Filter_ApplicationNumberF" /></td>
						<td nowrap="nowrap">
							<input type="button" id="btnApplyFilterF" class="button primary" onclick="applyFilter()" value="Найти"/>
							<input type="button" id="btnClearFilterF" class="button" onclick="clearFilter()" value="Сбросить фильтр"/>
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
		<div id="spAppCount" class="appCount">Количество абитуриентов: <span id="spAppCountField"></span></div>
	</div>
	<div id="divFilter">
	<table class="tableForm">
		<tbody><br/>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.ApplicationNumber) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.ApplicationNumber) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EntrantLastName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.EntrantLastName) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.DateBegin) %></td>
				<td><%= Html.DatePickerFor(x => x.Filter.DateBegin) %>&nbsp; <%= Html.TableLabelFor(x => x.Filter.DateEnd, new {@class="labelsInside"}) %> <%= Html.DatePickerFor(x => x.Filter.DateEnd) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EntrantFirstName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.EntrantFirstName) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.CompetitiveGroupName) %></td>
				<td id="CGroupTD"><%= Html.TextBoxExFor(x => x.Filter.CompetitiveGroupName) %></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EntrantMiddleName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.EntrantMiddleName) %></td>
			</tr>
			<tr>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.ApplicationStatusID) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.ApplicationStatusID, new {@readonly = "readonly"}) %>
				<div id="divApplicationStatuses" style="padding: 5px;display:none;position:absolute" class="ui-widget ui-widget-content ui-corner-all">fdsfdsfdss</div></td>
				<td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.EntrantDocSeries) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.EntrantDocSeries, new {style="width: 50px"}) %>&nbsp; <%= Html.TableLabelFor(x => x.Filter.EntrantDocNumber, new {@class="labelsInside"}) %> <%= Html.TextBoxExFor(x => x.Filter.EntrantDocNumber, new { style = "width: 150px" })%></td>
			</tr>
			<tr>
                <td class="labelsInside"><%= Html.TableLabelFor(x => x.Filter.CampaignName) %></td>
				<td><%= Html.TextBoxExFor(x => x.Filter.CampaignName) %></td>
				<td colspan="2" style="text-align: center;">
                    <input type="button" id="btnApplyFilter" class="button primary" onclick="applyFilter()" value="Найти"/>
				    <input type="button" id="btnClearFilter" class="button" onclick="clearFilter()" value="Сбросить фильтр"/>
				    <input type="button" id="btnSwitchFilter" class="button" onclick="toggleFilter(0)" value="Скрыть область"/>
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
							<span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.EntrantDataNull.FullName) %></span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.EntrantDataNull.IdentityDocument) %></span>
						</th>
						<th style="text-align:center">
							<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationNumber)%>
						</th>
						<th>
							<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationRegistrationDate)%>
						</th>
                        <th>
                            <%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationCampaign)%>
                        </th>
						<th>
							<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationCompetitiveGroup)%>
						</th>
						<th>
							<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationStatus)%>
						</th>
						<th id="thAction" style="width:1%;text-align:center">
							<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationID) %>
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
		var currentTab = 11
		var UserReadonly = <%= GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection) ? "true" : "false" %>
		
		function addItem($trBefore, item) {							

				var className = jQuery('[itemID]').last().attr('class')
				if (className == 'trline2') className = 'trline1'; else className = 'trline2';
				var resultTableContent = ''

				if (item.Applications.length == 0)
				{
					resultTableContent += '<tr itemID="' + item.EntrantID + '" class="' + className + '">'
											+ '<td align="left" rowspan="' + item.Applications.length + '">'
											+ getEntrantCardAnchor(item.EntrantID, item.FullName) + '</td>'
											+ '<td align="left" rowspan="' + item.Applications.length + '">' + escapeHtml(item.IdentityDocument) + '</td><td colspan="5"></td><tr>'
				}

				for (var i = 0; i < item.Applications.length; i++) {																

					if (i == 0) {
						resultTableContent += '<tr itemID="' + item.EntrantID + '" class="' + className + '" appID="' + item.Applications[i].ApplicationID + '" >'
										+ '<td align="left" rowspan="' + item.Applications.length + '">'
										+ getEntrantCardAnchor(item.EntrantID, item.FullName) + '</td>'
										+ '<td align="left" rowspan="' + item.Applications.length + '">' + escapeHtml(item.IdentityDocument) + '</td>'
					} else {
						if (className == 'trline2') className = 'trline1'; else className = 'trline2';
						resultTableContent += '<tr class="' + className + '" appID="' + item.Applications[i].ApplicationID + '" >'
					}
					
					resultTableContent += '<td align="center">' + getViewAnchor(item.Applications[i].ApplicationID, item.Applications[i].ApplicationStatusID, item.Applications[i].ApplicationNumber) + '</td>'
										+ '<td>' + item.Applications[i].ApplicationRegistrationDate + '</td>'
                                        + '<td>' + escapeHtml(item.Applications[i].ApplicationCampaign) + '</td>'
										+ '<td>' + escapeHtml(item.Applications[i].ApplicationCompetitiveGroup) + '</td>'
										+ '<td>' + item.Applications[i].ApplicationStatus + '</td>'
										+ '<td nowrap="nowrap" align="center">' + getEditAnchor(item.Applications[i].ApplicationID, item.Applications[i].ApplicationStatusID) + ' '
										+ getGoToPositionAnchor(item.Applications[i].ApplicationID)
										+ '</td></tr>'
				}
				
				$trBefore.before(resultTableContent)
		}	

		function getEditAnchor(applicationID, statusID) {
			if(!UserReadonly)
				if (statusID == 1 || statusID == 6 || statusID == 3 || statusID == 4 || statusID == 5)
					return '<a href="<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID='
							+ applicationID
							+ '&tabID=11" class="btnEdit" title="Редактировать заявление" onclick="doApplicationEditByAppID(' + applicationID + ');return false;"></a> '
			return ''
		}

		function getViewAnchor(applicationID, statusID, applicationNumber) {
			if ( statusID != 1 )
				return '<a href="#" title="Просмотр заявления" onClick="doApplicationView(' + applicationID + ');return false;">' + escapeHtml(applicationNumber) + '</a>'
			return applicationNumber
		}

		function getGoToPositionAnchor(applicationID) {
			return '<a href="<%= Url.Generate<InstitutionApplicationController>(x => x.FindApplicationInList(null)) %>?applicationID='
							+ applicationID + '" class="btnGoStat" title="Переход к заявлению в списке"></a>'
		}

		function getEntrantCardAnchor(entrantID, fullName) {
			return '<a href="<%=Url.Generate<EntrantController>(c=>c.EntrantInfo(null)) %>?entrantID=' 
			+ entrantID + '" title="Просмотр карточки абитуриента">' 
			+ escapeHtml(fullName) + '</span>'
		}

		function doApplicationEditByAppID(appID)
		{
			saveFilter()
			window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID='
					+ appID + '&tabID=' + currentTab
		}

		var viewApplicationID = 0
		function doApplicationView(applicationID)
		{
			saveFilter()
			viewApplicationID = applicationID

			doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationViewPopup(0, 0, false)) %>', 'tabID=0&applicationID=' + applicationID, function (data) {				
				jQuery('#divPopupApplication').html(data);
				jQuery('#divPopupApplication').dialog({
					modal: true,
					width: 900,
					title: 'Просмотр заявления',
					buttons: {
						"Закрыть": function () { jQuery(this).dialog('close'); }
					}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")
			return false
		}

		function doAppViewFromReturn()
		{
			var appID = getCookie('viewAppID')
			if (typeof appID != "undefined")
			{
				setCookie('viewAppID', '', -1)
				if (document.location.toString().indexOf('back=1') > 0)
					if (jQuery('.gvuzDataGrid tbody tr[appID="' + appID + '"]').length > 0)
						doApplicationView(appID)
			}
		}


		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()			
			
			if(gridItems.length > 0)
				jQuery.each(gridItems, function (index, object) { addItem(jQuery('#firstRow'), object) })
			else
				jQuery('#firstRow').before('<tr><td colspan="7" align="center">Не найдено ни одного абитуриента</td></tr>')
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

			doPostAjax('<%= Url.Generate<EntrantController>(x => x.GetEntrantsList(null)) %>', JSON.stringify(prepareModel()), function (data) {
				if (!addValidationErrorsFromServerResponse(data, false)) {
					gridItems = data.Data.Entrants
					fillGrid()
					setFilterAppCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount)
					fillPager(data.Data.TotalPageCount, pageNumber)
					doAppViewFromReturn()
				}
			})
		}

		function setFilterAppCount(filteredCount, totalCount)
		{
			var res = totalCount;
			if (filteredCount < totalCount)
				res = filteredCount + ' из ' + res;
			jQuery('#spAppCountField,#spAppCountFieldF').html(res)
		}


		jQuery(document).ready(function () {			
			jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' })
		
			var compGroupNames = jQuery.map(competitiveGroups, function(n,i) {return n.Name})
			autocompleteDropdown(jQuery("#Filter_CompetitiveGroupName"), {
				source: compGroupNames,
				delay : 200
			})

            var campaignNames = jQuery.map(campaigns, function(n,i) {return n.Name})
			autocompleteDropdown(jQuery("#Filter_CampaignName"), {
				source: campaignNames,
				delay : 200
			})

			jQuery('#Filter_EntrantLastNameF').change(function() { jQuery('#Filter_EntrantLastName').val(jQuery(this).val()) })
			jQuery('#Filter_ApplicationNumberF').change(function() { jQuery('#Filter_ApplicationNumber').val(jQuery(this).val()) })

			fillApplicationStatuses()
            applyFilter1()
			restoreFilter()
			updateData()
		})



		var competitiveGroups = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroups) %>')
        var campaigns = JSON.parse('<%= Html.Serialize(Model.Campaigns) %>')
		var applicationStatuses = JSON.parse('<%= Html.Serialize(Model.ApplicationStatuses) %>')

		function toggleFilter(v)
		{
			//divFilterRegionF
			if (v == 1)
			{
				jQuery('#divFilterRegionF').hide()
				jQuery('#divFilterRegion').show()
			}
			else
			{
				jQuery('#divFilterRegionF').show()
				jQuery('#divFilterRegion').hide()
			}
		}

		var filterModel = null

		function applyFilter()
		{
			applyFilter1()  
            updateData()
            saveFilter()         
		}

		function applyFilter1()
		{
			/*var compGroupID = 0
			var selCompGroupName = jQuery('#Filter_CompetitiveGroupName').val()
			jQuery.each(competitiveGroups, function ()
			{
				if (this.Name == selCompGroupName)
				{
					compGroupID = this.ID
					return false
				}
			})

            var campID = 0
            var selCampName = jQuery('#Filter_CampaignName').val()
			jQuery.each(campaigns, function ()
			{
				if (this.Name == selCampName)
				{
					campID = this.ID
					return false
				}
			})*/

			var stIDs = []
			var v = jQuery('#Filter_ApplicationStatusID').attr('stIDs')
			if (v != '' && (typeof v != "undefined"))
				stIDs = v.split(',')
			filterModel = {
				DateBegin: jQuery('#Filter_DateBegin').val(),
				DateEnd: jQuery('#Filter_DateEnd').val(),
				ApplicationNumber: jQuery('#Filter_ApplicationNumber').val(),
				EntrantLastName: jQuery('#Filter_EntrantLastName').val(),
				EntrantFirstName: jQuery('#Filter_EntrantFirstName').val(),
				EntrantMiddleName: jQuery('#Filter_EntrantMiddleName').val(),
				EntrantDocSeries: jQuery('#Filter_EntrantDocSeries').val(),
				EntrantDocNumber: jQuery('#Filter_EntrantDocNumber').val(),
				CompetitiveGroupName: jQuery('#Filter_CompetitiveGroupName').val(),
                CampaignName: jQuery('#Filter_CampaignName').val(),
				ApplicationStatusID: stIDs
			}

			pageNumber = 0            
		}

		function clearFilter()
		{
			filterModel = null
			jQuery('#Filter_DateBegin').val('')
			jQuery('#Filter_DateEnd').val('')
			jQuery('#Filter_EntrantLastName').val('')
			jQuery('#Filter_EntrantLastNameF').val('')
			jQuery('#Filter_EntrantFirstName').val('')
			jQuery('#Filter_EntrantMiddleName').val('')
			jQuery('#Filter_EntrantDocSeries').val('')
			jQuery('#Filter_EntrantDocNumber').val('')
			jQuery('#Filter_CompetitiveGroupName').val('')
			jQuery('#Filter_ApplicationNumber').val('')
			jQuery('#Filter_ApplicationNumberF').val('')
			jQuery('#Filter_CampaignName').val('')

            jQuery('#divApplicationStatuses input').removeAttr('checked')
			jQuery('#Filter_ApplicationStatusID').attr('stIDs', '')
			jQuery('#Filter_ApplicationStatusID').val('')
			applyFilter()
		}

		function saveFilter()
		{
			setCookie('appListFilter', JSON.stringify(prepareModel()))
		}

		function restoreFilter()
		{
			var appListFilterString = getCookie('appListFilter')
			if (typeof appListFilterString != "undefined")
			{
				//setCookie('appListFilter', '', -1)

					var model = JSON.parse(appListFilterString)
					currentSorting = model.SortID
					pageNumber = model.PageNumber
					if (model.Filter != null)
					{
						jQuery('#Filter_DateBegin').val(model.Filter.DateBegin)
						jQuery('#Filter_DateEnd').val(model.Filter.DateEnd)
						jQuery('#Filter_EntrantLastName').val(model.Filter.EntrantLastName)
						jQuery('#Filter_EntrantFirstName').val(model.Filter.EntrantFirstName)
						jQuery('#Filter_EntrantMiddleName').val(model.Filter.EntrantMiddleName)
						jQuery('#Filter_EntrantDocSeries').val(model.Filter.EntrantDocSeries)
						jQuery('#Filter_EntrantDocNumber').val(model.Filter.EntrantDocNumber)
						jQuery('#Filter_ApplicationNumber').val(model.Filter.ApplicationNumber)
						jQuery('#Filter_ApplicationNumberF').val(model.Filter.ApplicationNumber)
						jQuery('#Filter_EntrantLastNameF').val(model.Filter.EntrantLastName)
                        

						/*var compGroupName = ''
						jQuery.each(competitiveGroups, function ()
						{
							if (this.ID == model.Filter.CompetitiveGroupName)
							{
								compGroupName = this.Name
								return false
							}
						})

                        var campName = ''
						jQuery.each(campaigns, function ()
						{
							if (this.ID == model.Filter.CampaignName)
							{
								campName = this.Name
								return false
							}
						})*/

						/*jQuery('#Filter_CompetitiveGroupName').val(compGroupName)
                        jQuery('#Filter_CampaignName').val(campName)*/

						var stIDs = model.Filter.ApplicationStatusID
						var stIDsStr = ''
						jQuery.each(stIDs, function ()
						{
							stIDsStr += this
							jQuery('#divApplicationStatuses input[stID="' + this + '"]').attr('checked', 'checked')
						})

						jQuery('#Filter_ApplicationStatusID').attr('stIDs', stIDsStr)
						filterModel = model.Filter
					}
					return true
				
			}
			return false
		}


	function fillApplicationStatuses()
	{
		var res = ''
		for(var i = 0; i < applicationStatuses.length; i++)
		{
			res += '<input type="checkbox" id="cbAppSt_' + applicationStatuses[i].ID + '" stID="' + applicationStatuses[i].ID + '">'
			+ '<label for="cbAppSt_' + applicationStatuses[i].ID + '">'
				+ applicationStatuses[i].Name + "</label><br/>"
		}
		jQuery('#divApplicationStatuses').html(res)
		jQuery('#Filter_ApplicationStatusID').after('<img title="" id="Filter_ApplicationStatusID_Sel" class="ui-datepicker-trigger gvuz-calendar-icon" alt="..." src="' + absoluteAppPath + 'Resources/Images/ddl.png"/>')
		jQuery('#Filter_ApplicationStatusID').next().click(function() {jQuery(this).prev().focus()})
		jQuery('#Filter_ApplicationStatusID').focus(function()
		{
			var p = jQuery(this).position()
			jQuery('#divApplicationStatuses').css('position', 'absolute').css('z-index', 1100).css('top', p.top + jQuery(this).height() + 20)
			.css('left', p.left).css('width', jQuery(this).width()).fadeIn(300)
		})
		jQuery('body').click(function(evt)
		{
			if(evt.target == null || evt.target.parentNode == null) return
			if(evt.target.id == 'divApplicationStatuses' || evt.target.parentNode.id == 'divApplicationStatuses'
				|| evt.target.id == 'Filter_ApplicationStatusID'
				|| evt.target.id == 'Filter_ApplicationStatusID_Sel')
				return
			var res1 = ''
			var res2 = ''
			jQuery('#divApplicationStatuses input').each(function() {
				if(jQuery(this).attr('checked'))
				{
					res1 += jQuery(this).attr('stID') + ','
					res2 += jQuery(this).next().html() + ', '
				}
			})
			if(res1.length > 0)
				res1 = res1.substr(0, res1.length - 1)
			if(res2.length > 0)
				res2 = res2.substr(0, res2.length - 2)
			jQuery('#Filter_ApplicationStatusID').attr('stIDs', res1)
			jQuery('#Filter_ApplicationStatusID').val(res2)
			jQuery('#divApplicationStatuses').fadeOut(300)
		})
	}

    document.getElementById('Filter_CampaignName').onblur = function () {
    var re = $(this).attr('value');
    getCGroupList();
    };

    function getCGroupList()
    {
        //var campID = 0
        var selCampName = jQuery('#Filter_CampaignName').val()
		/*jQuery.each(campaigns, function ()
		{
			if (this.Name == selCampName)
			{
				campID = this.ID
				return false
			}
		})*/
        cmodel = 
        {
            campaignName: selCampName
        };
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.GetCGroupList(null)) %>', JSON.stringify(cmodel), function (data)
        {
            if (data.IsError) {/*alert(data.Message);*/}
            else
            {
            var st = data.Data;
            var filterCompetitiveGroups = st;
                $('#CGroupTD').html('<%= Html.TextBoxExFor(x => x.Filter.CompetitiveGroupName) %>');
                autocompleteDropdown(jQuery('#Filter_CompetitiveGroupName'), { source: filterCompetitiveGroups, minLength: 1 })
            }
        })
    }
	</script>

</asp:Content>

