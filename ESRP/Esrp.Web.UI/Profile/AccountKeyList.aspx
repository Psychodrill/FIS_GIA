<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyList.aspx.cs" 
    Inherits="Esrp.Web.Profile.AccountKeyList"
    MasterPageFile="~/Common/Templates/Personal.master" %>
    
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Parameters" Assembly="Esrp.Web.UI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %> 

<%@ Import Namespace="System.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content" id="JSPlaceHolder">
            <ul>
            <li><a href="/Profile/AccountKeyCreate.aspx" title="Создать ключ доступа">Создать ключ доступа</a></li>
            </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:DataGrid runat="server" id="dgAccountKeyList"
        DataSourceID="dsAccountKeyList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>

            <asp:TemplateColumn>
            <HeaderStyle Width="20%" CssClass="left-th" />
            <HeaderTemplate>
                <div>Ключ</div>
            </HeaderTemplate>
            <ItemTemplate>
                <a href="/Profile/AccountKeyEdit.aspx?key=<%# Eval("Key") %>" title="Редактировать ключ доступа" ><%# Eval("Key") %></a>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                Действителен с
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("DateFrom")) ? String.Empty : Convert.ToDateTime(Eval("DateFrom")).ToShortDateString()%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                Действителен по
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("DateTo")) ? String.Empty : Convert.ToDateTime(Eval("DateTo")).ToShortDateString() %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%" CssClass="right-th" />
            <HeaderTemplate>
                <div> Активен </div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsActive")) ? "Да" : "Нет" %>
            </ItemTemplate>
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgAccountKeyList">
        <Message><p class="notfound">Ключи доступа отсутствуют</p></Message>
    </web:NoRecordsText>
    
    <asp:SqlDataSource runat="server" ID="dsAccountKeyList" CancelSelectOnNullParameter="false"
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="dbo.SearchAccountKey"  SelectCommandType="StoredProcedure">
        <SelectParameters>
            <esrp:CurrentUserParameter Name="login" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</form>    
</asp:Content>
