<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.CompetitiveGroupEditViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div class="content">
<table class="data">
	<tbody>
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.Name) %></td>
			<td><b><%: Model.Name %></b></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CourseID) %></td>
			<td><b><%: CompetitiveGroupExtensions.GetCourseName(Model.CourseID) %></b></td>
		</tr>
<%--		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationalLevelID) %></td>
			<td><b><%: Model.EducationLevelName %></b></td>
		</tr>--%>
	</tbody>
</table>

<table class="gvuzDataGrid" id="tableData">
	<thead>
		<tr>
			<th rowspan="3"><%: Html.LabelFor(x => x.DisplayData.Directions) %></th>
			<th colspan="<%= Model.HasBudget.Values.Max(x => x.CountOOZZ) %>" <%= Model.HasBudget.Values.Max(x => x.CountOOZZ) == 0 ? "style=\"display:none\"" : "" %>><%: Html.LabelFor(x => x.DisplayData.BudgetName) %></th>
			<th colspan="<%= Model.HasPaid.Values.Max(x => x.CountOOZZ) %>" <%= Model.HasPaid.Values.Max(x => x.CountOOZZ) == 0 ? "style=\"display:none\"" : "" %>><%: Html.LabelFor(x => x.DisplayData.PaidName) %></th>
			<% if(Model.Organizations.Length > 0 && Model.HasTarget.Values.Max(x => x.CountOOZZ) > 0) { %>
			<th colspan="<%= Model.Organizations.Length * Model.HasTarget.Values.Max(x => x.CountOOZZ) %>" id="thTargetCaption"><%: Html.LabelFor(x => x.DisplayData.TargetName) %></th>
			<%} %>
		</tr>
		<tr id="trCaptionSecond">
			<% if(Model.HasBudget.Values.Any(x => x.HasO)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetO) %></th><%} %>
			<% if(Model.HasBudget.Values.Any(x => x.HasOZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetOZ) %></th><%} %>
			<% if(Model.HasBudget.Values.Any(x => x.HasZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetZ) %></th><%} %>
			<% if(Model.HasPaid.Values.Any(x => x.HasO)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidO) %></th><%} %>
			<% if(Model.HasPaid.Values.Any(x => x.HasOZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ) %></th><%} %>
			<% if(Model.HasPaid.Values.Any(x => x.HasZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ) %></th><%} %>
			<% if(Model.HasTarget.Values.Max(x => x.CountOOZZ) > 0) { foreach (var org in Model.Organizations) { %>
			<th colspan="<%= Model.HasPaid.Values.Max(x => x.CountOOZZ) %>"><%: org.Name %></th>
<% }} %>
		</tr>
		<tr id="trCaptionThird">
			<th colspan="<%= Model.HasBudget.Values.Max(x => x.CountOOZZ) + Model.HasPaid.Values.Max(x => x.CountOOZZ) %>" <%= Model.HasBudget.Values.Max(x => x.CountOOZZ) + Model.HasPaid.Values.Max(x => x.CountOOZZ) == 0 ? "style=\"display:none\"" : "" %>></th>
			<% if(Model.HasTarget.Values.Max(x => x.CountOOZZ) > 0) { foreach (var org in Model.Organizations) { %>
			<% if(Model.HasTarget.Values.Any(x => x.HasO)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidO) %></th><%} %>
			<% if(Model.HasTarget.Values.Any(x => x.HasOZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ) %></th><%} %>
			<% if(Model.HasTarget.Values.Any(x => x.HasZ)) { %> <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ) %></th><%} %>
<% }} %>
		</tr>
	</thead>
	<tbody>
		<% foreach (var dirInfo in Model.Rows) { %>
			<tr>
				<td><%: dirInfo.DirectionName %></td>
				<% for(var i = 0; i < dirInfo.Data.Length; i++)
				   {
					   var doTD = (Model.HasBudget.Values.Any(x => x.HasO) && i == 0) 
                                    || (Model.HasBudget.Values.Any(x => x.HasOZ) && i == 1) 
                                    || (Model.HasBudget.Values.Any(x => x.HasZ) && i == 2)
					                || (Model.HasPaid.Values.Any(x => x.HasO) && i == 3) 
                                    || (Model.HasPaid.Values.Any(x => x.HasOZ) && i == 4) 
                                    || (Model.HasPaid.Values.Any(x => x.HasZ) && i == 5)
					                || (Model.HasTarget.Values.Any(x => x.HasO) && i % 3 == 0 && i >= 6) 
                                    || (Model.HasTarget.Values.Any(x => x.HasOZ) && i % 3 == 1 && i >= 6) 
                                    || (Model.HasTarget.Values.Any(x => x.HasZ) && i % 3 == 2 && i >= 6);
					   var showVal = doTD
					                 && ((i == 0 && Model.HasBudget.ContainsKey(dirInfo.EducationLevelID) && Model.HasBudget[dirInfo.EducationLevelID].HasO)
                                        || (i == 1 && Model.HasBudget.ContainsKey(dirInfo.EducationLevelID) && Model.HasBudget[dirInfo.EducationLevelID].HasOZ)
                                        || (i == 2 && Model.HasBudget.ContainsKey(dirInfo.EducationLevelID) && Model.HasBudget[dirInfo.EducationLevelID].HasZ)
                                        || (i == 3 && Model.HasPaid.ContainsKey(dirInfo.EducationLevelID) && Model.HasPaid[dirInfo.EducationLevelID].HasO)
                                        || (i == 4 && Model.HasPaid.ContainsKey(dirInfo.EducationLevelID) && Model.HasPaid[dirInfo.EducationLevelID].HasOZ)
                                        || (i == 5 && Model.HasPaid.ContainsKey(dirInfo.EducationLevelID) && Model.HasPaid[dirInfo.EducationLevelID].HasZ)
                                        || (i % 3 == 0 && i >= 6 && Model.HasTarget.ContainsKey(dirInfo.EducationLevelID) && Model.HasTarget[dirInfo.EducationLevelID].HasO)
                                        || (i % 3 == 1 && i >= 6 && Model.HasTarget.ContainsKey(dirInfo.EducationLevelID) && Model.HasTarget[dirInfo.EducationLevelID].HasOZ)
                                        || (i % 3 == 2 && i >= 6 && Model.HasTarget.ContainsKey(dirInfo.EducationLevelID)) && Model.HasTarget[dirInfo.EducationLevelID].HasZ);
		   if (doTD)
		   {
		    %>
				<td align="center"><%: showVal ? dirInfo.Data[i].ToString() : "" %></td>
				 <%}} %>
			</tr>
<%} %>
	</tbody>
</table>
</div>
