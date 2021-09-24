<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultExport.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultExport" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Utility" %>
<% 
    
    // Установлю хидеры страницы
    ResponseWriter.PrepareHeaders("BatchRequestByPassportResult.xls", "application/vnd.xls", 
          Encoding.GetEncoding(1251));

%>
<html>
<body>
    <asp:DataGrid runat="server"
        DataSourceID="dsResultsList"
        AutoGenerateColumns="false" 
        ShowHeader="True" > 
        <Columns>
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div><nobr>Свидетельство</nobr></div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsDeny")) ? string.Format("<span style=\"color:Red\" title='Свидетельство №{0} аннулировано по следующей причине: {1}'>{0} (аннулировано)</span>", Eval("CertificateNumber"), Convert.ToString(Eval("DenyComment"))) :
                    (!Convert.ToBoolean(Eval("IsExist")) ? string.Format("<span style=\"color:Red\" title='Свидетельство не найдено'>Не&nbsp;найдено</span>", Eval("CertificateNumber")) : Eval("CertificateNumber"))%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Фамилия</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Имя</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("FirstName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Отчество</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("PatronymicName")%>
            </ItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                <div>Регион</div>
            </HeaderTemplate>
            <ItemTemplate>
               <%# Convert.ToString(Eval("RegionName")) %>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
</body>
</html>

<asp:SqlDataSource  runat="server" ID="dsResultsList" 
    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
    SelectCommand="dbo.SearchCommonNationalExamCertificateRequest"  CancelSelectOnNullParameter="false"
    SelectCommandType="StoredProcedure"> 
    <SelectParameters>
        <fbs:CurrentUserParameter Name="login" Type="String" />
        <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
    </SelectParameters>
</asp:SqlDataSource>
