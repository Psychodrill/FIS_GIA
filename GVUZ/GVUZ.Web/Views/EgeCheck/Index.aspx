<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="FogSoft.Helpers" %>


<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Проверка ЕГЭ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="height: 33px"></div>
	<iframe src='<%= AppSettings.Get("EgeCheckPage", "") %>?ticket=<%= Request["esrpAuth"] %>' width="100%"
		height="600" frameborder="1" scrolling="auto"></iframe>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
Проверка результатов ЕГЭ</asp:Content>