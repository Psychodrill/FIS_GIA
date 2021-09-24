<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagPrefix="gv" TagName="ApplicationIndividualAchivements" Src="~/Views/Shared/Portlets/Applications/ApplicationIndividualAchivements.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ввод заявления
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <gv:ApplicationIndividualAchivements runat="server" />
</asp:Content>