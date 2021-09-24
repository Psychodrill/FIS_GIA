<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.IndividualAchivementsViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<div id="content">
    <table class="gvuzDataGrid">
        <thead>
            <tr>
                <th><%= Html.LabelFor(x => x.FakedAchivement.UID)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IAName)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IAMark)%></th>
                <th><%= Html.LabelFor(x => x.FakedAchivement.IADocumentDisplay)%></th>
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
                                <a href="#" documentID="<%=item.IADocument.EntrantDocumentID %>" onclick="showDocument(this)"><%= item.IADocumentDisplay %></a>
                            </td>
                        </tr>   
                  <% }
               } %>
        </tbody>
    </table>
</div>
<div id="documentAddDialog"></div>

<script type="text/javascript">
    var getFileLink = '/Entrant/GetFile1'
    var getFileSep = '?'

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

    function showDocument(el) {
        var documentID = jQuery(el).attr('documentID');
        doView('<%= Url.Generate<EntrantController>(x => x.ViewDocument(0)) %>', 'entrantDocumentID=' + documentID);
    }
</script>