<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActivation.aspx.cs" 
    MasterPageFile="~/Common/Templates/Administration.Master" 
    Inherits="Esrp.Web.Administration.Reports.UserActivation" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
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
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
	<link type="text/css" rel="stylesheet" href="/Common/Styles/visualize.jQuery.css"/>
	<link type="text/css" rel="stylesheet" href="/Common/Styles/visualize.css"/>

	<!--[if IE]><script type="text/javascript" src="/Common/Scripts/excanvas.compiled.js"></script><![endif]-->
	<script type="text/javascript" src="/Common/Scripts/visualize.jQuery.js"></script>
	<script type="text/javascript">
		$(function(){
			//make some charts
			$('table.visualizeme').visualize({type: 'line', parseDirection: 'y', width: 700, height: 300, lineWeight: 2});
		});
	</script>
	
<form runat="server">



<asp:Repeater ID="rReportUserRegistration" runat="server" DataSourceID="dsReportUserRegistration" >
<HeaderTemplate>		
        <table class="visualizeme" >
			<caption>Динамика регистрации пользователей за месяц</caption>
			<thead>
				<tr>
					<td></td>
					<th>Зарегистрировано</th>
					<th>Активировано</th>
					<th>На рассмотрении</th>
					<th>На доработке</th>
					<th>Отключено</th>
				</tr>
			</thead>
			<tbody>
</HeaderTemplate>
<ItemTemplate>
    <tr>
        <th title="<%# Eval("day") %>"><%# Convert.ToDateTime(Eval("date")).ToShortDateString() %></th>
        <td><%# Eval("registration") %></td>
        <td><%# Eval("activated") %></td>
        <td><%# Eval("consideration") %></td>
        <td><%# Eval("revision") %></td>
        <td><%# Eval("deactivated") %></td>
    </tr>
</ItemTemplate>
<FooterTemplate>
			</tbody>
		</table>
</FooterTemplate>
</asp:Repeater>
  

    <web:NoRecordsText runat="server" ControlId="rReportUserRegistration">
        <p class="notfound">Нет регистрационной активности пользователей</p>
    </web:NoRecordsText>

    <asp:SqlDataSource runat="server" ID="dsReportUserRegistration" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="ReportUserRegistration" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
    </asp:SqlDataSource>

</form>
</asp:Content>
