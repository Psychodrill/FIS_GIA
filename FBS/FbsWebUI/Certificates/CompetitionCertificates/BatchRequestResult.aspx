<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CompetitionCertificates.BatchRequestResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
<% if (!HasResults) {%>
    <meta http-equiv="refresh" content="3" />
<%} %>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
            <li><a href="/Certificates/CompetitionCertificates/BatchRequestExportCsv.aspx?id=<%= Request.QueryString["id"] %>" 
                title="Экспорт в CSV" class="gray">Экспорт в CSV</a></li>
        </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>    
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <table class="pager">
    <tr><td>

        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsResultsListCount"
            StartRowIndexParamName="start" 
            MaxRowCountParamName="count"
            HideDefaultTemplates="true"
            AlwaysShow="true"
            DefaultMaxRowCount="20"
            DefaultMaxRowCountSource="Cookies">
            <Header>
                Результаты запросов #StartRowIndex#-#LastRowIndex# из #TotalCount#.
        
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
                    DataSourcePagerId="DataSourcePagerHead">
                <Header>Записей на странице: </Header>
                <Footer>.</Footer>
                <Separator>, </Separator>
                <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                </web:DataSourceMaxRowCount>                 
            </Header>
        </web:DataSourcePager>

    </td>    
    <td align="right">

        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
            StartRowIndexParamName="start"
            MaxRowCountParamName="count">
			<PrevGroupTemplate></PrevGroupTemplate>
            <NextGroupTemplate></NextGroupTemplate>
            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
        </web:DataSourcePager>    

    </td></tr>
    </table>
    
    <asp:DataGrid runat="server" id="dgResultsList"
            DataSourceID="dsResultsList"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle CssClass="left-th" />
            <HeaderTemplate>
                <div>Степень</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsExist")) ? Convert.ToString(Eval("Degree")) : "<span style='color:red'>Не найдено</span>" %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Олимпиада
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("CompetitionTypeName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Фамилия
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName")%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Имя
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("FirstName")%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Отчество
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("PatronymicName")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderTemplate>
                Регион
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("RegionName")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderTemplate>
                Город
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("City")%>
            </ItemTemplate>
            </asp:TemplateColumn>
              
            <asp:TemplateColumn>
            <HeaderTemplate>
                Школа
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("School")%>
            </ItemTemplate>
            </asp:TemplateColumn> 
                     
            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <HeaderTemplate><div>Класс &nbsp;</div></HeaderTemplate>
            <ItemTemplate>
                <%# Eval("Class")%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgResultsList">
        <style type="text/css">#ExportContainer, #ResultContainer {display:none;}</style> 
        <p class="notfound">Не обработано ни одной записи</p>
    </web:NoRecordsText>    
    
    <table class="pager">
    <tr><td>

        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
            StartRowIndexParamName="start" 
            MaxRowCountParamName="count"
            HideDefaultTemplates="true"
            AlwaysShow="true"
            DefaultMaxRowCount="20"
            DefaultMaxRowCountSource="Cookies">
            <Header>
                Результаты запросов #StartRowIndex#-#LastRowIndex# из #TotalCount#.
        
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
                    DataSourcePagerId="DataSourcePagerHead">
                <Header>Записей на странице: </Header>
                <Footer>.</Footer>
                <Separator>, </Separator>
                <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                </web:DataSourceMaxRowCount>                 
            </Header>
        </web:DataSourcePager>

    </td>    
    <td align="right">

        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
            StartRowIndexParamName="start"
            MaxRowCountParamName="count">
			<PrevGroupTemplate></PrevGroupTemplate>
            <NextGroupTemplate></NextGroupTemplate>
            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
        </web:DataSourcePager>    

    </td></tr>
    </table>

    <asp:SqlDataSource  runat="server" ID="dsResultsList" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCompetitionCertificateRequest"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:QueryStringParameter DefaultValue="1" Name="startRowIndex" Type="String" QueryStringField="start" />
            <asp:QueryStringParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource  runat="server" ID="dsResultsListCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCompetitionCertificateRequest"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:Parameter Name="showCount" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>