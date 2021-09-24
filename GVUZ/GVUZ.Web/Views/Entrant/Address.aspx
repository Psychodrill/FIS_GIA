<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagName="EntrantPersonalDataAddress" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsAddressView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Адрес
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <gv:EntrantPersonalDataAddress ID="viewAddress" runat="server" />

</asp:Content>

