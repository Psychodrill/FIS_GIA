<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EducationalOrganization.aspx.cs" 
    Inherits="Fbs.Web.Administration.Reports.EducationalOrganization" %>
<%@ Import Namespace="Fbs.Utility" %>
<% 
    
    // Установлю хидеры страницы
    ResponseWriter.PrepareHeaders("BatchRequestResult.xls", "application/vnd.xls", 
          Encoding.GetEncoding(1251));

%>
<html>
<body>
    <asp:DataGrid runat="server"
        DataSourceID="dsReport"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True">
        <Columns>
            <asp:TemplateColumn>
            <HeaderTemplate>Название организации</HeaderTemplate>
            <ItemTemplate><%# Eval("OrganizationName") %></ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <Columns>
            <asp:TemplateColumn>
            <HeaderTemplate>Тип ОУ</HeaderTemplate>
            <ItemTemplate><%# Eval("EducationInstitutionType")%></ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <Columns>
            <asp:TemplateColumn>
            <HeaderTemplate>Регион</HeaderTemplate>
            <ItemTemplate><%# Eval("RegionName")%></ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</body>
</html>
<asp:SqlDataSource runat="server" ID="dsReport" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.GetEducationalOrganizationReport" 
    CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure" > 
</asp:SqlDataSource>