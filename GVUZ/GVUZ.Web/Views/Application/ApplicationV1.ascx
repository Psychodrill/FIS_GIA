<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationV1Model>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<script type="text/javascript" language="javascript">

    var Application=<%=Model.ApplicationID%>;
    var EntrantID = <%=Model.EntrantID%>;
    getEntrantDocuments(EntrantID);
    
    function getEntrantDocuments(EntrantID) {
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEntrantDocuments(null)) %>', JSON.stringify({ EntrantID: EntrantID }),
        function (data) {
            if (!data.IsError) {
                for (i = 0; i < data.Data.length; i++) {
                    renderDocument($('#trEntityDocsAddNew'), data.Data[i], 0);
                }
                if (data.Data.length == 0) {
                    $('#EntityDocs').hide();
                } else {
                    $('#EntityDocs').show();
                }
            } else { // Обработка ошибки.
                res = false;
                infoDialog("Не удалось поулчить список документов. "); //  + data.Message
            }
        });
    }

    function renderDocument($trBefore, item) {
        var iTr = $trBefore.parent().find('tr[itemID=' + item.EntrantDocumentID + ']');
        if (iTr.length > 0) { // Обновление 
            updTrDocument($(iTr), item);
        } else { // Добавление
            var className = $trBefore.prev().attr('class');
            var res = renderTrDocument(item, className);
            $trBefore.before(res);
            $('#EntityDocs').show();
        }
    }

    function updTrDocument($iTr, item) {
        var trin = '<td><span class="btnView linkSumulator" onclick="ViewDocument(' + item.EntrantDocumentID + ')">' + escapeHtml(item.DocumentTypeName) + '</a></td>'
            + '<td>' + escapeHtml(item.DocumentSeriesNumber) + '</td>'
            + '<td>' + item.DocumentDate + '</td>'
            + '<td>' + escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td>';

        $iTr.html(trin);
    }
    function renderTrDocument(item, className) {
    if (className == 'trline1'){ className = 'trline2'; }else{ className = 'trline1';}
    var res = '<tr itemID="' + item.EntrantDocumentID + '" class="' + className + '">'
                +'<td><span class="btnView linkSumulator" onclick="ViewDocument('+item.EntrantDocumentID+')">' + escapeHtml(item.DocumentTypeName)+ '</a></td>'
                +'<td>' + escapeHtml(item.DocumentSeriesNumber) + '</td>'
                +'<td>' + item.DocumentDate + '</td>'
                +'<td>'	+ escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td>'
              +'</tr>';
    return res;
  }

  //-- Открываем диалог просмотра документа --
  function ViewDocument(EntrantDocumentID) {
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
      $('#UniDDialog').html(data);
      $('#UniDDialog').dialog({
        modal: true,
        width: 800,
        title: "Дополнительный документ",
        buttons: {  "Закрыть": function () { $(this).dialog('close'); }}
        ,  close: function () { }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");

  }
</script>
<div id="UniDDialog" style="display:none"></div>
<div id="content" style="margin-right: 10px;">
    <table class="data tableApp2">
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.EntrantLastName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.EntrantLastName)%></td>
            <td class="caption"><%= Html.TableLabelFor(x => x.GenderName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.GenderName)%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.EntrantFirstName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.EntrantFirstName)%></td>
            <td class="caption"><%= Html.TableLabelFor(x => x.CountryName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.CountryName)%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.EntrantMiddleName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.EntrantMiddleName)%></td>
            <td class="caption"><%= Html.TableLabelFor(x => x.BirthPlace)%></td>
            <td><%= Html.CommonInputReadOnly(Model.BirthPlace)%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.DocumentTypeName)%></td>
            <td><%= Html.CommonInputReadOnly(Model.DocumentTypeName)%></td>
            <td class="caption" rowspan="4"><%= Html.TableLabelFor(m => m.CustomInformation) %></td>
            <td rowspan="4"><%= Html.CommonTextAreaReadOnly(Model.CustomInformation) %></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.DocumentBirthDate)%></td>
            <td>
                <%if (Model.DocumentBirthDate != null) { %>
                <%= Html.CommonInputReadOnly(Model.DocumentBirthDate.Value.ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture))%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.DocumentSeriaNumber)%></td>
            <td><%= Html.CommonInputReadOnly(Model.DocumentSeriaNumber)%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.DocumentOrganization)%></td>
            <td><%= Html.CommonInputReadOnly(Model.DocumentOrganization)%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.DocumentDate)%></td>
            <td><%= Html.CommonInputReadOnly((Model.DocumentDate.HasValue ? Model.DocumentDate.Value.ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) : ""))%></td>
            <td class="caption"><%= Html.TableLabelFor(x => x.NeedHostel)%></td>
            <td><%= Html.CheckBoxFor(x => x.NeedHostel, new { disabled = "disabled" })%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.AttachmentID)%></td>
            <td><%= Url.GenerateLinkIf<EntrantController>(x => x.GetFile1(Model.AttachmentFileID), Model.AttachmentName, Model.AttachmentFileID != Guid.Empty)%></td>
        </tr>
    </table>
    <br/>
    <br/>
    <table id="EntityDocs" class="gvuzDataGrid" cellpadding="3" >
			<thead>
				<tr><th>Вид документа</th>
					<th>Серия и номер документа</th>
					<th>Дата выдачи</th>
					<th>Кем выдан</th>
				</tr>
			</thead>
			<tbody id="EntityDocsTBody">
				<tr id="trEntityDocsAddNew" style="display:none"></tr>
			</tbody>
		</table>
</div>