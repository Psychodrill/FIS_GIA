<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.EGEDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber) %></td>
                <td><%: Model.DocumentNumber %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.TypographicNumber) %></td>
                <td><%: Model.TypographicNumber %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentYear) %></td>
                <td><%: Model.DocumentYear.HasValue ? Model.DocumentYear.Value.ToString() : "" %></td>
            </tr>
            <%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Decision) %></td>
				<td><%: Model.Decision %></td>
			</tr>--%>
            <%--
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.ProfessionTypeID) %></td>
				<td><%: Model.ProfessionTypeName %></td>
			</tr>
--%>
            <%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.DecisionNumber) %></td>
				<td><%: Model.DecisionNumber%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.DecisionDate) %></td>
				<td><%: Model.DecisionDate.HasValue ? Model.DecisionDate.Value.ToString("dd.MM.yyyy") : "" %></td>
			</tr>--%>
            <tr>
                <td class="caption"></td>
                <td>
                    <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                        <thead>
                            <tr>
                                <th style="width: 75%">
                                    <%= Html.LabelFor(x => x.SubjectDetails.SubjectName) %>
                                </th>
                                <th style="width: 25%">
                                    <%= Html.LabelFor(x => x.SubjectDetails.Value) %>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (EGEDocumentViewModel.SubjectData data in Model.Subjects)
                               { %>
                                <tr>
                                    <td><%: data.SubjectName %></td>
                                    <td align="center"><%: data.Value %></td>
                                </tr>
                            <% } %>
                        </tbody>
                    </table>
                </td>
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