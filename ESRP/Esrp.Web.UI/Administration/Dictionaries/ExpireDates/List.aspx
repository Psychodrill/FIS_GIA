<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Esrp.Web.Administration.Dictionaries.ExpireDates.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>


<asp:Content runat="server" ContentPlaceHolderID="cphContent">

<form runat="server">
    
    <asp:DataGrid runat="server" id="dgExpireDates"
        DataSourceID="dsExpireDates"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th"
        >
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle width="0%" CssClass="left-th" />
            <HeaderTemplate>
                <div>Год выдачи</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("Year") %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <HeaderTemplate >
                <div>Срок действия</div>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:TextBox runat="server" ID="tbExpireDate" Text='<%#Eval("ExpireDate")%>' Width="7em"></asp:TextBox>
                <asp:HiddenField ID="hfExpireDateYear" runat="server" Value='<%# Eval("year")%>' />
                <asp:CompareValidator id="dateValidator" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="tbExpireDate" ErrorMessage="неверная дата" Display="Dynamic" EnableClientScript="true"></asp:CompareValidator>
                <asp:RegularExpressionValidator runat="server" ValidationExpression="^\d{2}.\d{2}.\d{4}$" ControlToValidate="tbExpireDate" Display="Dynamic" EnableClientScript="true" ErrorMessage="(дд.мм.гггг)" />
            </ItemTemplate>
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>
    <asp:Button ID="btnSave" runat="server" text="Сохранить" 
        onclick="btnSave_Click"/>
    <asp:SqlDataSource runat="server" ID="dsExpireDates" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SearchExpireDate" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
    </asp:SqlDataSource>

</form>
    
    </asp:Content>
