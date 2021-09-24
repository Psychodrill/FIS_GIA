<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagName="ApplicationPersonalData" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationPersonalData.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ввод заявления
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <gv:ApplicationPersonalData runat="server" />
</asp:Content>