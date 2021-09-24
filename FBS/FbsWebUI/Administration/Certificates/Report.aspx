<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" 
    MasterPageFile="~/Common/Templates/Administration.Master" 
    Inherits="Fbs.Web.Administration.Certificates.Report" %>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<link type="text/css" rel="stylesheet" href="/Common/Styles/visualize.jQuery.css"/>
	<link type="text/css" rel="stylesheet" href="/Common/Styles/visualize.css"/>
	<script type="text/javascript" src="/Common/Scripts/jquery.min.js"></script>
	<!--[if IE]><script type="text/javascript" src="/Common/Scripts/excanvas.compiled.js"></script><![endif]-->
	<script type="text/javascript" src="/Common/Scripts/visualize.jQuery.js"></script>
	<script type="text/javascript">
		$(function(){
			//make some charts
			$('table.visualizeme').visualize({type: 'line', parseDirection: 'y', width: 700, height: 300, lineWeight: 2});
		});
	</script>
    <form runat="server">
    <asp:Repeater runat="server" DataSourceID="dsReportWeekly">
        <HeaderTemplate>
            <table class="visualizeme">
                <caption>
                    Динамика загрузки свидетельств за неделю</caption>
                <thead>
                    <tr>
                        <td>
                        </td>
                        <th>
                            Новых свидетельств
                        </th>
                        <th>
                            Обновленных свидетельств
                        </th>
                        <th>
                            Новых аннулирований
                        </th>
                        <th>
                            Обновленных аннулирований
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <th title="<%# Eval("day") %>">
                    <%# Eval("date") %>
                </th>
                <td>
                    <%# Eval("cnecNew")%>
                </td>
                <td>
                    <%# Eval("cnecUpdated")%>
                </td>
                <td>
                    <%# Eval("cnecdNew")%>
                </td>
                <td>
                    <%# Eval("cnecdUpdated")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater runat="server" DataSourceID="dsReportMonthly">
        <HeaderTemplate>
            <table class="visualizeme">
                <caption>
                    Динамика загрузки свидетельств за месяцку</caption>
                <thead>
                    <tr>
                        <td>
                        </td>
                        <th>
                            Новых свидетельств
                        </th>
                        <th>
                            Обновленных свидетельств
                        </th>
                        <th>
                            Новых аннулирований
                        </th>
                        <th>
                            Обновленных аннулирований
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <th title="<%# Eval("day") %>">
                    <%# Eval("date") %>
                </th>
                <td>
                    <%# Eval("cnecNew")%>
                </td>
                <td>
                    <%# Eval("cnecUpdated")%>
                </td>
                <td>
                    <%# Eval("cnecdNew")%>
                </td>
                <td>
                    <%# Eval("cnecdUpdated")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>    
    
    <asp:SqlDataSource runat="server" ID="dsReportWeekly" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="ReportCnecLoading" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="type" DefaultValue="week"/>
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsReportMonthly" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="ReportCnecLoading" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>    
    </form>
</asp:Content>
