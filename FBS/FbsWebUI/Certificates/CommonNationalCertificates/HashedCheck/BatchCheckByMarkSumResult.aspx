<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Certificates.Master" AutoEventWireup="true" CodeBehind="BatchCheckByMarkSumResult.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.BatchCheckByMarkSumResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">
 <% if (this.InProgress)
      {%>
    <meta http-equiv="refresh" content="10"/>
    <%} %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCertificateActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphCertificateContent" runat="server">

<asp:Panel runat="server" ID="waitBox" Visible="true">
    <p class="norecords">Осуществляется обработка файла</p>
</asp:Panel>   
<asp:Panel runat="server" ID="resultPanel" Visible="false">
    <p>Обработка выполнена</p>
    <asp:HyperLink runat="server" ID="ResultFileLink">Скачать результат</asp:HyperLink>
    <br />
    <br />
    <a href="BatchCheckByMarkSum.aspx">Вернуться назад</a>
</asp:Panel>
</asp:Content>
