<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestSuccess.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CompetitionCertificates.BatchRequestSuccess"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <p>Пакет успешно добавлен на проверку.</p>
    <p><a href="/Certificates/CompetitionCertificates/BatchRequestResult.aspx?id=<%= Request.QueryString["id"] %>">Просмотреть результаты проверки пакета</a></p>
    <p><a href="/Certificates/CompetitionCertificates/BatchRequest.aspx">Вернуться к списку пакетов</a></p>
</asp:Content>