<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="EntrantApplicationList" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/EntrantApplicationList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Список заявлений
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <gv:EntrantApplicationList ID="EntrantApplicationList1" runat="server" />

</asp:Content>
