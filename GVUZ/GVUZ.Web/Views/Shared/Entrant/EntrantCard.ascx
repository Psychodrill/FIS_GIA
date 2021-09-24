<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.EntrantInfoViewModelC>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="divPopupApplication"></div>

<table id="cardTable" class="tableAdmin" cellpadding="10">
	<tbody>
	<tr>
		<td class="caption big" style="width:12%;">
			<%= Html.LabelFor(x => x.BirthDate) %>:
		</td>
		<td style="width:24%;">
			<%= Html.CommonInputReadOnly(Model.BirthDate)%>
		</td>
		<td colspan="2" class="caption big" style="text-align:center">
			Документ, удостоверяющий личность
		</td>
	</tr>
	<tr>
		<td class="caption big">
			<%= Html.LabelFor(x => x.Gender) %>:
		</td>
		<td>
			<%= Html.CommonInputReadOnly(Model.Gender)%>
		</td>
		<td class="caption big" style="width:12%;">
			<%= Html.LabelFor(x => x.DocumentType) %>:
		</td>
		<td>
			<%= Html.CommonInputReadOnly(Model.DocumentType)%>
		</td>
	</tr>
	<tr>
		<td class="caption big">
			<%= Html.LabelFor(x => x.BirthPlace) %>:
		</td>
		<td>
			<%= Html.CommonInputReadOnly(Model.BirthPlace)%>
		</td>
		<td class="caption big">
			<%= Html.LabelFor(x => x.DocumentSeriesNumber) %>:
		</td>
		<td>
			<%= Html.CommonInputReadOnly(Model.DocumentSeriesNumber)%>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td>&nbsp;</td>
		<td class="caption">
			<%= Html.LabelFor(x => x.Uid) %>:
		</td>
		<td>        
		    <% if (GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection)) { %> 
            <%= Html.CommonInputReadOnly(Model.Uid) %> 
            <%} else { %>
			<div><%= Html.TextBoxExFor(m => m.Uid)%>
			&nbsp;<a href="#" id="saveUid" title="Сохранить UID" class="btnSave" style="margin-bottom: 6px;">&nbsp;</a>
			</div>	
            <% } %>
		</td>
	</tr>
	</tbody>
</table>

<table class="gvuzDataGrid tableStatement2">
	<thead>
		<tr>			
			<th style="text-align:center">
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationNumber)%>
			</th>
			<th>
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationStatus)%>
			</th>
            <th>
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationCampaign)%>
			</th>
			<th>
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationCompetitiveGroup)%>
			</th>
			<th>
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationRegistrationDate)%>
			</th>
			<th>
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationBenefit)%>
			</th>

			<th id="thAction" style="width:1%;text-align:center;">
				<%= Html.LabelFor(x => x.ApplicationDataNull.ApplicationID) %>
			</th>						
		</tr>					
	</thead>
	<tbody>
		<tr id="firstRow" style="display:none">									
		</tr>
	</tbody>
</table>

<script language="javascript" type="text/javascript">
	var entrantId = <%= Model.EntrantID %>;

	jQuery(function() {
		var cardTable = jQuery('#cardTable');
		var uid = jQuery('#Uid');
		jQuery('#saveUid', cardTable).live('click', function() {	
			doPostAjax('<%= Url.Generate<EntrantController>(x => x.SaveUid(null, null)) %>',
				'entrantId=' + entrantId + '&uid=' + uid.val(),
				function(data) {
					if (addValidationErrorsFromServerResponse(data, false)) {
						return false;
					}
					FogSoft.Common.showSuccessMessage('UID сохранён успешно.');
					return true;
				}
			, "application/x-www-form-urlencoded", null);
			return false;
		});
		
	});

	var gridItems = JSON.parse('<%= Html.Serialize(Model.Applications) %>');
	var currentSorting = null;
	var currentTab = 'e<%= Model.EntrantID %>';
	var UserReadonly = <%= GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection) ? "true" : "false" %>;

	function addItem($trBefore, item) {

		var className = $trBefore.prev().attr('class')
		if (className == 'trline2') className = 'trline1'; else className = 'trline2';
		var resultTableContent = ''

		resultTableContent += '<tr class="' + className + '" appID="' + item.ApplicationID + '">'
			+ '<td align="center">' + getViewAnchor(item.ApplicationID, item.ApplicationStatusID, item.ApplicationNumber) + '</td>'
				+ '<td>' + item.ApplicationStatus + '</td>'
					+ '<td>' + escapeHtml(item.ApplicationCampaign) + '</td>'
					+ '<td>' + escapeHtml(item.ApplicationCompetitiveGroup) + '</td>'
						+ '<td>' + item.ApplicationRegistrationDate + '</td>'
							+ '<td>' + escapeHtml(item.ApplicationBenefit ? item.ApplicationBenefit : '') + '</td>'
								+ '<td nowrap="nowrap" align="center">' + getEditAnchor(item.ApplicationID, item.ApplicationStatusID) + ' '
										+ getGoToPositionAnchor(item.ApplicationID)
											+ '</td></tr>'
		
		$trBefore.before(resultTableContent)
	}

	function getEditAnchor(applicationID, statusID) {
		if(!UserReadonly)
			if (statusID == 1 || statusID == 6 || statusID == 3 || statusID == 4 || statusID == 5)
				return '<a href="<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID='
					+ applicationID
						+ '&tabID=e<%=Model.EntrantID %>" class="btnEdit" title="Редактировать заявление"></a> '
		return ''
	}

	function getViewAnchor(applicationID, statusID, applicationNumber) {
		if (statusID != 1)
			return '<a href="#" title="Просмотр заявления" onClick="doApplicationView(' + applicationID + ');return false;">' + escapeHtml(applicationNumber) + '</a>'
		return applicationNumber
	}

	function getGoToPositionAnchor(applicationID) {
		return '<a href="<%= Url.Generate<InstitutionApplicationController>(x => x.FindApplicationInList(null)) %>?applicationID='
			+ applicationID + '" class="btnGoStat" title="К заявлению"></a>'
	}
	
	var viewApplicationID = 0
	function doApplicationView(applicationID) 
	{
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

	function fillGrid() {
		jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()

		jQuery.each(gridItems, function (index, object) {
			addItem(jQuery('#firstRow'), object)
		})
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


	jQuery(document).ready(function () {

		fillGrid()
		doAppViewFromReturn()
	})

</script>