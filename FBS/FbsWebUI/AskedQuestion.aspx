<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AskedQuestion.aspx.cs"
    Inherits="Fbs.Web.AskedQuestion" MasterPageFile="~/Common/Templates/Regular.master" %>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <p><%=CurrentAskedQuestion.Question %></p>
    <p><%=CurrentAskedQuestion.Answer %></p>
</asp:Content>