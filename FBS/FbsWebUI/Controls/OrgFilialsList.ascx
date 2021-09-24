<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgFilialsList.ascx.cs" Inherits="Fbs.Web.Controls.OrgFilialsList" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="asp" Namespace="WebControls" Assembly="WebControls" %>

<asp:DataGrid AutoGenerateColumns="false" CssClass="table-th" DataSourceID="filialsDataSource"
              EnableViewState="false" GridLines="None" ID="FilialsTable" runat="server" ShowHeader="True"
              ToolTip="Филиалы" Width="100%">
    <HeaderStyle CssClass="th" />
    <Columns>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="left-th" />
            <HeaderTemplate>
                <div>
                    <fbs:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="FullName" SortExprText="Id"
                                        Prefix="../../../Common/Images/" />
                </div>
            </HeaderTemplate>
            <ItemStyle CssClass="left-aligned" />
            <ItemTemplate>
                <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%#this.Eval("Id")%>">
                    <%#Convert.ToString(this.Eval("Id"))%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" Width="100%" />
            <HeaderTemplate>
                <div>
                    <fbs:SortRef_Prefix ID="SortRef_Prefix1" runat="server" SortExpr="UserCount" SortExprText="Название"
                                        Prefix="../../../Common/Images/" />
                </div>
            </HeaderTemplate>
            <ItemStyle CssClass="left-aligned" />
            <ItemTemplate>
                <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%#this.Eval("Id")%>">
                    <%#Convert.ToString(this.Eval("FullName"))%></a>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<asp:NoRecordsText ID="NoRecordsText1" runat="server" ControlId="FilialsTable">
    <Message>
        Филиалы не найдены.
    </Message>
</asp:NoRecordsText>

<asp:SqlDataSource runat="server" ID="filialsDataSource" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
                   OnSelecting="DataSourceOnSelectingHandler" SelectCommand="SELECT [FullName], [Id] FROM [dbo].[Organization2010] WHERE MainId = @MainOrgId">

    <SelectParameters>
        <asp:Parameter Name="MainOrgId" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>