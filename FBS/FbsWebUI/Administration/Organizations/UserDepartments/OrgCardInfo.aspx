<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="OrgCardInfo.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.UserDepartments.OrgCardInfo" %>
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
