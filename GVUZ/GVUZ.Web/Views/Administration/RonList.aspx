<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.UserListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Administration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Подсистема администрирования - пользователи Рособрнадзора.
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Пользователи Рособрнадзора</h2>
	<table class="gvuzDataGrid">
		<thead>
			<tr>
				<th>
					<span class="linkSumulator" onclick="doSort(1)" id="spSort1">						
							<%: Html.LabelFor(x => x.DisplayModel.Login) %>						
					</span>
				</th>
				<th>
					<span class="linkSumulator" onclick="doSort(2)" id="spSort2">
						<%: Html.LabelFor(x => x.DisplayModel.FullName) %>
					</span>
				</th>
				<th>
					<%: Html.LabelFor(x => x.DisplayModel.RolesDict) %>
				</th>				
			</tr>
		</thead>
		<tbody>
			<% foreach(UserViewModel user in Model.Users) { %>
			<tr id="<%= user.UserID %>">
				<td class="caption">
					<a href="<%= Url.Generate<AdministrationController>(x => x.ViewUser(user.UserID)) %>">
						<%: user.Login %>
					</a>
				</td>
				<td>
					<%: user.FullName %>
				</td>
				<td>
					<%: user.GetUserRoles() %>
				</td>
			</tr>
			<% } %>
		</tbody>
	</table>

	<script type="text/javascript">
		jQuery(document).ready(function()
		{
			if(currentSortID != null)
			{
				if(currentSortID > 0)
					jQuery('#spSort' + currentSortID).after('<span class="sortUp"></span>')
				else
					jQuery('#spSort' + (-currentSortID)).after('<span class="sortDown"></span>')
			}
		})

		var currentSortID = <%= Model.SortID %>

		function doSort(sortID)
		{
			if (sortID == currentSortID) sortID= -sortID
			window.location = '<%= Url.Generate<AdministrationController>(x => x.RonList(null)) %>?sortID=' + sortID
		}

	</script>
</asp:Content>
