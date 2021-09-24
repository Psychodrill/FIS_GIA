<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultExportCsvExtended.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultExportCsvExtended" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<asp:sqldatasource runat="server" id="dsResultsList" connectionstring="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    selectcommand="dbo.SearchCommonNationalExamCertificateRequest" cancelselectonnullparameter="false"
    selectcommandtype="StoredProcedure" OnSelecting="dsResultsList_Selecting"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
        <asp:Parameter Name="isExtended" Type="Boolean" DefaultValue="true" />
    </SelectParameters>
</asp:sqldatasource>
