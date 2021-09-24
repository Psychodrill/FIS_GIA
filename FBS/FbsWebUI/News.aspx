<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Fbs.Web.News"
    MasterPageFile="~/Common/Templates/Regular.master" %>
    
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
<div id="NewsText">
    <%=CurrentNews.Text %>
</div>
</asp:Content>
