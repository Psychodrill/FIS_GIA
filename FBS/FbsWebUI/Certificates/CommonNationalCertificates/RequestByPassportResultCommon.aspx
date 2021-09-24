<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/CertificatesResult.master" 
    AutoEventWireup="true" CodeBehind="RequestByPassportResultCommon.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByPassportResultCommon" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphSearchDataSource">

    <%-- Источники данных для интерактивных проверок --%>
    <%--4.1.3. Запрос по ФИО, номеру и серии документа [dbo].[SingleCheck_FioDocumentNumberSeries]--%>
    <asp:SqlDataSource runat="server" ID="dsSearch_FIOPassport" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="[dbo].[SingleCheck_FioDocumentNumberSeries]" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" EnableCaching="True" CacheDuration="300" CacheExpirationPolicy="Sliding">
        <SelectParameters>
            <asp:Parameter Name="senderType" Type="Int32" DefaultValue="1" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="surname" Type="String"
                QueryStringField="surname" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="name" Type="String"
                QueryStringField="name" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="secondName" Type="String"
                QueryStringField="secondName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="documentNumber" Type="String"
                QueryStringField="documentNumber" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="documentSeries" Type="String"
                QueryStringField="documentSeries" />
            <fbs:CurrentUserParameter Name="login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>