<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OlympicDetailsViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
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
				<td class="caption"><%= Html.TableLabelFor(m => m.OlympicID) %></td>
				<td><%: Model.OlympicDetails.OlympicName %></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.SubjectNames) %></td>
				<td> <% foreach (var subjectName in Model.OlympicDetails.SubjectNames) {%>
					<%: subjectName %><br />
		<% } %> </td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.LevelName) %></td>
				<td><%: Model.OlympicDetails.LevelName  %></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.OlympicYear) %></td>
				<td><%: Model.OlympicDetails.OlympicYear%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.OlympicDetails.OrganizerName) %></td>
				<td><%: Model.OlympicDetails.OrganizerName  %></td>
			</tr>
		</tbody>
	</table>
</div>

