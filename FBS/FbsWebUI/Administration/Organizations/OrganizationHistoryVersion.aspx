<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHistoryVersion.aspx.cs"
    Inherits="Fbs.Web.Administration.Organizations.OrganizationHistoryVersion" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register src="~/Controls/OrganizationView.ascx" TagName="OrganizationView" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <uc:OrganizationView ID="OrganizationView" runat="server" />
</asp:Content>
