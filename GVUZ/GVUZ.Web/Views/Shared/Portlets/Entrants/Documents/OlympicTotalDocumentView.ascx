<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.OlympicTotalDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Model.Entrants.Documents" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div id="content">
    <table class="data">
        <thead>
            <tr>
                <th class="caption"></th><th></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID) %></td>
                <td><b><%: Model.DocumentTypeName %></b></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.UID) %></td>
                <td><%: Model.UID %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentSeries) %></td>
                <td><%: Model.DocumentSeries %>&nbsp;<%= Html.TableLabelFor(m => m.DocumentNumber) %><%: Model.DocumentNumber %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DiplomaTypeID) %></td>
                <td><%: Model.DiplomaTypeName %></td>
            </tr>
            <tr>
                <td class="caption"></td>
                <td>
                    <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                        <thead>
                            <tr>
                                <th style="width: 75%">
                                    <%= Html.LabelFor(x => x.SubjectDetails.SubjectName) %>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (OlympicTotalDocumentViewModel.SubjectBriefData data in Model.Subjects)
                               { %>
                                <tr>
                                    <td><%: data.SubjectName %></td>
                                </tr>
                            <% } %>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentDate) %></td>
                <td><%: Model.DocumentDate.HasValue ? Model.DocumentDate.Value.ToString("dd.MM.yyyy") : "" %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentOrganization) %></td>
                <td><%: Model.DocumentOrganization %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                <td><% if (Model.DocumentAttachmentID != Guid.Empty)
                       { %><a fileID="<%= Model.DocumentAttachmentID %>" class="getFileLink"><%: Model.DocumentAttachmentName %></a><% }
                       else
                       { %>Нет<% } %></td>
            </tr>
        </tbody>
    </table>
</div>