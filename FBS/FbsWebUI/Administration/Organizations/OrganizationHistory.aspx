<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHistory.aspx.cs"
    Inherits="Fbs.Web.Administration.Organizations.OrganizationHistory" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsHistoryListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Изменения #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
			        <PrevGroupTemplate></PrevGroupTemplate>
                    <NextGroupTemplate></NextGroupTemplate>
                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <asp:DataGrid runat="server" ID="dgHistoryList" DataSourceID="dsHistoryList" AutoGenerateColumns="false"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle CssClass="left-th" Width="5%" />
                <HeaderTemplate>
                    <div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <a href='OrganizationHistoryVersion.aspx?OrgId=<%#Eval("Organization.Id")%>&Version=<%#Eval("Organization.Version")%>'>
                        просмотр </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="5%" />
                <HeaderTemplate>
                    Версия
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("Organization.Version")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    Дата
                </HeaderTemplate>
                <HeaderStyle Width="5%" />
                <ItemTemplate>
                    <%#Eval("Organization.UpdateDate", "{0:dd.MM.yyyy hh:mm:ss}")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderTemplate>
                    <div>
                        Изменения относительно следующей версии
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("UpdateDescription")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateColumn>

            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" Width="20%" />
                <HeaderTemplate>
                    <div>
                        Пользователь
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("EditorName")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateColumn>

        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText runat="server" ControlId="dgHistoryList">
        <Message>
            <p class="notfound">
                Изменений не было</p>
        </Message>
    </web:NoRecordsText>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count" HideDefaultTemplates="true" AlwaysShow="true" DefaultMaxRowCount="20"
                    DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Изменения #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
			        <PrevGroupTemplate></PrevGroupTemplate>
                    <NextGroupTemplate></NextGroupTemplate>
                    <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                    <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                    <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
                    <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource runat="server" ID="dsHistoryList" DataObjectTypeName="Fbs.Core.Organizations.OrganizationUpdateHistoryEntry"
        TypeName="Fbs.Core.Organizations.OrganizationDataAccessor" SelectMethod="SelectOrgUpdateHistory"
        OnSelecting="dsHistoryList_OnObjectCreating"></asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="dsHistoryListCount" SelectMethod="SelectOrgUpdateHistoryCount"
        TypeName="Fbs.Core.Organizations.OrganizationDataAccessor" OnSelecting="dsHistoryListCount_OnObjectCreating">
    </asp:ObjectDataSource>
</asp:Content>
