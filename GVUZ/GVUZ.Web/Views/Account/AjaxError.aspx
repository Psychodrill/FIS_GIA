<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteSimple.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
 ФИС ЕГЭ и приема: Ошибка запроса
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server" type="text/C#">
	protected override void OnLoad(EventArgs e)
	{
 		Response.StatusCode = 500;
		base.OnLoad(e);
	}
</script>
	<h2>
		Ошибка при запросе</h2>
	<p>
		При обращении к серверу произошла ошибка. 
		</p>
</asp:Content>
