<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
 ФИС ЕГЭ и приема: Ошибка авторизации
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server" type="text/C#">
	protected override void OnLoad(EventArgs e)
	{
 		Response.StatusCode = 500;
		base.OnLoad(e);
	}
	protected string ESRPPath
	{
		get 
		{
			string path = ConfigurationManager.AppSettings["ESRPAuth.Path"];
			if (!path.EndsWith("/"))
				path += "/";
			return path;
		}
	}
</script>
	<h2>
		Ошибка авторизации</h2>
	<p>
		<% if(ViewData["StatusID"] != null && Convert.ToInt32(ViewData["StatusID"]) == 2) { %>
			Вы зашли под пользователем, который не имеет доступа к данной системе. <a href="<%= Url.Generate<AccountController>(x => x.LogOff()) %>">Сменить пользователя</a>
		<%} %>
		<% if(ViewData["StatusID"] != null && Convert.ToInt32(ViewData["StatusID"]) == 10) { %>
			При входе в систему произошла ошибка обновления данных о пользователе и/или об организации.
			Пожалуйста, обратитесь к <a href="<%=ESRPPath %>support.aspx">оператору Горячей линии</a> Единой системы регистрации пользователей.
			<%--<a href="<%= Url.Generate<AccountController>(x => x.LogOff()) %>">Сменить пользователя</a>--%>
		<%} %>
		<% if(ViewData["StatusID"] != null && Convert.ToInt32(ViewData["StatusID"]) == 11) { %>
			Данный пользователь не прикреплён к ОО. Доступ запрещён. <a href="<%= Url.Generate<AccountController>(x => x.LogOff()) %>">Сменить пользователя</a>
		<%} %>
		<% if(ViewData["StatusID"] != null && Convert.ToInt32(ViewData["StatusID"]) == 3) { %>
			
		<%} %>
		<% if(ViewData["StatusID"] != null && Convert.ToInt32(ViewData["StatusID"]) == 4) { %>
			Нет доступа к данной странице
		<%} %>
		</p>
</asp:Content>
