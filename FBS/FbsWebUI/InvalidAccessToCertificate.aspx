<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvalidAccessToCertificate.aspx.cs" Inherits="Fbs.Web.InvalidAccessToCertificate"
    MasterPageFile="~/Common/Templates/Regular.master" %>

<%@ Import Namespace="Fbs.Web" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <p class="l8">
        Доступ к сертификату запрещен</p>
    <p class="l8"><asp:Label runat="server" ID="lMessgae" /></p>
</asp:Content>
