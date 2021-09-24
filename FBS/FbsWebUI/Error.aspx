<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Fbs.Web.Error"
    MasterPageFile="~/Common/Templates/Regular.master" %>

<%@ Import Namespace="Fbs.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p class="l8">
        Произошла ошибка</p>
    <p class="l8"><asp:Label runat="server" ID="lMessgae" /></p>
</asp:Content>
