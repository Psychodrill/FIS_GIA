<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% var m = new GVUZ.Web.ViewModels.AdmissionVolumeViewModel(); %>
	<div id="divViewDirection" style="padding: 5px;display:none;position:absolute" class="ui-widget ui-widget-content ui-corner-all outershadow">
		<table>
			<tbody>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(x => m.DirectionDisp.DirectionID) %></td>
					<td><span id="dirId"></span></td>
				</tr> 
				<tr>
					<td class="caption"><%= Html.TableLabelFor(x => m.DirectionDisp.DirectionName) %></td>
					<td><span id="dirName"></span></td>
				</tr>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(x => m.DirectionDisp.ParentCode) %></td>
					<td><span id="dirParentCode"></span></td>
				</tr>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(x => m.DirectionDisp.ParentName) %></td>
					<td><span id="dirParentName"></span></td>
				</tr>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(x => m.DirectionDisp.EducationLevelName) %></td>
					<td><span id="dirEdLevel"></span></td>
				</tr>
			</tbody>
		</table>
	</div>

<script language="javascript" type="text/javascript">
    function viewDirectionDetails(el, dirID) {
        var showPopup = function (data) { 
	        jQuery('#dirId').text(data.DirectionID);
	        jQuery('#dirName').text(data.DirectionName);
	        jQuery('#dirParentCode').text(data.ParentCode);
	        jQuery('#dirParentName').text(data.ParentName);
	        jQuery('#dirPeriod').text(data.PeriodName);
	        jQuery('#dirEdLevel').text(data.EducationLevelName);

	        var p = jQuery(el).offset()
	        jQuery('#divViewDirection').css('position', 'absolute').css('z-index', 1100)
                .animate({ top: p.top + jQuery(el).height() + 5, left: p.left + 10 }, jQuery('#divViewDirection').is(':visible') ? 300 : 0).fadeIn(300)
	    }
	    
		clearTimeout(dirTimerID)
		dirTimerID = setTimeout(function ()
		{
		    if (typeof cachedDirections != 'undefined') {
		        var cd = cachedDirections[dirID];
		        if (cd) {
		            showPopup(cd);
		            return;
		        }
		    }
			doPostAjax('<%= Url.Generate<AdmissionController>(x => x.GetDirectionInfo(null)) %>', "directionID=" + dirID, function (data)
			{
				if (data.Data != null)
				    showPopup(data.Data);
			}, "application/x-www-form-urlencoded")
		}, 300)
	}

	var dirTimerID = 0
	function hideDirectionDetails()
	{
		clearTimeout(dirTimerID)
		dirTimerID = setTimeout(function ()
		{
			jQuery('#divViewDirection').fadeOut(300)
		}, 700)
	}
</script>
