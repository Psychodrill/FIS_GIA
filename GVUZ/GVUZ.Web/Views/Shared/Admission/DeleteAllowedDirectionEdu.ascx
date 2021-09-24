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

<table class="data" id="allowedDirectionExist">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationLevelID) %></td>
			<td><%= Html.DropDownListExFor(x => x.EducationLevelID, Model.EducationLevels, new {onchange = "doChangeEdData1()"})%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.ParentDirectionID) %></td>
			<td><%= Html.DropDownListExFor(x => x.ParentDirectionID, Model.ParentDirections, new { onchange = "doChangeEdData1()" })%></td>
		</tr>
		<tr>
			<td colspan="2">
				<div id="allowedDirectionsContainer" style="padding-top: 10px; padding-left: 10px;">Отсутствуют подходящие специальности</div>
			</td>
		</tr>
        <tr>
            <td colspan="2">
            Комментарий:
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <textarea style="width: 640px; height: 150px;" id = "delcomment" cols="91" rows="9" placeholder="Введите здесь причины удаления специальностей..."></textarea>
            </td>
        </tr>
	</tbody>
</table>

<script type="text/javascript">
	function doChangeEdData1() {
	    var edLevel = jQuery('#EducationLevelID option:selected').val();
	    var pDir = jQuery('#ParentDirectionID option:selected').val();
	    if (edLevel == 0 || pDir == 0)
	        $('#allowedDirectionsContainer').html('Отсутствуют подходящие специальности')
	    else {
	        var model =
            {
                ParentDirectionID: pDir,
                EducationLevelID: edLevel
            };
            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsGetExistingEdu(null)) %>', JSON.stringify(model), function (data) {
                if (data.IsError)
                    alert(data.Message);
                else {
                    var res = '';
                    jQuery.each(data.Data, function (i, e) {
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

    function doDeleteDirectionsEdu() {
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        doDeleteDirectionEdu(dirs);
    }

    function doDeleteDirectionEdu(dirIDs) {
        dirArrDel[dircDel] = [];
        commentdel[dircDel] = '';
        dirEduDel[dircDel] = 0;
        var edLevel = jQuery('#EducationLevelID option:selected').val();
        var pDir = jQuery('#ParentDirectionID option:selected').val();
        var dirs = $('#allowedDirectionsContainer').find('input[type="checkbox"]:checked').attrToArr('dirID');
        dirEduDel[dircDel] = edLevel;

        var dirTemp = (dirs + '').split(",");
        while (dirTemp.length > 0) {
            dirArrDel[dircDel].push(dirTemp.pop());
        }

        var model = {
            ParentDirectionID: pDir,
            EducationLevelID: edLevel
        };
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.AllowedDirectionsGetExistingEdu(null)) %>', JSON.stringify(model), function (data) {
            if (data.IsError)
                alert(data.Message);
            else {
                //alert(res2);
                res2 = $('#todeleteID').html();
                var tempr = '';
                var b;
                jQuery.each(data.Data, function (i, e) {
                    b = false;
                    for (var j = 0; j < dirArrDel[dircDel].length; j++) {
                        //alert(dirArrDel[dircDel][j]);
                        if (e.ID == dirArrDel[dircDel][j]) { b = true; break; }
                        /*if (b == true)
                        alert(e.ID + ' == ' + dirArrDel[dircDel][j]);
                        else
                        alert(e.ID + ' <> ' + dirArrDel[dircDel][j]);*/
                    }
                    if (b == true) {
                        tempr = '<div id="' + e.ID + 'href' + dircDel + '"><span class="" onclick="">' + (e.Code == null ? '' : e.Code.trim() + '.' + e.QualificationCode.trim()) + '/' + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + ' - ' + e.Period + '</span>&nbsp;&nbsp;&nbsp;<span ><a class="btnDelete" href="#" onclick="canceldel(' + e.ID + ',' + dircDel + ')">&nbsp;</a></span><br/></div>';
                        if (res2.indexOf((e.Code == null ? '' : e.Code.trim() + '.' + e.QualificationCode.trim()) + '/' + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + ' - ' + e.Period) == -1)
                            res2 += tempr;
                        //alert(res2);
                    }
                });
                if (b == true)
                    commentdel[dircDel] = $("textarea#delcomment").val();
                //res2 = res2 + '<br/>' + commentdel[dircDel] + '<br/>';
                if (data.Data.length == 0)
                /*$('#todeleteID').html('Отсутствуют подходящие специальности')*/;
                else
                    $('#todeleteID').html(res2);
                dircDel++;
            }
        });
    }


</script>
