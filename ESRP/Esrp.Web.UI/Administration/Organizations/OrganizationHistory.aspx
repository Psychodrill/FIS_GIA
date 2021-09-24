<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHistory.aspx.cs"
    Inherits="Esrp.Web.Administration.Organizations.OrganizationHistory" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphHead">

    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            }
            cuSel(params);
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form id="Form2" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Организация&nbsp;<%= this.currentOrg.FullName %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <% if (User.IsInRole("CreateOrganization"))
                       { %>
                    <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%=this.currentOrg.Id%>"
                        class="gray"><span>Изменить</span></a>
                    <% } %>
                    <a href="#" title="История изменений" class="active"><span>История изменений</span></a>
                </p>
                <div class="clear">
                </div>
            </div>
            <div class="main_table">
                <div class="sort table_header">
                    <div class="sorted">
                        <web:DataSourcePager ID="DataSourcePager2" runat="server" DataSourceId="DataSourcePagerHead"
                            StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                            AlwaysShow="true">
                            <Header>
                                <div class="sort_page">
                                    <select id="selectPageCount" onchange="changePageCount()">
                                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                                            DataSourcePagerId="DataSourcePagerHead">
                                            <Footer>
                                            </Footer>
                                            <Separator>
                                            </Separator>
                                            <ActiveTemplate>
                                                <option selected="selected" value="#count#"><span>#count#</span></option>
                                            </ActiveTemplate>
                                            <InactiveTemplate>
                                                <option value="#count#"><span>#count#</span></option>
                                            </InactiveTemplate>
                                            <%--InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">#count#</a>
                                        </InactiveTemplate--%>
                                        </web:DataSourceMaxRowCount>
                                    </select>
                                </div>
                            </Header>
                        </web:DataSourcePager>
                        <p class="rec">
                            Записей на странице:&nbsp;
                        </p>
                        <p class="views">
                            <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsHistoryListCount"
                                StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                                AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                                <Header>
                                    Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.
                                </Header>
                            </web:DataSourcePager>
                        </p>
                        <p class="page_nav">
                            <web:DataSourcePager ID="DataSourcePager1" runat="server" DataSourceId="DataSourcePagerHead"
                                StartRowIndexParamName="start" AlwaysShow="false" MaxRowCountParamName="count">
                                <Header>
                                    Страницы&nbsp;</Header>
                                <PrevGroupTemplate>
                                </PrevGroupTemplate>
                                <NextGroupTemplate>
                                </NextGroupTemplate>
                                <FirstPageTemplate>
                                    <a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                                <LastPageTemplate>
                                    &nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                                <ActivePrevPageTemplate>
                                    <a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                                <ActivePageTemplate>
                                    <span>#PageNo#</span>
                                </ActivePageTemplate>
                                <ActiveNextPageTemplate>
                                    &nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                            </web:DataSourcePager>
                        </p>
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <asp:DataGrid runat="server" ID="dgHistoryList" DataSourceID="dsHistoryList" AutoGenerateColumns="false"
                    UseAccessibleHeader="true" EnableViewState="false" ShowHeader="True" GridLines="None"
                    CssClass="table-th" Width="100%">
                    <HeaderStyle CssClass="actions" />
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle Width="5%" />
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
                            <HeaderStyle Width="15%" />
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
                            <HeaderStyle Width="20%" />
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
                    <div class="sort table_footer">
                        <div class="f_left">
                        </div>
                        <div class="sorted">
                            <web:DataSourcePager ID="DataSourcePager5" runat="server" DataSourceId="DataSourcePagerHead"
                                StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                                AlwaysShow="true">
                                <Header>
                                    <div class="sort_page">
                                        <select id="selectPageCountBottom" onchange="changePageCount(this)">
                                            <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server" Variants="20,50,100"
                                                DataSourcePagerId="DataSourcePagerHead">
                                                <Footer>
                                                </Footer>
                                                <Separator>
                                                </Separator>
                                                <ActiveTemplate>
                                                    <option selected="selected" value="#count#"><span>#count#</span></option>
                                                </ActiveTemplate>
                                                <InactiveTemplate>
                                                    <option value="#count#"><span>#count#</span></option>
                                                </InactiveTemplate>
                                                <%--InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">#count#</a>
                                        </InactiveTemplate--%>
                                            </web:DataSourceMaxRowCount>
                                        </select>
                                    </div>
                                </Header>
                            </web:DataSourcePager>
                            <p class="rec">
                                Записей на странице:&nbsp;
                            </p>
                            <p class="views">
                                <web:DataSourcePager runat="server" ID="DataSourcePager6" DataSourceId="DataSourcePagerHead"
                                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                                    <Header>
                                        Показано <span>#StartRowIndex#-#LastRowIndex#</span> из #TotalCount#.
                                    </Header>
                                </web:DataSourcePager>
                            </p>
                            <p class="page_nav">
                                <web:DataSourcePager ID="DataSourcePager7" runat="server" DataSourceId="DataSourcePagerHead"
                                    StartRowIndexParamName="start" AlwaysShow="false" MaxRowCountParamName="count">
                                    <Header>
                                        Страницы&nbsp;</Header>
                                    <PrevGroupTemplate>
                                    </PrevGroupTemplate>
                                    <NextGroupTemplate>
                                    </NextGroupTemplate>
                                    <FirstPageTemplate>
                                        <a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
                                    <LastPageTemplate>
                                        &nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
                                    <ActivePrevPageTemplate>
                                        <a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
                                    <ActivePageTemplate>
                                        <span>#PageNo#</span>
                                    </ActivePageTemplate>
                                    <ActiveNextPageTemplate>
                                        &nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
                                </web:DataSourcePager>
                            </p>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
    <asp:ObjectDataSource runat="server" ID="dsHistoryList" DataObjectTypeName="Esrp.Core.Organizations.OrganizationUpdateHistoryEntry"
        TypeName="Esrp.Core.Organizations.OrganizationDataAccessor" SelectMethod="SelectOrgUpdateHistory"
        OnSelecting="dsHistoryList_OnObjectCreating"></asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="dsHistoryListCount" SelectMethod="SelectOrgUpdateHistoryCount"
        TypeName="Esrp.Core.Organizations.OrganizationDataAccessor" OnSelecting="dsHistoryListCount_OnObjectCreating">
    </asp:ObjectDataSource>
</asp:Content>
