<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAccountActivityByRegion.aspx.cs" 
    MasterPageFile="~/Common/Templates/Administration.Master" 
    Inherits="Esrp.Web.Administration.Reports.UserAccountActivityByRegion" %>

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
<form runat="server">

    <asp:DataGrid runat="server" id="dgReport"
        DataSourceID="dsReport"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th" Width="70%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="20%" CssClass="left-th" />
            <HeaderTemplate>
                <div>Код региона</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsTotal")) ? String.Empty : 
                       Convert.ToString(Eval("RegionCode")) %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="60%"/>
            <HeaderTemplate>
                <div>Наименование региона</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsTotal")) ? "<b>Итого</b>" :
                        Convert.ToString(Eval("RegionName")) %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle Width="15%" CssClass="right-th"/>
            <HeaderTemplate>
                <div>Количество</div>
            </HeaderTemplate>
            <ItemTemplate>
                &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("Count")%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgReport">
        <p class="notfound">Нет данных</p>
    </web:NoRecordsText>

    <asp:SqlDataSource runat="server" ID="dsReport" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="dbo.GetUserAccountActivityByRegionReport" 
        CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" > 
    </asp:SqlDataSource>

</form>
</asp:Content>
