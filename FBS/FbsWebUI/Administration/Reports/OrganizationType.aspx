<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationType.aspx.cs" 
    MasterPageFile="~/Common/Templates/Administration.Master" 
    Inherits="Fbs.Web.Administration.Reports.OrganizationType" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphContent" runat="server">
<form runat="server">

    <asp:DataGrid runat="server" id="dgReport"
        DataSourceID="dsReport"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th" Width="50%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="50%" CssClass="left-th" />
            <HeaderTemplate>
                <div>Тип ОУ</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("TypeName") + "&nbsp;" + Eval("OPF")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn ItemStyle-Wrap="false">
            <HeaderStyle Width="50%" CssClass="right-th"/>
            <HeaderTemplate>
                <div>Количество</div>
            </HeaderTemplate>
            <ItemTemplate>
                &nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("UsersCount")%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgReport">
        <p class="notfound">Нет данных</p>
    </web:NoRecordsText>

    <asp:SqlDataSource runat="server" ID="dsReport" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.GetOrganizationTypeReport" 
        CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" > 
    </asp:SqlDataSource>

</form>
</asp:Content>
