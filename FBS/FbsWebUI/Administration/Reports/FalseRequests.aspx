<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FalseRequests.aspx.cs"
    Inherits="Fbs.Web.Administration.Reports.FalseRequests" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <link media="all" href="/Common/Styles/ReportStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="AjaxScriptManager" AsyncPostBackTimeout="0" runat="server" />
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" SizeToReportContent="True"
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" ProcessingMode="Remote" ShowCredentialPrompts="False"
        ShowFindControls="False" ShowBackButton="False" >
    </rsweb:ReportViewer>
    </form>
</asp:Content>
