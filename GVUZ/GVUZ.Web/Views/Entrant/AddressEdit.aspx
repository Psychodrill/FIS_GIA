<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagName="EntrantPersonalDataAddressEdit" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsAddress.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Адрес
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <gv:EntrantPersonalDataAddressEdit ID="editAddress" runat="server" />

</asp:Content>

