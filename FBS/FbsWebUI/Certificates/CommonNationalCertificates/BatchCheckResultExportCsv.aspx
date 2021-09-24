<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckResultExportCsv.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchCheckResultExportCsv" %>
<%@ Import Namespace="Fbs.Web"%>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI"%>
<asp:SqlDataSource  runat="server" ID="dsResultsList"
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateCheck"  CancelSelectOnNullParameter="false" 
    SelectCommandType="StoredProcedure" OnSelecting="dsResultsList_Selecting"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters>
</asp:SqlDataSource>