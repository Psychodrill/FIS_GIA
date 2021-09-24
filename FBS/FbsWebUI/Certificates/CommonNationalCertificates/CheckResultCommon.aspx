<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/CertificatesResult.master" 
    AutoEventWireup="true" CodeBehind="CheckResultCommon.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.CheckResultCommon" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphSearchDataSource">
    <%-- Источники данных для интерактивных проверок --%>
    <%--4.1.1. Запрос по регистрационному номеру и ФИО  [dbo].[SingleCheck_LicenseNumberFio]--%>
    <asp:SqlDataSource runat="server" ID="dsSearch_FIOLicenseNumber" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="[dbo].[SingleCheck_LicenseNumberFio]" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" EnableCaching="True" CacheDuration="300" CacheExpirationPolicy="Sliding">
        <SelectParameters>
            <asp:Parameter Name="senderType" Type="Int32" DefaultValue="1" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="licenseNumber" Type="String"
                QueryStringField="number" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="surname" Type="String"
                QueryStringField="LastName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="name" Type="String"
                QueryStringField="FirstName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="secondName" Type="String"
                QueryStringField="PatronymicName" />
            <fbs:CurrentUserParameter Name="login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
