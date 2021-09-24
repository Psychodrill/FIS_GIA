<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.PreparatoryCourseViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<table class="gvuzDataGrid" cellpadding="3">
	<thead>
		<tr>
			<th>
				<%= Html.LabelFor(x => x.CourseName) %>
			</th>
			<th>
				<%= Html.LabelFor(x => x.Subjects) %>
			</th>
			<th>
				<%= Html.LabelFor(x => x.Information) %>
			</th>
			<th>
				<%= Html.LabelFor(x => x.AdditionalInfo) %>
			</th>
		</tr>
	</thead>
	<tbody>
		<% foreach (var course in Model.Courses) {%>
		<tr>
			<td><%: course.Name %></td>
			<td><%: course.Subjects %></td>
			<td><%: course.Information %></td>
			<td><a href="<%var c1 = course;%><%= Url.Generate<BaseController>(x => x.GetFile1(c1.FileID)) %>"><%: course.FileName %></a></td>
		</tr>
		<%} %>
	</tbody>
</table>

