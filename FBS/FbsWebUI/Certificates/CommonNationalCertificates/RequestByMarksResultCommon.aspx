<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/CertificatesResult.master" 
    AutoEventWireup="true" CodeBehind="RequestByMarksResultCommon.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByMarksResultCommon" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphSearchDataSource">
    <%-- Источники данных для интерактивных проверок --%>
    <%--4.1.4. Запрос по ФИО и баллам по предметам [dbo].[SingleCheck_FioSubjectsMarks]--%>
    <asp:SqlDataSource runat="server" ID="dsSearch_FIOSubjectMarks" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="[dbo].[SingleCheck_FioSubjectsMarks]" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" EnableCaching="True" 
        CacheDuration="300" CacheExpirationPolicy="Sliding">
        <SelectParameters>
            <asp:Parameter Name="senderType" Type="Int32" DefaultValue="1" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="surname" Type="String"
                QueryStringField="LastName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="name" Type="String"
                QueryStringField="FirstName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="secondName" Type="String"
                QueryStringField="PatronymicName" />
            <asp:QueryStringParameter Name="subjectsMarks" Type="String" Size="1024" QueryStringField="SubjectMarks" />
            <fbs:CurrentUserParameter Name="login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
