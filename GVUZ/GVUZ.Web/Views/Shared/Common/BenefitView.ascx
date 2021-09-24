<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.BenefitViewModel>" %>
<table class="gvuzDataGrid" cellpadding="3">
	<thead>
		<tr>
			<th>
				<%= Html.LabelFor(x => x.DiplomaType) %>
			</th>
			<th>
				<%= Html.LabelFor(x => x.ComptetitionLevel) %>
			</th>
			<th>
				<%= Html.LabelFor(x => x.BenefitType) %>
			</th>
		</tr>
	</thead>
	<tbody>
		<% foreach (var benefit in Model.Benefits) { %>
		<tr>
			<td>
				<%= benefit.DiplomaType %>
			</td>
			<td align="center">
				<%= benefit.CompetitionLevel %>
			</td>
			<td>
				<%= benefit.BenefitType %>
			</td>
		</tr>
		<% } %>
	</tbody>
</table>

