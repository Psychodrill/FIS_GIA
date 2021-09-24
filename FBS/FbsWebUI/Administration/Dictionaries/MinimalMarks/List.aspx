<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" 
    Inherits="Fbs.Web.Administration.Dictionaries.MinimalMarks.List" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>


<asp:Content runat="server" ContentPlaceHolderID="cphContent">

<form runat="server">
    <span style="font-size:medium;">Выберите год: </span> <asp:DropDownList runat="server" ID="ddlYears" 
        DataSourceID="dsMinimalMarksYears" DataTextField="year" DataValueField="year" AutoPostBack="true" /><br /><br />
        
    <asp:DataGrid runat="server" id="dgMinimalMarks"
        DataSourceID="dsMinimalMarks"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        
        GridLines="None"
        CssClass="table-th"
        >
        <HeaderStyle CssClass="th" />
        <Columns>
            
            <asp:TemplateColumn>
            <HeaderStyle CssClass="left-th" />
            <HeaderTemplate>
                <div>Предмет</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("Name")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <HeaderTemplate>
                <div>Балл</div>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:TextBox id="tbMinimalMarkValue" runat="server" Text='<%# Eval("MinimalMark")%>' Width="4em" ></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbMinimalMarkValue" 
                EnableClientScript="true" Display="Dynamic"
                    ValidationExpression="^\d{1,2}([.,]\d)*$" 
                    ErrorMessage='oшибка'
                    />
                <asp:HiddenField ID="hfMinimalMarkId" runat="server" Value='<%# Eval("Id")%>' />
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <asp:Button runat="server" Text="Сохранить" ID="btnSave" 
        onclick="btnSave_Click" />
</form>
    
    <asp:SqlDataSource runat="server" ID="dsMinimalMarks" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchMinimalMark" CancelSelectOnNullParameter="False"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlYears" DefaultValue="2009" Name="year" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:Parameter Name="getAvailableYears" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource runat="server" ID="dsMinimalMarksYears" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchMinimalMark" CancelSelectOnNullParameter="False"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <asp:Parameter Name="getAvailableYears" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>    

</asp:Content>
