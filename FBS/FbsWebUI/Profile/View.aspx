<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Fbs.Web.Personal.Profile.View"
    MasterPageFile="~/Common/Templates/Personal.master" %>
<%@ OutputCache Location="None"  %>

<%@ Register TagPrefix="fbs" TagName="IntrantProfileView" Src="/Controls/IntrantProfileView.ascx" %>
<%@ Register TagPrefix="fbs" TagName="UserProfileView" Src="/Controls/UserProfileView.ascx" %>
<%@ Import Namespace="Fbs.Core" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
<%-- 
    Выбор контрола просмотра, в зависимости от типа пользователя, происходит в CodeBehind классе 
--%>
</form> 
</asp:Content>

