<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="EntrantLanguageView" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/EntrantLanguageView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ForeignLanguage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <gv:EntrantLanguageView ID="EntrantLanguageView" runat="server" />

</asp:Content>
