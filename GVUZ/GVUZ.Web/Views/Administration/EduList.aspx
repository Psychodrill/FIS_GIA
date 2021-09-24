<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.UserViewModel[]>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Administration" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Администрирование - пользователи.
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle">Пользователи ОО</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
	<% ViewData["MenuItemID"] = 0; %>
	<gv:AdminMenuControl runat="server" />	
	<table class="gvuzDataGrid">
		<thead>
			<tr>
				<th>					
					Логин
				</th>
				<th>
					Ф.И.О.
				</th>
				<th>
					Должность
				</th>
				<th>
					Роли
				</th>
			</tr>
		</thead>
		<tbody>
		    <% var isAdmin = UserRole.CurrentUserInRole(UserRole.EduAdmin); %>
			<% foreach(UserViewModel user in Model) { %>
			<tr>
				<td class="caption">
				    <% if (isAdmin)
					   { %>
					<%= Url.GenerateLink<AdministrationController>(x => x.EditFbdUser(user.UserID), user.Login) %>
                    <% } else
					   { %>
                    <%: user.Login %>
                    <% } %>
				</td>
				<td>
					<%: user.FullName %>
				</td>
				<td>
					<%: user.Position %>
				</td>
				<td>
					<%: user.roles %>
				</td>
			</tr>
			<% } %>
		</tbody>
	</table>
	</div>
<%--
	<div class="rightAlign">
		<input type="button" id="addUser" value="Добавить пользователя" onclick="javascript:alert('Не реализовано')" />
	</div>
--%>
</asp:Content>
