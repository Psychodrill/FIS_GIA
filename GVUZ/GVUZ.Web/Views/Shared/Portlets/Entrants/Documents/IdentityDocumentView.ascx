<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.IdentityDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelFor(m => m.IdentityDocumentTypeID) %></td>
                <td><%: Model.IdentityDocumentTypeName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber) %></td>
                <td><%: Model.DocumentSeries %> <%: Model.DocumentNumber %>
                    <span>&nbsp;&nbsp;&nbsp;</span>
                    <%= Html.TableLabelFor(m => m.DocumentDate) %> 
                    <%: (Model.DocumentDate.HasValue ? Model.DocumentDate.Value.ToString("dd.MM.yyyy") : "") %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.SubdivisionCode) %></td>
                <td><%: (Model.SubdivisionCode ?? "") %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentOrganization) %></td>
                <td><%: Model.DocumentOrganization %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.GenderTypeID) %></td>
                <td><%: Model.GenderTypeName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.NationalityTypeID) %></td>
                <td><%: Model.NationalityTypeName %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.BirthDate) %></td>
                <td><%: Model.BirthDate.ToString("dd.MM.yyyy") %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.BirthPlace) %></td>
                <td><%: Model.BirthPlace %></td>
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