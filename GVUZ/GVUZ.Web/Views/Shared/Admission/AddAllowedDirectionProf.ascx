<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AllowedDirectionAddViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<style type="text/css">
	textarea{
	    resize:none;
	}
</style>

<table class="data" id="allowedDirectionAdd">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationLevelID) %></td>
			<td><%= Html.DropDownListExFor(x => x.EducationLevelID, Model.EducationLevels, new {})%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.ParentDirectionID) %></td>
			<td><%= Html.DropDownListExFor(x => x.ParentDirectionID, Model.ParentDirections, new {})%></td>
		</tr>
		<tr>
			<td colspan="2">
				<div id="allowedDirectionsContainer" style="padding-top: 10px;padding-left: 10px;">Отсутствуют подходящие специальности</div>
			</td>
		</tr>
        <tr>
            <td colspan="2">
            Комментарий:
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <textarea style="width: 640px; height: 150px;" id = "addcomment" cols="91" rows="9" placeholder="Введите здесь причины добавления специальностей..."></textarea>
            </td>
        </tr>
	</tbody>
</table>

<script type="text/javascript">
	function doChangeEdData() 
    {
		var edLevel = jQuery('#EducationLevelID option:selected').val();
		var pDir = jQuery('#ParentDirectionID option:selected').val();
		if(edLevel == 0 || pDir == 0)
			$('#allowedDirectionsContainer').html('Отсутствуют подходящие специальности')
		else
		{
			var model = 
            {
				ParentDirectionID: pDir,
				EducationLevelID: edLevel
			};
			doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsGetAvailableEdu(null)) %>', JSON.stringify(model), function (data)
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
		            //alert(res);
					if (data.Data.length == 0)
						$('#allowedDirectionsContainer').html('Отсутствуют подходящие специальности');
					else
						$('#allowedDirectionsContainer').html(res);
				}
			});
		}
	}

	

    /*function doAddDirections() {
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        doAddDirection(dirs);
    }*/

    function doAddDirections() {
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        doAddDirection(dirs);
    }

	/*function doAddDirection(dirIDs)
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
    }*/


    function doAddDirection(dirIDs) {
        dirArrAdd[dircAdd] = [];
        commentadd[dircAdd] = '';
        dirEduAdd[dircAdd] = 0;
        var edLevel = jQuery('#EducationLevelID option:selected').val();
        var pDir = jQuery('#ParentDirectionID option:selected').val();
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        dirEduAdd[dircAdd] = edLevel;
        var dirTemp = (dirs + '').split(",");
        while (dirTemp.length > 0) {
            dirArrAdd[dircAdd].push(dirTemp.pop());
        }

        var model = {
            ParentDirectionID: pDir,
            EducationLevelID: edLevel
        };

        //$('#allowedID').html(res1);

        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsGetAvailableEdu(null)) %>', JSON.stringify(model), function (data) {
            if (data.IsError)
                alert(data.Message);
            else {
                //alert(res1);
                res1 = $('#allowedID').html();
                var tempr = '';
                var b;
                jQuery.each(data.Data, function (i, e) {
                    b = false;
                    for (var j = 0; j < dirArrAdd[dircAdd].length; j++) {
                        //alert(dirArrAdd[dircAdd][j]);
                        if (e.ID == dirArrAdd[dircAdd][j]) { b = true; break; }
                        /*if (b == true)
                        alert(e.ID + ' == ' + dirArrAdd[dircAdd][j]);
                        else
                        alert(e.ID + ' <> ' + dirArrAdd[dircAdd][j]);*/
                    }
                    if (b == true) {
                        tempr = '<div id="' + e.ID + 'href' + dircAdd + '"><span class="" onclick="">' + (e.Code == null ? '' : e.Code.trim() + '.' + e.QualificationCode.trim()) + '/' + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + ' - ' + e.Period + '</span>&nbsp;&nbsp;&nbsp;<span><a class="btnDelete" href="#" onclick="canceladd(' + e.ID + ',' + dircAdd + ')">&nbsp;</a></span><br/></div>';
                        if (res1.indexOf((e.Code == null ? '' : e.Code.trim() + '.' + e.QualificationCode.trim()) + '/' + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + ' - ' + e.Period) == -1)
                            res1 += tempr;
                        //alert(res1);
                    }
                });
                if (b == true)
                    commentadd[dircAdd] = $("textarea#addcomment").val();
                //res1 = res1 + '<br/>' + commentadd[dircAdd] + '<br/>';
                if (data.Data.length == 0)
                /*$('#allowedID').html('Отсутствуют подходящие специальности')*/;
                else
                    $('#allowedID').html(res1);
                //res1 += '<br/>';
                //alert(res1);
                //$('#divAllowedDirectionAdd').close;
                dircAdd++;
            }
        });
    }
</script>
