<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="ApplicationDocuments" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationDocuments.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ввод заявления
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <gv:ApplicationDocuments runat="server" />
</asp:Content>