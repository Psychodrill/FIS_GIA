<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.OlympicDocumentViewModel>" %>
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
                <td><%: Model.DocumentSeries %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber) %></td>
                <td><%: Model.DocumentNumber %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DiplomaTypeID) %></td>
                <td><%: Model.DiplomaTypeName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.OlympicID) %></td>
                <td><%: Model.OlympicDetails.OlympicName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.SubjectNames) %></td>
                <td> <% foreach (string subjectName in Model.OlympicDetails.SubjectNames)
                        { %>
                    <%: subjectName %><br />
                    <% } %> </td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.LevelName) %></td>
                <td><%: Model.OlympicDetails.LevelName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.OlympicYear) %></td>
                <td><%: Model.OlympicDetails.OlympicYear %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.OrganizerName) %></td>
                <td><%: Model.OlympicDetails.OrganizerName %></td>
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