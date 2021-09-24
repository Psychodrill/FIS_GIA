<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="EntrantDocumentsListEdit" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/EntrantDocumentsListEdit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Список документов
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <gv:EntrantDocumentsListEdit runat="server" />

</asp:Content>
