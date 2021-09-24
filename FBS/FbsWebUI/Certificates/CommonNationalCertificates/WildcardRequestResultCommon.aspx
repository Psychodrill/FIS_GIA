<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WildcardRequestResultCommon.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.WildcardRequestResultCommon" 
    MasterPageFile="~/Common/Templates/CertificatesResult.master" %>


<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphSearchDataSource">
    <%--4.1.5. Запрос по неполным данным [dbo].[SingleCheck_Wildcard]--%>
    <asp:SqlDataSource runat="server" ID="dsSearch_Wildcard" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="[dbo].[SingleCheck_Wildcard]" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" EnableCaching="True" CacheDuration="300" CacheExpirationPolicy="Sliding">
        <SelectParameters>
            <asp:Parameter Name="senderType" Type="Int32" DefaultValue="1" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="surname" Type="String"
                QueryStringField="LastName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="name" Type="String"
                QueryStringField="FirstName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="secondName" Type="String"
                QueryStringField="PatronymicName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="documentNumber" Type="String"
                QueryStringField="DocNumber" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="documentSeries" Type="String"
                QueryStringField="DocSeries" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="licenseNumber" Type="String"
                QueryStringField="Number" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="typographicNumber"
                Type="String" QueryStringField="TypographicNumber" />
            <fbs:CurrentUserParameter Name="login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>