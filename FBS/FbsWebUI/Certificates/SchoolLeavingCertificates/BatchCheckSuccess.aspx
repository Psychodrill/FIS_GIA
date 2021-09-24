<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckSuccess.aspx.cs" 
    Inherits="Fbs.Web.Certificates.SchoolLeavingCertificates.BatchCheckSuccess" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <p>Пакет успешно добавлен на проверку.</p>
    <p><a href="/Certificates/SchoolLeavingCertificates/BatchCheckResult.aspx?id=<%= Request.QueryString["id"] %>">Просмотреть результаты проверки пакета</a></p>
    <p><a href="/Certificates/SchoolLeavingCertificates/BatchCheck.aspx">Вернуться к списку пакетов</a></p>
</asp:Content>