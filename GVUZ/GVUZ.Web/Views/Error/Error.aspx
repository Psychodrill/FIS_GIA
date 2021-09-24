<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
 ФИС ЕГЭ и приема: Ошибка
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
		<%= Html.Encode(ViewData["Title"]) %></h2>
	<p>
		<%= Html.Encode(ViewData["Description"])%></p>
	<div>
		Перейти на страницу <%= Url.Link<InstitutionController>(c => c.View(null), "общей информации") %> образовательного учреждения.
	</div>
	<div>
		Перейти на страницу <%= Url.Link<StructureController>(c => c.Index(), "структуры") %> образовательного учреждения.
	</div>
<%--
	<script type="text/javascript">
		$('.instSelector').hide();
	</script>
--%>
</asp:Content>
