<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.EntrantDocumentListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants.Documents" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<div id="documentAddDialog"></div>
<div id="content">
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentTypeName) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentSeriesNumber) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentDate) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentOrganization) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentAttachmentID) %>
                </th>
                <th style="width: 40px">
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (DocumentShortInfoViewModel doc in Model.Documents)
               { %>
                <% DocumentShortInfoViewModel doc1 = doc; %>
                <tr itemID="<%= doc.EntrantDocumentID %>">
                    <td><a href="" class="btnView"><%: doc.DocumentTypeName %></a></td>
                    <td><%: doc.DocumentSeriesNumber %></td>
                    <td><%: doc.DocumentOrganization %></td>
                    <td><%: doc.DocumentDate %></td>
                    <td><%= Url.GenerateLink<EntrantController>(x => x.GetFile1(doc1.DocumentAttachmentID), doc.DocumentAttachmentName) %></td>
                    <td><a href="" class="btnView">подробнее</a></td>
                </tr>
            <% } %>
        </tbody>
    </table>
</div>
<script type="text/javascript">

    var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>';
    var doView = function(navUrl, postData) {
        createdItem = null;
        doPostAjax(navUrl, postData, function(data) {
            jQuery('#documentAddDialog').html(data);
            var sep = getFileLink.indexOf('?') >= 0 ? '&' : '?';
            jQuery('a.getFileLink').each(function() { jQuery(this).attr('href', getFileLink + sep + 'fileID=' + jQuery(this).attr('fileID')); });
            jQuery('#documentAddDialog').dialog({
                modal: true,
                width: 800,
                title: jQuery('#dialogCaption').html(),
                buttons: {
                    "Закрыть": function() { jQuery(this).dialog('close'); }
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    };
    jQuery('.btnView').click(function() {
        var $tr = jQuery(this).parents('tr:first');
        var itemID = $tr.attr('itemID');
        doView('<%= Url.Generate<EntrantController>(x => x.ViewDocument(0)) %>', 'entrantDocumentID=' + itemID);
        return false;
    });
</script>