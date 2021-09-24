<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteSimple.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
 ФИС ЕГЭ и приема: Авторизация
</asp:Content>

<script runat="server">
	protected string EsrpPath
	{
		get { return (string)HttpContext.Current.Items["ESRPRedirectPath"]; }
	}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		</h2>
	<p>
		Идет перенаправление на страницу аутентификации. Если страница аутентификации не отображается в течение 5 секунд, нажмите <a href="<%= EsrpPath %>">здесь</a>.
		</p>
<script type="text/javascript">
	setTimeout(function ()
	{
		window.location = '<%= EsrpPath %>'
	}, 600)
</script>
</asp:Content>
