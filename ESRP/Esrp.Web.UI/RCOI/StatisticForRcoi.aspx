<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatisticForRcoi.aspx.cs" Inherits="Esrp.Web.StatisticForRcoi"
    MasterPageFile="~/Common/Templates/Regular.master" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
    <script src="/Common/Scripts/FixPng.js" type="text/javascript"></script>
</asp:Content>  

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphContent">
    <form id="form1" runat="server">
    <asp:LinkButton ID="LBReportOrgsInfoByRegionTVF" runat="server" 
        Text="Отчет о текущей активности образовательных учреждений, находящихся на территории субъекта РФ"
        CommandArgument="ReportOrgsInfoByRegionTVF" onclick="LBReportOrgsInfoByRegionTVF_Click">
    </asp:LinkButton><br />
</form>
</asp:Content>