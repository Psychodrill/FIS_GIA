<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<GVUZ.Web.ViewModels.InstitutionInfo.InstitutionInfoYearDocumentViewModel>>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<% 
    bool isReadOnly = ViewData.ContainsKey("isReadOnly") && (bool)ViewData["isReadOnly"];
    const int fileNameBreakLen = 76;
%>

<table style="width: 100%;border-collapse: collapse;border: 1px solid #b2c5dc">
    <colgroup>
        <col style="width: 20%;border: 1px solid #b2c5dc" />
        <col style="border: 1px solid #b2c5dc;" />
    </colgroup>
    <% foreach (var yearDocs in Model.GroupBy(x => x.Year).OrderBy(x => x.Key)) { %>
    <tr>
        <td rowspan="<%= yearDocs.Count() %>" style="border-bottom: 1px solid #b2c5dc"><%= yearDocs.Key %></td>
        <td style="border-top: 1px solid #b2c5dc">
            <div class="<%= Html.GetExtensionFromFileName(yearDocs.First().FileName) %>" style="height: auto">
                <%= Html.ActionLink(Html.BreakStringByLength(yearDocs.First().DisplayName, fileNameBreakLen), "GetFile1", "Institution", new { fileID = yearDocs.First().FileId }, null) %>
                <% if (!isReadOnly) {%>
                    <button class="fileDelete" title="Удалить файл" data-attachment-id="<%= yearDocs.First().AttachmentId %>"></button>
                <% } %>
            </div>
        </td>

    </tr>
    <% foreach(var item in yearDocs.Skip(1)) { %>
    <tr>
        <td>
            <div class="<%= Html.GetExtensionFromFileName(item.FileName) %>" style="height: auto">
                <%= Html.ActionLink(Html.BreakStringByLength(item.DisplayName, fileNameBreakLen), "GetFile1", "Institution", new { fileID = item.FileId }, null) %>
                <% if (!isReadOnly) {%>
                    <button class="fileDelete" title="Удалить файл" data-attachment-id="<%= item.AttachmentId %>"></button>
                <% } %>
            </div>
        </td>
    </tr>
    <%} %>
    <% } %>
</table>
