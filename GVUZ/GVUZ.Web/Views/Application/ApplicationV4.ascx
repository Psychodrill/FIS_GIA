<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationV4Model>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
    <table class="gvuzDataGrid">
        <thead>
            <tr>
                <th><%= Html.LabelFor(x => x.FakedAchivement.UID)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IAName)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IAMark)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IADocumentDisplay)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.isAdvantageRight)%></th>
            </tr>
        </thead>
        <tbody>
            <% if (Model.Items != null && Model.Items.Count() > 0)
               {
                   foreach (var item in Model.Items)
                   { %>
                        <tr>
                            <td>
                                <%= string.IsNullOrEmpty(item.UID) ? string.Empty : item.UID %>
                            </td>
                            <td>
                                <%= item.IAName %>
                            </td>
                            <td>
                                <%= item.IAMark.HasValue ? item.IAMark.Value.ToString() : string.Empty %>
                            </td>
                            <td>
                                <a href="#" documentID="<%=item.IADocument.EntrantDocumentID %>" onclick="showDocument(this)"><%= item.IADocument.DocumentTypeNameText %></a>
                            </td>
                            <td>
                                <%= item.isAdvantageRight == true ? "Да" : "" %>
                            </td>
                        </tr>   
                  <% }
               } %>
        </tbody>
    </table>
<table class="data tableApp2">
        <tbody>
            <tr><td class="caption" style="width:200px"><label>Суммарное количество баллов:</label></td><td><input type="text" class="view" style="width:60px" disabled="disabled" id="iachSum" value="<%=Model.IndividualAchivementsMarkStr%>" /></td></tr>
        </tbody>
    </table>
<div id="documentAddDialog"></div>
<div id="UniDDialog" style="display:none"></div>


<script type="text/javascript">
    var getFileLink = '/Entrant/GetFile1';
    var getFileSep = '?';

    var doView = function (navUrl, postData) {
        doPostAjax(navUrl, postData, function (data) {
            jQuery('#documentAddDialog').html(data)
            jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')) })
            jQuery('#documentAddDialog').dialog({
                modal: true,
                width: 800,
                title: "Просмотр документа",
                buttons: {
                    "Закрыть": function () { jQuery(this).dialog('close'); }
                },
                close: function () { }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html")
    }
    //-- Открываем диалог просмотра документа --
    function showDocument(el) {
        var EntrantDocumentID = jQuery(el).attr('documentID');
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
            $('#UniDDialog').html(data);
            $('#UniDDialog').dialog({
                modal: true,
                width: 800,
                title: "Дополнительный документ",
                buttons: { "Закрыть": function () { $(this).dialog('close'); } }
        , close: function () { }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }
</script>