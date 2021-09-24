<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AllowedDirectionAddViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<table class="data" id="allowedDirectionAdd">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationLevelID) %></td>
			<td><%= Html.DropDownListExFor(x => x.EducationLevelID, Model.EducationLevels, new {onchange = "doChangeEdData()"})%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.ParentDirectionID) %></td>
			<td><%= Html.DropDownListExFor(x => x.ParentDirectionID, Model.ParentDirections, new { onchange = "doChangeEdData()" })%></td>
		</tr>
		<tr>
			<td colspan="2">
				<div id="allowedDirectionsContainer" style="padding-top: 10px;padding-left: 10px;">Отсутствуют подходящие специальности</div>
			</td>
		</tr>
	</tbody>
</table>


<script type="text/javascript">
    function doChangeEdData() {
        //debugger;
		var edLevel = jQuery('#EducationLevelID option:selected').val();
		var pDir = jQuery('#ParentDirectionID option:selected').val();
		if(edLevel == 0 || pDir == 0)
			$('#allowedDirectionsContainer').html('Отсутствуют подходящие специальности')
		else
		{
			var model = {
				ParentDirectionID: pDir,
				EducationLevelID: edLevel
			};
			doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsGetAvailable(null)) %>', JSON.stringify(model), function (data)
            {
				if (data.IsError)
					alert(data.Message);
				else
				{
					var res = '';
					jQuery.each(data.Data, function (i, e)
					{
						res += '<input type="checkbox" dirID="' + e.ID + '"/>' +
						    '<span class="" onclick="">' + (e.Code == null ? '' : e.Code.trim() + '.' + e.QualificationCode.trim()) + '/' + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + '</span><br/>';
					});
					if (data.Data.length == 0)
						$('#allowedDirectionsContainer').html('Отсутствуют подходящие специальности');
					else
						$('#allowedDirectionsContainer').html(res);
				}
			});
		}
	}

    function doAddDirections() {
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        doAddDirection(dirs);
    }

	function doAddDirection(dirIDs)
	{
		var model = {
			ParentDirectionID: jQuery('#ParentDirectionID option:selected').val(),
			EducationLevelID: jQuery('#EducationLevelID option:selected').val(),
			DirectionIDs: dirIDs
		};
		doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsAddSave(null)) %>', JSON.stringify(model), function (data)
		{
			if (data.IsError)
				alert(data.Message);
			else
			{
				window.location.reload(1);
			}
		});
	}
</script>
