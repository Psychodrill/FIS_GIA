<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" 
    Inherits="Fbs.Web.Personal.Profile.Edit"
    MasterPageFile="~/Common/Templates/Personal.master" %>
<%@ OutputCache VaryByParam="none" Duration="1" %>    
<%@ Import Namespace="Fbs.Core" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
<%-- 
    Выбор контрола редактирования, в зависимости от типа пользователя, происходит в CodeBehind классе 
--%>
</form>    
</asp:Content>

