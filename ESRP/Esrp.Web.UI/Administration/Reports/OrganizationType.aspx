<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationType.aspx.cs"
    MasterPageFile="~/Common/Templates/Administration.Master" Inherits="Esrp.Web.Administration.Reports.OrganizationType" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphThirdLevelMenu">
    <div class="third_line">
        <div class="max_width">
            <esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="report" HeaderTemplate="<ul>"
                FooterTemplate="</ul>" />
            <div class="clear">
            </div>
        </div>
    </div>
    <!--bottom_line-->
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <form runat="server">
    <div class="main_table" style="width: 50%">
        <asp:DataGrid runat="server" ID="dgReport" DataSourceID="dsReport" AutoGenerateColumns="false"
            EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th"
            Width="100%">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle Width="50%" CssClass="left-th" />
                    <HeaderTemplate>
                        <div style="font-weight: bold;">
                            Тип ОУ</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("TypeName") + "&nbsp;" + Eval("OPF")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Wrap="false">
                    <HeaderStyle Width="50%" CssClass="right-th" />
                    <HeaderTemplate>
                        <div style="font-weight: bold;">
                            Количество</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("UsersCount")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <web:NoRecordsText runat="server" ControlId="dgReport">
        <p class="notfound">
            Нет данных</p>
    </web:NoRecordsText>
    <asp:SqlDataSource runat="server" ID="dsReport" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="dbo.GetOrganizationTypeReport" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </form>
</asp:Content>
