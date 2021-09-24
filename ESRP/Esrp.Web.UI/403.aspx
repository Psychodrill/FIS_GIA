<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Main.Master" AutoEventWireup="true" CodeBehind="403.aspx.cs" Inherits="Esrp.Web._403" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContentModalDialog" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSecondLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphThirdLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContent" runat="server">
<h1><font color="red">Ошибка 403</font></h1><asp:Label runat="server" Text="доступ запрещен" ID="errorMessage"/>
</asp:Content>
