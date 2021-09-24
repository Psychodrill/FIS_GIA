<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="PersonalRecordsData" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsData.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Личная информация
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <gv:PersonalRecordsData runat="server" />

</asp:Content>
