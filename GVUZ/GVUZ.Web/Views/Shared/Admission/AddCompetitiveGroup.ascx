<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.CompetitiveGroupAddViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<table class="data" id="competitiveGroupAdd">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.Name) %></td>
			<td><%= Html.TextBoxExFor(x => x.Name) %></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CampaignID) %></td>
			<td><%= Html.DropDownListExFor(x => x.CampaignID, Model.Campaigns, new {onchange = "campaignChange()"}) %></td>
		</tr>
<%--
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationLevelID) %></td>
			<td><select id="Sel_EducationLevelID" onchange="edLevelChange()"></select></td>
		</tr>
--%>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CourseID) %></td>
			<td><select id="Sel_Course"></select></td>
		</tr>
		
	</tbody>
</table>

<script type="text/javascript">
	
	var campaignData = JSON.parse('<%= Html.Serialize(Model.Campaigns) %>')

	function campaignChange() {
		var campID = $('#CampaignID').val();
		$.each(campaignData, function (i, e)
		{
			if (e.ID == campID)
			{
				var res = '';
				for (var k = 0; k < e.Courses.length; k++)
					if (e.Campaign_Courses.indexOf(campID + "_" + e.Courses[k].ID) >= 0)
						res += '<option value="' + e.Courses[k].ID + '">' + e.Courses[k].Name + '</option>';
				$("#Sel_Course").html(res);
				return false;
			}
		});
	}


	function cg_save(callback)
	{
		if (revalidatePage(jQuery('#competitiveGroupAdd'))) return false;

		cg_submitData(callback);
	}

	function cg_submitData(callback)
	{ 
		var model = {
			Name: jQuery('#Name').val(),
			CampaignID: jQuery('#CampaignID').val(),
			CourseID: jQuery('#Sel_Course').val()
		}
		doPostAjax('<%= Url.Generate<AdmissionController>(c => c.SaveCompetitiveGroupAdd(null)) %>', JSON.stringify(model),
			function (data, status)
			{
				if (!addValidationErrorsFromServerResponse(data))
				{
					navigateTo(data.Data.GroupEditUrl);
				}
			})
	}
	campaignChange()
</script>
