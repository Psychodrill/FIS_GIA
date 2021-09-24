<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostponeReport.aspx.cs" Inherits="Esrp.Web.Administration.Reports.PostponeReport" MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
<link media="all" href="/Common/Styles/ReportStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphThirdLevelMenu">
    <div class="third_line">
        <div class="max_width">
            <esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="report"
                HeaderTemplate="<ul>" FooterTemplate="</ul>" />
            <div class="clear">
            </div>
        </div>
    </div>
    <!--bottom_line-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="AjaxScriptManager" AsyncPostBackTimeout="0" runat="server" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" 
            SizeToReportContent="True" Font-Names="Verdana" Font-Size="8pt" 
            InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" ProcessingMode="Remote">
        </rsweb:ReportViewer>
    </form>
</asp:Content>